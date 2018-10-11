using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AutoFish
{
    /// <summary>
    /// Retangle.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Retangle : Window
    {
        public bool MouseClicked = false;
        private DispatcherTimer ScheDuller = new DispatcherTimer();
        private Thread UpdateThread = null; //귀찮아서 그냥 쓰레드에 박음 ㅋㅋ

        public Retangle(Rect rect)
        {
            InitializeComponent();
            this.Left = rect.LeftX;
            this.Top = rect.LeftY;
            this.Width = Math.Abs( rect.RightX - rect.LeftX );
            this.Height = Math.Abs ( rect.RightY - rect.LeftY );
            RecodBorderSizeObject.Width = Math.Abs(rect.RightX - rect.LeftX);
            RecodBorderSizeObject.Height = Math.Abs(rect.RightY - rect.LeftY);
            MouseDown += Window_MouseDown;
            MouseUp += Finder_MouseUp;
            Closing += Window_Closing;
            StartThread();
            this.Topmost = true;
        }

        private void StartThread()
        {
            UpdateThread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            AppSetting.LeftX = ( int )this.Left;
                            AppSetting.LeftY = ( int )this.Top;
                            AppSetting.RightX = ( int )this.Left + ( int )this.Width;
                            AppSetting.RightY = ( int )this.Top + ( int )this.Height;
                        });
                        MainWindow.mainWindow.Dispatcher.Invoke(() =>
                        {
                            MainWindow.mainWindow.UpdateSetting();
                            MainWindow.mainWindow.TextBox_LeftX.Text = (( int )this.Left).ToString();
                            MainWindow.mainWindow.TextBox_LeftY.Text = (( int )this.Top).ToString();
                            MainWindow.mainWindow.TextBox_RightX.Text = (( int )this.Left + ( int )this.Width).ToString();
                            MainWindow.mainWindow.TextBox_RightY.Text = (( int )this.Top + ( int )this.Height).ToString();
                        }); 
                        Thread.Sleep(100);
                    }
                }
                catch 
                {
                }
            });
            UpdateThread.Start();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if ( UpdateThread != null )
                UpdateThread.Abort();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ( e.ChangedButton == MouseButton.Left )
            {
                MouseClicked = true;
                this.DragMove();
            }
        }

        private void Finder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseClicked = false;
            ScheDuller.Stop();
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MouseClicked = true;

            RecodBorderSizeObject.Width = e.NewSize.Width;
            RecodBorderSizeObject.Height = e.NewSize.Height;
        }
    }
}
