// See https://aka.ms/new-console-template for more information
// Initialize DirectInput
using SharpDX.DirectInput;
using WindowsInput;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System.Diagnostics;
using Handbrake_to_Keyboard;
using Figgle;
using Pastel;
using System.Text.Json;

// Settings
string settingsPath = "settings.json";
AppSettings settings = new() 
{
    KeyboardMode = false,
    Verbose = false,
    FindDeviceMode = FindDeviceModes.ByName,
    TargetDevice = "DigiKey",
    TargetButton = JoystickOffset.Buttons0,
    SimKey = VirtualKeyCode.VK_A,
    SimButton = Xbox360Button.A
};

if (File.Exists(settingsPath))
{
    var readSettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(settingsPath));
    if (readSettings is not null)
    {
        settings = readSettings;
    }
    else
    {
        Console.WriteLine("Invalid settings file!");
        return;
    }
}
else
{
    File.WriteAllText(settingsPath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
}


string iconAuthor = "Freepik";

InputSimulator inputSim = new();
var directInput = new DirectInput();
var vigemClient = new ViGEmClient();
IXbox360Controller x360 = null;

var versionInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

Console.WriteLine(FiggleFonts.ThreePoint.Render($"{versionInfo.ProductName}").Pastel(ConsoleColor.Blue));
Console.WriteLine($"v.{versionInfo.ProductVersion} by {versionInfo.CompanyName}");
Console.WriteLine("https://gs2012.xyz");
Console.WriteLine("");
Console.WriteLine($"Application Icon by {iconAuthor}");
Console.WriteLine("");

if (!settings.KeyboardMode)
{
    Console.WriteLine("Starting in controller mode...");
    x360 = vigemClient.CreateXbox360Controller();
    x360.Connect();
}

// Find a Joystick Guid
var joystickGuid = Guid.Empty;


Console.WriteLine(Environment.NewLine + "==========================================" + Environment.NewLine);

foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad,
            DeviceEnumerationFlags.AllDevices))
{
    Console.WriteLine("Device: " + deviceInstance.InstanceName + " | GUID: " + deviceInstance.InstanceGuid);
    if (settings.FindDeviceMode == FindDeviceModes.ByName)
    {
        if (deviceInstance.InstanceName == settings.TargetDevice)
        {
            joystickGuid = deviceInstance.InstanceGuid;
            break;
        }
    }
    else if (settings.FindDeviceMode == FindDeviceModes.ByGuid)
    {
        if (deviceInstance.InstanceGuid == new Guid(settings.TargetDevice))
        {
            joystickGuid = deviceInstance.InstanceGuid;
            break;
        }
    }
}

if (joystickGuid == Guid.Empty)
{
    Console.WriteLine("Target device not found.");
    return;
}


var joystick = new Joystick(directInput, joystickGuid);

//Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

joystick.Properties.BufferSize = 128;
joystick.Acquire();

bool isKeyPressed = false;
Thread keyPressThread = new Thread(() =>
{
    while (HandbrakeManager.isRunning)
    {
        if (isKeyPressed)
        {
            inputSim.Keyboard.KeyDown(settings.SimKey);
            
        }
        else
        {
            inputSim.Keyboard.KeyUp(settings.SimKey);
        }
        Thread.Sleep(10);
    }

    
});
if (settings.KeyboardMode)
{
    Console.WriteLine("Keyboard mode enabled.");
    keyPressThread.Start();
}


Console.WriteLine("Starting... Press Q to exit.");
Thread haltThread = new Thread(HandbrakeManager.ListenForHalt);
haltThread.Start();
while (HandbrakeManager.isRunning)
{

    try
    {
        joystick.Poll();
        var datas = joystick.GetBufferedData();
        foreach (var state in datas)
        {
            if (state.Offset == settings.TargetButton)
            {
                if (state.Value == 128)
                {
                    if (settings.Verbose) Console.WriteLine("Handbrake pressed!");
                    isKeyPressed = true;
                    if (x360 is not null)
                    {
                        x360.SetButtonState(settings.SimButton, true);
                    }
                }
                else if (state.Value == 0)
                {
                    if (settings.Verbose) Console.WriteLine("Handbrake released!");
                    isKeyPressed = false;
                    if (x360 is not null)
                    {
                        x360.SetButtonState(settings.SimButton, false);
                    }
                }

            }
        }

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
joystick.Unacquire();
joystick.Dispose();
if (x360 is not null)
{
    Console.WriteLine("Disconnecting controller...");
    x360.Disconnect();
}



static class HandbrakeManager
{
    public static bool isRunning = true;
    public static void ListenForHalt()
    {
        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                //Console.WriteLine($"Key pressed: {key.Key}");
                if (key.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("Exiting...");
                    isRunning = false;
                    //Environment.Exit(0);
                }

            }
            Task.Delay(100).Wait();
        }
    }
}

