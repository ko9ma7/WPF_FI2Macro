namespace AutoFish
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Diagnostics;
    using System.Security.Principal;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using Microsoft.VisualBasic;
    using System.Threading;
    using Form = System.Windows.Forms;

    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow = null;

        public MainWindow()
        {
            //PriviliegeChecking();
            InitializeComponent();
            LoadSettings();
            UpdateSetting();
            UpdateTT();
            mainWindow = this;
            this.Topmost = true;
            var Handling_hotkey = new HotKey(Key.Subtract, KeyModifier.None, HandlingOnHotKeyHandler);
            

        }

        private void HandlingOnHotKeyHandler(HotKey obj)
        {
            if ( GameManager.Macro.IsWorking() )
                GameManager.Macro.Stop();
            else
                GameManager.Macro.Start();
        }



        #region 관리자권한 체크
        private void PriviliegeChecking()
        {
            bool isElevated = false;
            using ( WindowsIdentity identity = WindowsIdentity.GetCurrent() )
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            if ( isElevated == false )
            {
                MessageBox.Show("관리자 권한으로 실행해주세요");
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region 로그
        public void WriteLog(string Log)
        {
            ListView_Log.Dispatcher.Invoke(new Action(() =>
            {
                if ( ListView_Log.Items.Count > 50 )
                    ListView_Log.Items.RemoveAt(0);

                ListViewItem item = new ListViewItem();
                if ( Log.Contains("<Fail>") )
                    item.Background = new SolidColorBrush(Color.FromArgb(60, 230, 0, 0));
                else if ( Log.Contains("<Success>") )
                    item.Background = new SolidColorBrush(Color.FromArgb(60, 0, 230, 0));
                item.Content = "[" +  DateTime.Now.ToShortTimeString() + "] " + Log;
                ListView_Log.Items.Add(item);


                ListView_Log.ScrollIntoView(ListView_Log.Items[ListView_Log.Items.Count - 1]);
            }));
        }
        #endregion

        #region  Close
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GameManager.Macro.Stop();
            if ( Tools.IsViewing() )
                Tools.ViewSearchRange();
        }

        #endregion

        #region Tab_Main
        
        #region 상태 괴수 블록 수정
        public void ModifyBigMosnter(string st)
        {
            TextBlock_BigMonster.Dispatcher.Invoke(() =>
            {
                TextBlock_BigMonster.Text = st;
            });
        }
        #endregion

        #region 상태 어망 풀 블록 수정
        public void ModifySellFullFish(string st)
        {
            TextBlock_SellFullFish.Dispatcher.Invoke(() =>
            {
                TextBlock_SellFullFish.Text = st;
            });
        }
        #endregion

        #region 상태 낚시성공 텍스트 블록 수정
        public void ModifyGetSuccess(string st)
        {
            TextBlock_GetSuccess.Dispatcher.Invoke(() =>
            {
                TextBlock_GetSuccess.Text = st;
            });
        }
        #endregion

        #region 상태 낚시실패 텍스트 블록 수정
        public void ModifyGetFail(string st)
        {
            TextBlock_GetFail.Dispatcher.Invoke(() =>
            {
                TextBlock_GetFail.Text = st;
            });
        }
        #endregion

        private void Button_ImagePath_Click(object sender, RoutedEventArgs e)
        {
            try { Process.Start(GameManager.ImagesDirectory); }
            catch { MessageBox.Show("해당 경로가 존재하는지 확인해주세요\n" + GameManager.ImagesDirectory); }
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            if ( (string)Button_Start.Content == "시작")
                GameManager.Macro.Start();
            else if (( string )Button_Start.Content == "중지")
                GameManager.Macro.Stop();
        }


        #endregion

        #region Tab_Test
        private bool ImgCheck(Point p)
        {
            if ( !GameManager.IsExist(p) )
            {
                WriteLog("<Fail> : 파일이 존재하지 않습니다");
                return false;
            }
            if ( !GameManager.IsFind(p) )
            {
                WriteLog("<Fail> : 서치에 실패했습니다");
                return false;
            }
            return true;
        }

        private void Button_Test_Fail_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(GamePicture.Fail);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Button_Test_Success_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(GamePicture.Success);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }



        private void Num_0_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )0);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Num_1_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )1);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Num_2_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )2);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Num_3_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )3);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Num_4_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )4);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Num_5_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )5);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Num_6_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )6);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Num_7_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )7);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Num_8_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )8);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        private void Num_9_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(( GamePicture )9);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
        }

        //어망비우기
        Thread a = null;
        private void Button_FullFish_Click(object sender, RoutedEventArgs e)
        {
            if ( a is null )
            {
                WriteLog("어망 비우기 테스트 매크로 시작");
                int count = 0;
                a = new Thread(() => GameManager.Macro.FullFishCheckingThread(
                            ref count, GamePicture.FullFish, AppSetting.RectList[( int )RectEnum.FullFishStart]
                            ));
                a.Start();
            }
            else
            {
                if (a != null)
                {
                    WriteLog("어망 비우기 테스트 매크로 중지");
                    a.Abort();
                    a = null;
                }
            }

        }

        private void Button_FullFish1_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(AppSetting.RectList[( int )RectEnum.FullFishStart], GamePicture.FullFish);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
            MouseHooker.MoveTo(point, true);
        }

        private void Button_FullFish_AllSell_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(AppSetting.RectList[( int )RectEnum.FullFishContinue], GamePicture.FullFish_SellAll);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
            MouseHooker.MoveTo(point, true);
        }

        private void Button_FullFish_AllSellOk(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(AppSetting.RectList[( int )RectEnum.FullFishContinue], GamePicture.FullFish_SellAllOk);
            if ( !ImgCheck(point) ) return;
            MouseHooker.MoveTo(point, true);
        }
        private void Button_SellAllCheck_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(AppSetting.RectList[( int )RectEnum.FullFishContinue], GamePicture.FullFish_SellCheck);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
            MouseHooker.MoveTo(point, true);
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            Point point = GameManager.FindGamePicture(AppSetting.RectList[( int )RectEnum.FullFishContinue], GamePicture.Back);
            if ( !ImgCheck(point) ) return;
            WriteLog("<Success> : " + point.X + " / " + point.Y);
            MouseHooker.MoveTo(point, true);
        }
        #endregion

        #region Tab_Setting

        #region LoadSettings
        private void LoadSettings()
        {
            const string path = "Setting.json";
            const string rectPath = "RectSetting.txt";
            const string macroInputPath = "MacroInputPath.txt";
            if ( !File.Exists(path) )
                return;
            if ( !File.Exists(rectPath) )
            {
                MessageBox.Show("RectSetting.txt 파일이 없습니다");
                return;
            }
            if ( !File.Exists(macroInputPath) )
            {
                MessageBox.Show("MacroInputPath.txt 파일이 없습니다");
                return;
            }

            JObject obj = JObject.Parse(File.ReadAllText(path));
            AppSetting.Accuracy = int.Parse(obj["accuracy"].ToString());
            AppSetting.LeftX = int.Parse(obj["leftx"].ToString());
            AppSetting.LeftY = int.Parse(obj["lefty"].ToString());
            AppSetting.RightX = int.Parse(obj["rightx"].ToString());
            AppSetting.RightY = int.Parse(obj["righty"].ToString());
            String[] sp1 = Strings.Split(File.ReadAllText(rectPath), "\n");
            for ( int i = 0 ; i < sp1.Length ; i++ )
            {
                var splited = Strings.Split(sp1[i], ",");
                AppSetting.RectList.Add(new Rect(
                    int.Parse(splited[0]),
                    int.Parse(splited[1]),
                    int.Parse(splited[2]),
                    int.Parse(splited[3])
                    ));
            }

            String[] sp2 = Strings.Split(File.ReadAllText(macroInputPath), "\n");
            for ( int i = 0 ; i < sp2.Length ; i++ )
            {
                if ( sp2[i].Contains("m") )
                {
                    var str = Strings.Replace(sp2[i], "m", "");
                    String[] splited = Strings.Split(str, ",");
                    AppSetting.InputList.Add(new MacroInput(new Point(
                        int.Parse(splited[0]),
                        int.Parse(splited[1])
                        )));
                }
                else if ( sp2[i].Contains("d") )
                {
                    var str = Strings.Replace(sp2[i], "d", "");
                    AppSetting.InputList.Add(new MacroInput(
                        int.Parse(str)
                        ));
                }
            }
        }
        #endregion

        #region SaveSettings
        private void SaveSettings()
        {
            const string path = "Setting.json";
            JObject obj = new JObject();
            obj.Add("accuracy", AppSetting.Accuracy);
            obj.Add("leftx", AppSetting.LeftX);
            obj.Add("lefty", AppSetting.LeftY);
            obj.Add("rightx", AppSetting.RightX);
            obj.Add("righty", AppSetting.RightY);
            File.WriteAllText(path, obj.ToString());
        }
        #endregion

        private void TextBox_LeftX_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_LeftX.Text.Length <= 0 ||
                TextBox_LeftY.Text.Length <= 0 ||
                TextBox_RightX.Text.Length <= 0 ||
                TextBox_RightY.Text.Length <= 0)
            {
                MessageBox.Show("입력칸을 모두 채워주세요");
                return;
            }

            AppSetting.LeftX = int.Parse(TextBox_LeftX.Text);
            AppSetting.LeftY = int.Parse(TextBox_LeftY.Text);
            AppSetting.RightX = int.Parse(TextBox_RightX.Text);
            AppSetting.RightY = int.Parse(TextBox_RightY.Text);
            UpdateSetting();
            SaveSettings();
        }

        public void UpdateSetting()
        {
            TextBlock_Setting.Dispatcher.Invoke(() =>
            {
                TextBlock_Setting.Text =
                "Left X : " + AppSetting.LeftX + "\n" +
                "Left Y : " + AppSetting.LeftY + "\n" +
                "Right X : " + AppSetting.RightX + "\n" +
                "Right Y : " + AppSetting.RightY + "\n" +
                "Accuracy : " + AppSetting.Accuracy;
            });
            
        }

        private void Button_Apply2_Click(object sender, RoutedEventArgs e)
        {
            if ( TextBox_Accuracy.Text.Length <= 0 )
            {
                MessageBox.Show("입력칸을 모두 채워주세요");
                return;
            }

            AppSetting.Accuracy = int.Parse(TextBox_Accuracy.Text);
            UpdateSetting();
            SaveSettings();
        }
        #endregion

        #region Tab_Tools
        private void Button_ViewSearchRange_Click(object sender, RoutedEventArgs e)
        {
            Tools.ViewSearchRange();
        }

        private void Button_TopMost_Click(object sender, RoutedEventArgs e)
        {
            Tools.WindowTopMost();
        }



        private void UpdateTT()
        {
            TextBlock_tt.Text =
                MouseHooker.DisplayWidth + " / " + MouseHooker.DisplayHeight + "\n" +
                MouseHooker.ABSOLUTE_SIZE;
        }

        Thread mousePosThread = null;
        private void Button_MousePositionThread_Click(object sender, RoutedEventArgs e)
        {
            if ( mousePosThread is null )
            {
                mousePosThread = new Thread(() =>
                {
                    try
                    {
                        while ( true )
                        {
                            var pos = Form.Cursor.Position;
                            TextBlock_MousePosition.Dispatcher.Invoke(
                                () => TextBlock_MousePosition.Text = "X: " + pos.X + " Y: " + pos.Y);

                            Thread.Sleep(50);
                        }
                    }
                    catch { }
                });
                mousePosThread.Start();
            }
            else
            {
                mousePosThread.Abort();
                mousePosThread = null;
            }
        }

        private void Button_AbsoluteApplyClick(object sender, RoutedEventArgs e)
        {
            if ( TextBox_AbsoluteSize.Text.Length == 0 )
            {
                MessageBox.Show("앱솔 사이즈 입력");
                return;
            }

            MouseHooker.ABSOLUTE_SIZE = uint.Parse(TextBox_AbsoluteSize.Text);
            UpdateTT();
        }

        private void Button_HookreMove_Click(object sender, RoutedEventArgs e)
        {
            if ( TextBox_HookerX.Text.Length == 0 ||
                TextBox_HookerY.Text.Length == 0 )
                return;

            MouseHooker.MoveTo(new Point(
                int.Parse(TextBox_HookerX.Text),
                int.Parse(TextBox_HookerY.Text)
                ), true);
        }




        #endregion


    }
}
