namespace AutoFish
{
    using System.Threading;

    public enum MIT
    {
        Click,
        Delay
    }

    public class MacroInput
    {
        public MIT mit { get; }
        private int delay;
        private Point point;

        public MacroInput(int delay)
        {
            this.mit = MIT.Delay;
            this.delay = delay;
        }

        public MacroInput(Point p)
        {
            this.point = p;
            this.mit = MIT.Click;
        }

        public void Excecute()
        {
            if ( mit == MIT.Click )
                MouseHooker.LeftClick(point);
            else if ( mit == MIT.Delay )
                Thread.Sleep(delay);
        }
    }
}
