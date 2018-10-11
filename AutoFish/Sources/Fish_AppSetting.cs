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
        public static List<Rect> RectList = new List<Rect>();
        public static List<MacroInput> InputList = new List<MacroInput>();
    }
}

