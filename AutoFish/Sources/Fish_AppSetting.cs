using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AutoFish
{
    public class AppSetting
    {
        public static int Accuracy = 80;
        public static int LeftX = 500;
        public static int LeftY = 500;
        public static int RightX = 1200;
        public static int RightY = 800;
        public static Point LeftFishPoint = new Point(-1, -1);
        public static Point RightFishPoint = new Point(-1, -1);
        public static Point LeftBoxSpritePoint = new Point(-1, -1);
        public static Point RightBoxSpritePoint = new Point(-1, -1);
        public static int MinDelay =0 ;
        public static int MaxDelay = 0;
        public static List<Rect> RectList = new List<Rect>();
    }
}

