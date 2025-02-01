using Nefarius.ViGEm.Client.Targets.Xbox360;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace Handbrake_to_Keyboard
{
    public class AppSettings
    {
        public required bool KeyboardMode { get; set; } // false
        public bool Verbose { get; set; } = false;
        public FindDeviceModes FindDeviceMode { get; set; } = FindDeviceModes.ByName;

        // Name or Guid of the device to find (based on FindDeviceMode)
        public required string TargetDevice { get; set; } // "DigiKey"

        // Button press to capture
        public required JoystickOffset TargetButton { get; set; }//= JoystickOffset.Buttons0;

        // SimKey is used in KeyboardMode, SimButton is used in ControllerMode
        public VirtualKeyCode SimKey { get; set; } = VirtualKeyCode.VK_A;
        public Xbox360Button SimButton { get; set; } = Xbox360Button.A;
    }
    public enum FindDeviceModes
    {
        ByGuid,
        ByName
    }
}
