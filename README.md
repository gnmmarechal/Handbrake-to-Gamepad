# Handbrake to Gamepad

- Explain what it is and why


## Settings File Format

The settings file must be in the same directory as the program's executable and be named "settings.json". As the name would imply, it is a JSON-formatted file.

### Example settings file

```
{
  "KeyboardMode": false,
  "Verbose": false,
  "FindDeviceMode": 1,
  "TargetDevice": "DigiKey",
  "TargetButton": 48,
  "SimKey": 65,
  "SimButton": {
    "Value": 4096,
    "Name": "A",
    "Id": 11
  }
}
```
### Explanation

- **KeyboardMode** : Set this to true if you want to emulate a keyboard rather than an Xbox 360 controller.
- **Verbose** : It'll just write to the console everytime your device's target button press is detected.
- **FindDeviceMode** : 1 if you want to find the device by name, 2 if you want to find it by GUID.
- **TargetButton** : The button to capture in integer form - refer to [this](https://github.com/sharpdx/SharpDX/blob/master/Source/SharpDX.DirectInput/JoystickOffset.cs) page to find it.
- **SimKey** : The keyboard button to simulate if in keyboard mode - refer to [this](https://gitlab.com/SchwingSK/Keyboard2Xinput/-/blob/master/virtualKeyNames.md) page to find it.
- **SimButton** : The Xbox 360 button to simulate if in controller mode - refer to vigemKeys.md to find your desired output.

### To Do
- Add a configuration utility to make it easier to set up the settings file.
- Support axis mapping for progressive handbrakes.