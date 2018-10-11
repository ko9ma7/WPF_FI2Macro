namespace AutoFish
{
    public class Tools
    {
        private static Retangle rectange = null;

        public static void ViewSearchRange()
        {
            if ( rectange is null )
            {
                rectange = new Retangle(new Rect(AppSetting.LeftX, AppSetting.LeftY, AppSetting.RightX, AppSetting.RightY));
                rectange.Show();
            }
            else
            {
                rectange.Close();
                rectange = null;
            }
        }

        public static bool IsViewing()
        {
            if ( rectange is null )
                return false;
            return true;
        }

        public static void WindowTopMost()
        {
            if ( !MainWindow.mainWindow.Topmost )
                MainWindow.mainWindow.Topmost = true;
            else
                MainWindow.mainWindow.Topmost = false;
        }
    }
}
