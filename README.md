# Handbrake to Gamepad

This is a utility that maps a handbrake's input to a keyboard key or an Xbox 360 controller button. It uses the SharpDX library to interface with DirectInput devices and the ViGEmClient library to emulate an Xbox 360 controller.

## Why?

Games like Forza Horizon 4 refuse to recognise my ATTiny85-based handbrake as an input device. This is a workaround for that.

## Settings File Format

The settings file must be in the same directory as the program's executable and be named "settings.json". As the name would imply, it is a JSON-formatted file. If no file is found, the program will create one with default values as described below.

The default values reflect my own usage scenario, mapping my handbrake's input button to the A button on an Xbox 360 controller.

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

## To Do
- Add a configuration utility to make it easier to set up the settings file.
- Support axis mapping for progressive handbrakes.

## Releases

- Download it here: [v.1.0](https://dl.gs2012.xyz/?id=73)
## Dependencies

- [ViGEmBus driver](https://github.com/nefarius/ViGEmBus/releases)

## Issues

- Forza Horizon 4 doesn't seem to support multiple controllers properly, it'll switch between them whenever there's an input. This means that, for example, when holding the handbrake, the wheel input will be disregarded until you submit a wheel input again. I'm unsure whether this is solvable without emulating the whole wheel and binding the handbrake to one of the buttons of the emulated wheel instead.