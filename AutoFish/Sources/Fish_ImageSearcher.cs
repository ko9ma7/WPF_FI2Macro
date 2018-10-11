namespace AutoFish
{
    using System;
    using System.Runtime.InteropServices;

    class ImageSearcher
    {
        [DllImport("ImageSearchDLL.dll")]
        private static extern IntPtr ImageSearch(int x, int y, int right, int bottom, [MarshalAs(UnmanagedType.LPStr)]string imagePath);

        public static String[] Search(string imgPath)
        {
            IntPtr result = ImageSearch(AppSetting.LeftX, AppSetting.LeftY, AppSetting.RightX, AppSetting.RightY, "*" + +AppSetting.Accuracy + " " + imgPath);
            String res = Marshal.PtrToStringAnsi(result);
            if ( res[0] == '0' ) 
                return null;
            return res.Split('|');
        }

        public static String[] Search(int x, int y, int right, int bottom, string imgPath)
        {
            IntPtr result = ImageSearch(x, y, right, bottom, "*" + +AppSetting.Accuracy + " " + imgPath);
            String res = Marshal.PtrToStringAnsi(result);
            if ( res[0] == '0' )
                return null;
            return res.Split('|');
        }
    }
}
