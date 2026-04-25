namespace Clickett.Native
{
    public static class NativeConstants
    {
        public const int GwlExStyle = -20;
        public const int WsExTransparent = 0x00000020;

        public const int ModAlt = 0x0001;
        public const int ModControl = 0x0002;
        public const int ModShift = 0x0004;
        public const int ModNoRepeat = 0x4000;

        public const uint LeftMouseDown = 0x02;
        public const uint LeftMouseUp = 0x04;
        public const uint RightMouseDown = 0x08;
        public const uint RightMouseUp = 0x10;
        public const uint MiddleMouseDown = 0x20;
        public const uint MiddleMouseUp = 0x40;
    }
}
