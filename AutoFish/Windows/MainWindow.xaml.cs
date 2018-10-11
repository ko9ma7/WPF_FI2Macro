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
            if ( !File.Exists(path) )
                return;
          
            JObject obj = JObject.Parse(File.ReadAllText(path));
            AppSetting.Accuracy = int.Parse(obj["accuracy"].ToString());
            AppSetting.LeftX = int.Parse(obj["leftx"].ToString());
            AppSetting.LeftY = int.Parse(obj["lefty"].ToString());
            AppSetting.RightX = int.Parse(obj["rightx"].ToString());
            AppSetting.RightY = int.Parse(obj["righty"].ToString());
            AppSetting.LeftFishPoint = new Point(
                int.Parse(obj["left_fishpointx"].ToString()),
                int.Parse(obj["left_fishpointy"].ToString()));
            AppSetting.RightFishPoint = new Point(
                int.Parse(obj["right_fishpointx"].ToString()),
                int.Parse(obj["right_fishpointy"].ToString()));
            AppSetting.LeftBoxSpritePoint = new Point(
                int.Parse(obj["left_boxspritex"].ToString()),
                int.Parse(obj["left_boxspritey"].ToString()));
            AppSetting.RightBoxSpritePoint = new Point(
                int.Parse(obj["right_boxspritex"].ToString()),
                int.Parse(obj["right_boxspritey"].ToString()));
            AppSetting.MaxDelay = int.Parse(obj["delay_max"].ToString());
            AppSetting.MinDelay = int.Parse(obj["delay_min"].ToString());

            JArray array = obj["rectArray"] as JArray;
            for (int i = 0; i < array.Count; i++)
            {
                var obj2 = array[i] as JObject;
                var rectstring = obj2["value"].ToString();
                var rectlist = Strings.Split(rectstring, ",");
                AppSetting.RectList.Add(new Rect(
                    int.Parse(rectlist[0]),
                    int.Parse(rectlist[1]),
                    int.Parse(rectlist[2]),
                    int.Parse(rectlist[3])));

            }

            UpdateSetting2();
            UpdateRectListView();
            //String[] sp1 = Strings.Split(File.ReadAllText(rectPath), "\n");
            //for ( int i = 0 ; i < sp1.Length ; i++ )
            //{
            //    var splited = Strings.Split(sp1[i], ",");
            //    AppSetting.RectList.Add(new Rect(
            //        int.Parse(splited[0]),
            //        int.Parse(splited[1]),
            //        int.Parse(splited[2]),
            //        int.Parse(splited[3])
            //        ));
            //}

            //String[] sp2 = Strings.Split(File.ReadAllText(macroInputPath), "\n");
            //for ( int i = 0 ; i < sp2.Length ; i++ )
            //{
            //    if ( sp2[i].Contains("m") )
            //    {
            //        var str = Strings.Replace(sp2[i], "m", "");
            //        String[] splited = Strings.Split(str, ",");
            //        AppSetting.InputList.Add(new MacroInput(new Point(
            //            int.Parse(splited[0]),
            //            int.Parse(splited[1])
            //            )));
            //    }
            //    else if ( sp2[i].Contains("d") )
            //    {
            //        var str = Strings.Replace(sp2[i], "d", "");
            //        AppSetting.InputList.Add(new MacroInput(
            //            int.Parse(str)
            //            ));
            //    }
            //}
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
            obj.Add("left_fishpointx", AppSetting.LeftFishPoint.X);
            obj.Add("left_fishpointy", AppSetting.LeftFishPoint.Y);
            obj.Add("right_fishpointx", AppSetting.RightFishPoint.X);
            obj.Add("right_fishpointy", AppSetting.RightFishPoint.Y);
            obj.Add("left_boxspritex", AppSetting.LeftBoxSpritePoint.X);
            obj.Add("left_boxspritey", AppSetting.LeftBoxSpritePoint.Y);
            obj.Add("right_boxspritex", AppSetting.RightBoxSpritePoint.X);
            obj.Add("right_boxspritey", AppSetting.RightBoxSpritePoint.Y);
            obj.Add("delay_max", AppSetting.MaxDelay);
            obj.Add("delay_min", AppSetting.MinDelay);
            JObject obj2 = new JObject();
            JArray array = new JArray();
            for (int i = 0; i < AppSetting.RectList.Count; i++)
            {
                JObject rectobj = new JObject();
                rectobj.Add("value", AppSetting.RectList[i].ToString());
                array.Add(rectobj);
            }
            obj.Add("rectArray", array);


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

        #region Setting2
       
        private void Button_Setting2_Apply_Click(object sender, RoutedEventArgs e)
        {
            #region exception
            if (TextBox_FishingLeftX.Text.Length == 0 ||
               TextBox_FishingLeftY.Text.Length == 0 ||
               TextBox_FishingRightX.Text.Length == 0 ||
               TextBox_FishingRightY.Text.Length == 0)
            {
                MessageBox.Show("낚시 좌표 설정 값을 제대로 입력해주세요");
                return;
            }
            else if (TextBox_BoxSpriteLeftX.Text.Length == 0 ||
                     TextBox_BoxSpriteLeftY.Text.Length == 0 ||
                     TextBox_BoxSpriteRightX.Text.Length == 0 ||
                     TextBox_BoxSpriteRightY.Text.Length == 0)
            {
                MessageBox.Show("박스,정령 좌표 설정 값을 제대로 입력해주세요");
                return;
            }
            else if (TextBox_ClickDelayMinimun.Text.Length == 0 ||
                    TextBox_ClickDelayMaximum.Text.Length == 0)
            {
                MessageBox.Show("딜레이를 제대로 입력해주세요");
                return;
            }
            #endregion

            int.TryParse(TextBox_FishingLeftX.Text, out int FishingLeftX);
            int.TryParse(TextBox_FishingLeftY.Text, out int FishingLeftY);
            int.TryParse(TextBox_FishingRightX.Text, out int FishingRightX);
            int.TryParse(TextBox_FishingRightY.Text, out int FishingRightY);

            int.TryParse(TextBox_BoxSpriteLeftX.Text, out int BoxSpriteLeftX);
            int.TryParse(TextBox_BoxSpriteLeftY.Text, out int BoxSpriteLeftY);
            int.TryParse(TextBox_BoxSpriteRightX.Text, out int BoxSpriteRightX);
            int.TryParse(TextBox_BoxSpriteRightY.Text, out int BoxSpriteRightY);

            int.TryParse(TextBox_ClickDelayMinimun.Text, out int min);
            int.TryParse(TextBox_ClickDelayMaximum.Text, out int max);

            if (FishingLeftX > FishingRightX)
            {
                MessageBox.Show("낚시 좌X는 우X보다 클수 없습니다");
                return;
            }
            else if (FishingLeftY > FishingRightY)
            {
                MessageBox.Show("낚시 좌Y는 우Y보다 클수 없습니다");
                return;
            }
            else if (BoxSpriteLeftX > BoxSpriteRightX)
            {
                MessageBox.Show("박스정령 좌X는 우X보다 클수 없습니다");
                return;
            }
            else if (BoxSpriteLeftY > BoxSpriteRightY)
            {
                MessageBox.Show("박스정령 좌Y는 우Y보다 클수 없습니다");
                return;
            }
            else if (max < min )
            {
                MessageBox.Show("딜레이 최소값이 최대값보다 클 수 없습니다");
                return;
            }

            AppSetting.LeftFishPoint = new Point(FishingLeftX, FishingLeftY);
            AppSetting.RightFishPoint = new Point(FishingRightX, FishingRightY);
            AppSetting.MinDelay = min;
            AppSetting.MaxDelay = max;
            AppSetting.LeftBoxSpritePoint = new Point(BoxSpriteLeftX, BoxSpriteLeftY);
            AppSetting.RightBoxSpritePoint = new Point(BoxSpriteRightX, BoxSpriteRightY);
        }

        private void Button_Setting2_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            WriteLog("낚시, 정령, 박스 좌표값 저장완료");
        }

        private void UpdateSetting2()
        {
            TextBox_FishingLeftX.Text = AppSetting.LeftFishPoint.X.ToString();
            TextBox_FishingLeftY.Text = AppSetting.LeftFishPoint.Y.ToString();
            TextBox_FishingRightX.Text = AppSetting.RightFishPoint.X.ToString();
            TextBox_FishingRightY.Text = AppSetting.RightFishPoint.Y.ToString();

            TextBox_BoxSpriteLeftX.Text = AppSetting.LeftBoxSpritePoint.X.ToString();
            TextBox_BoxSpriteLeftY.Text = AppSetting.LeftBoxSpritePoint.Y.ToString();
            TextBox_BoxSpriteRightX.Text = AppSetting.RightBoxSpritePoint.X.ToString();
            TextBox_BoxSpriteRightY.Text = AppSetting.RightBoxSpritePoint.Y.ToString();

            TextBox_ClickDelayMinimun.Text = AppSetting.MinDelay.ToString();
            TextBox_ClickDelayMaximum.Text = AppSetting.MaxDelay.ToString();
        }
        #endregion

        #region Setting3
        private void Button_Setting3_AddRect_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_Setting3_LeftX.Text.Length == 0 ||
                TextBox_Setting3_LeftY.Text.Length == 0 ||
                TextBox_Setting3_RightX.Text.Length == 0||
                TextBox_Setting3_RightY.Text.Length == 0)
            {
                MessageBox.Show("입력은 꼭해주세요");
                return;
            }

            int.TryParse(TextBox_Setting3_LeftX.Text, out int LeftX);
            int.TryParse(TextBox_Setting3_LeftY.Text, out int LeftY);
            int.TryParse(TextBox_Setting3_RightX.Text, out int RightX);
            int.TryParse(TextBox_Setting3_RightY.Text, out int RightY);

            if (LeftY > RightY)
            {
                MessageBox.Show("좌Y는 우Y보다 클수 없습니다");
                return;
            }
            else if (LeftX > RightX)
            {
                MessageBox.Show("좌X는 우X보다 클수 없습니다");
                return;
            }

            AppSetting.RectList.Add(new Rect(LeftX, LeftY, RightX, RightY));
            UpdateRectListView();
            SaveSettings();
            WriteLog("저장 후 공간좌표 저장완료");
        }

        private void UpdateRectListView()
        {
            this.Dispatcher.Invoke(() =>
            {
                ListView_RectList.Items.Clear();
                for (int i=0; i < AppSetting.RectList.Count; i++)
                {
                    var rect = AppSetting.RectList[i];
                    ListView_RectList.Items.Add(
                        rect.LeftX + "," + rect.LeftY + "," +
                        rect.RightX + "," + rect.RightY);
                }
            });
        }

        private void ListView_RectList_KeyDown(object sender, KeyEventArgs e)
        {
            if (ListView_RectList.SelectedItem is null)
                return;

            if (e.Key == Key.Delete)
            {
                AppSetting.RectList.RemoveAt(ListView_RectList.SelectedIndex);
                SaveSettings();
                UpdateRectListView();
                WriteLog("삭제 후 공간좌표 저장완료");
            }
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
