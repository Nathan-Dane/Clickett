using Clickett.Native;

namespace Clickett.Models
{
    public sealed class ClickProfile
    {
        public bool UseIntervalInput { get; set; }
        public bool LockToLocation { get; set; }
        public bool RocketMode { get; set; }
        public bool Jitter { get; set; }
        public bool DoubleClick { get; set; }

        public int ClickInterval { get; set; }
        public ClickMode Mode { get; set; }
        public int BurstCount { get; set; }
        public int Threads { get; set; }

        public uint XPosition { get; set; }
        public uint YPosition { get; set; }

        public MouseButtonType MouseButton { get; set; } = MouseButtonType.Left;

        public uint ClickDownFlag
        {
            get
            {
                return MouseButton switch
                {
                    MouseButtonType.Left => NativeConstants.LeftMouseDown,
                    MouseButtonType.Middle => NativeConstants.MiddleMouseDown,
                    MouseButtonType.Right => NativeConstants.RightMouseDown,
                    _ => NativeConstants.LeftMouseDown
                };
            }
        }

        public uint ClickUpFlag
        {
            get
            {
                return MouseButton switch
                {
                    MouseButtonType.Left => NativeConstants.LeftMouseUp,
                    MouseButtonType.Middle => NativeConstants.MiddleMouseUp,
                    MouseButtonType.Right => NativeConstants.RightMouseUp,
                    _ => NativeConstants.LeftMouseUp
                };
            }
        }

        public bool IsBurstMode => Mode == ClickMode.Burst;
        public bool IsToggleMode => Mode == ClickMode.Toggle;
        public bool IsHoldMode => Mode == ClickMode.Hold;
    }
}
