namespace AutoFish
{
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class MouseHooker
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        public static uint ABSOLUTE_SIZE = 65535;
        private const uint MOUSEEVENTF_MOVE = 0x01;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;

        public static int DisplayWidth = Screen.PrimaryScreen.Bounds.Width;
        public static int DisplayHeight = Screen.PrimaryScreen.Bounds.Height;

        public static void MoveTo(Point p, bool debug = false)
        {
            uint X = ( uint )(ABSOLUTE_SIZE * ( uint )p.X / DisplayWidth ) + 1;
            uint Y = ( uint )(ABSOLUTE_SIZE * ( uint )p.Y / DisplayHeight) + 1;

            if (debug)
                MainWindow.mainWindow.WriteLog("Real p " + X + " / " + Y);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, X, Y, 0, 0);
        }

        public static void LeftClick(Point p)
        {
            MoveTo(p);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void LeftDoubleClick(Point p)
        {
            MoveTo(p);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(150);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }



        public static void RightClick(Point p)
        {
            MoveTo(p);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static void DoubleClick(Point p)
        {
            MoveTo(p);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            Thread.Sleep(150);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
    }
}
