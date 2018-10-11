namespace AutoFish
{
    public struct Rect
    {
        public int LeftX;
        public int LeftY;
        public int RightX;
        public int RightY;

        public Rect(int x, int y, int right, int bottom)
        {
            LeftX = x;
            LeftY = y;
            RightX = right;
            RightY = bottom;
        }
    }
}
