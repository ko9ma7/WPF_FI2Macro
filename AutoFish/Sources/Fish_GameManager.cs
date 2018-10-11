namespace AutoFish
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Threading;

    class GameManager
    {
        public static string CurrentDirectory = Environment.CurrentDirectory;
        public static string ImagesDirectory = Environment.CurrentDirectory + "\\ImageSources";

        public static string GetPicturePath(GamePicture picture)
        {
            if ( Directory.Exists(ImagesDirectory) == false )
                return string.Empty;

            if ( picture == GamePicture.Fail )
                return ImagesDirectory + "\\Fail.PNG";
            else if ( picture == GamePicture.Success )
                return ImagesDirectory + "\\Success.PNG";
            else if ( picture == GamePicture.SuccessFairy )
                return ImagesDirectory + "\\SuccessFairy.PNG";
            else if ( picture == GamePicture.FullFish )
                return ImagesDirectory + "\\FullFish.PNG";
            else if ( picture == GamePicture.FullFish_SellAll )
                return ImagesDirectory + "\\FullFish_SellAll.PNG";
            else if ( picture == GamePicture.FullFish_SellAllOk )
                return ImagesDirectory + "\\FullFish_SellAllOk.PNG";
            else if ( picture == GamePicture.FullFish_SellCheck )
                return ImagesDirectory + "\\FullFish_SellCheck.PNG";
            else if ( picture == GamePicture.Back )
                return ImagesDirectory + "\\Back.PNG";
            else if ( picture == GamePicture.BigMonster )
                return ImagesDirectory + "\\BigMonster.PNG";
            else 
                return ImagesDirectory + "\\Numbers\\" + (int)picture + ".PNG";
        }

        public static Point FindGamePicture(Rect rect, GamePicture picture)
        {
            var path = GetPicturePath(picture);
            if ( !File.Exists(path) )
                return new Point(-2, -2);
            string[] search = ImageSearcher.Search(rect.LeftX, rect.LeftY, rect.RightX, rect.RightY, path);
            if ( search == null )
                return new Point(-1, -1);
            int[] search_ = new int[search.Length];
            for ( int j = 0 ; j < search.Length ; j++ )
                search_[j] = Convert.ToInt32(search[j]);
            return new Point(search_[1], search_[2]);
        }

        public static Point FindGamePicture( GamePicture picture)
        {
            var path = GetPicturePath(picture);
            if ( !File.Exists(path))
                    return new Point(-2, -2);
            string[] search = ImageSearcher.Search(path);
            if ( search == null )
                return new Point(-1, -1);
            int[] search_ = new int[search.Length];
            for ( int j = 0 ; j < search.Length ; j++ )
                search_[j] = Convert.ToInt32(search[j]);
            return new Point(search_[1], search_[2]);
        }

        public static Point FindGamePictureDuring(Rect rect, GamePicture picture, int ms, int maxcount)
        {
            var path = GetPicturePath(picture);
            if ( !File.Exists(path) )
                return new Point(-2, -2);

            int count = 0;
            while ( count < maxcount )
            {
                string[] search = ImageSearcher.Search(path);
                if ( search == null )
                {
                    count++;
                    Thread.Sleep(ms);
                    continue;
                }
                int[] search_ = new int[search.Length];
                for ( int j = 0 ; j < search.Length ; j++ )
                    search_[j] = Convert.ToInt32(search[j]);
                return new Point(search_[1], search_[2]);
            }

            return new Point(-1, -1);
        }


        public static bool IsFind(Point p)
        {
            if ( p.X == -1 && p.Y == -1 )
                return false;
            else
                return true;
        }

        public static bool IsExist(Point p)
        {
            if ( p.X == -2 && p.Y == -2 )
                return false;
            else
                return true;
        }

        public class Macro
        {
            private static Thread FinderThread = null;
            private static Thread MacroThread = null;
            public static int StackSuccessCount = 0;
            public static int StackFailCount = 0;
            public static int StackMacroCount = 0;
            public static int StackBigMonsterCount = 0;
            public static int StackFullFishCount = 0;

            #region ClickThread
            private static void ClickThread(ref int count, GamePicture picture, Rect rect)
            {
                try
                {
                    List<bool> check = new List<bool>() { false, false};
                    while ( true )
                    {
                        var point = FindGamePicture(rect, picture);
                        if ( !IsFind(point) )
                        {
                            if ( check[0] == true && check[1] == false )
                            {
                                count++;
                                if ( picture == GamePicture.Fail )
                                    MainWindow.mainWindow.ModifyGetFail("실패 : " + count.ToString() + "(" + (++StackFailCount) + ")");
                                else if ( picture == GamePicture.Success )
                                    MainWindow.mainWindow.ModifyGetSuccess("성공 : " + count.ToString() + "(" + (++StackSuccessCount) + ")");
                                else if ( picture == GamePicture.BigMonster )
                                    MainWindow.mainWindow.ModifyBigMosnter("괴수 등장 : " + count.ToString() + "(" + (++StackBigMonsterCount) + ")");
                            }

                            Thread.Sleep(1000);
                            check.Add(false);
                            check.RemoveAt(0);
                            continue;
                        }
                        check.Add(true);
                        check.RemoveAt(0);
                        MouseHooker.LeftClick(point);
                        Thread.Sleep(1000);
                    }
                }
                catch { }
            }
            #endregion

            #region ClickThreadOnce
            private static void ClickThreadOnce(ref int count, GamePicture picture, Rect rect)
            {
                try
                {
                    List<bool> check = new List<bool>() { false, false };
                    while ( count == 0 )
                    {
                        var point = FindGamePicture(rect, picture);
                        if ( !IsExist(point) )
                        {
                            MainWindow.mainWindow.WriteLog(picture + "경로에 없음");
                            throw new Exception("");
                        }

                        if ( !IsFind(point) )
                        {
                            if ( check[0] == true && check[1] == false )
                            {
                                count++;
                                MainWindow.mainWindow.WriteLog(picture + "확인");
                            }

                            MainWindow.mainWindow.WriteLog(picture + "서치 실패");
                            Thread.Sleep(1000);
                            check.Add(false);
                            check.RemoveAt(0);
                            continue;
                        }
                        check.Add(true);
                        check.RemoveAt(0);
                        MouseHooker.LeftClick(point);
                        
                        Thread.Sleep(1000);
                    }
                }
                catch { }
            }
            #endregion

            #region ClickEvent
            private static void ClickEvent(GamePicture picture, Rect rect, int ms, Action action)
            {
                List<bool> check = new List<bool>() { false, false };
                while ( true )
                {
                    var point = FindGamePicture(rect, picture);
                    if ( !IsFind(point) )
                    {
                        if ( check[0] == true && check[1] == false )
                        {
                            MainWindow.mainWindow.WriteLog(picture + "확인");
                            action();
                            return;
                        }
                        MainWindow.mainWindow.WriteLog(picture + "서치 실패");
                        Thread.Sleep(ms);
                        check.Add(false);
                        check.RemoveAt(0);
                        continue;
                    }
                    check.Add(true);
                    check.RemoveAt(0);
                    MouseHooker.LeftClick(point);
                    Thread.Sleep(ms);
                }
            }
            #endregion

            #region MacroInputCheckingThread
            private static void MacroInputCheckingThread(ref int count, Rect rect)
            {
                try
                { 
                    while (true)
                    {
                        for ( int i = 0 ; i <= 10 ; i++ )
                        {
                            var point = FindGamePicture(rect, ( GamePicture )i);
                            if ( IsFind(point) )
                                MouseHooker.LeftClick(point);
                            Thread.Sleep(30);
                        }
                    }
                }
                catch { }
            }
            #endregion

            #region FullFishCheckingThread
            public static void FullFishCheckingThread(ref int fullFishCount, GamePicture fullFish, Rect rect)
            {
                List<bool> check = new List<bool>() { false, false };
                List<Thread> FinderThreadList = new List<Thread>();
                try
                {
                    
                    while ( true )
                    {
                        AA:;
                        var point = FindGamePicture(rect, fullFish);
                        if ( !IsFind(point) )
                        {
                            if ( check[0] == true && check[1] == false )
                            {
                                if ( MacroThread != null )
                                    MacroThread.Abort();
                                MainWindow.mainWindow.WriteLog("어망 진입완료");
                                MainWindow.mainWindow.ModifySellFullFish("어망 풀 : " + (++fullFishCount).ToString() + "(" + (++StackFullFishCount) + ")");
                                int a = 0;
                                int b = 0;
                                int NoFish = 0;

                                try
                                {
                                    FinderThreadList.Add(new Thread(() => ClickThreadOnce(
                                        ref a, GamePicture.FullFish_SellAll, AppSetting.RectList[( int )RectEnum.FullFishContinue]
                                        )));
                                    FinderThreadList.Add(new Thread(() => ClickThreadOnce(
                                        ref a, GamePicture.FullFish_SellAllOk, AppSetting.RectList[( int )RectEnum.FullFishContinue]
                                        )));
                                    StartList(FinderThreadList);
                                }
                                catch(Exception e) { MainWindow.mainWindow.WriteLog("문제1 : " +e ); }

                                try
                                {
                                    while ( true )
                                    {
                                        var Clear어망 = FindGamePicture(AppSetting.RectList[( int )RectEnum.FullFishContinue], GamePicture.FullFish_SellCheck);
                                        if ( IsFind(Clear어망) )
                                        {
                                            NoFish++;
                                            MainWindow.mainWindow.WriteLog(GamePicture.FullFish_SellCheck + "확인");
                                            break;
                                        }
                                        Thread.Sleep(500);
                                    }
                                }
                                catch ( Exception e ) { MainWindow.mainWindow.WriteLog("문제2 : " + e); }


                                try
                                {
                                    if ( NoFish > 0 )
                                    {
                                        AbortList(FinderThreadList);
                                        FinderThreadList.Clear();
                                        ClickEvent(GamePicture.Back, AppSetting.RectList[( int )RectEnum.FullFishContinue], 1000, new Action(() =>
                                        {
                                            MouseHooker.MoveTo(new Point(0, 0));
                                            MainWindow.mainWindow.WriteLog("낚시터로 이동완료");


                                            MacroThread = new Thread(MacroThreadFunction);
                                            MacroThread.Start();
                                        }));
                                        check[0] = false;
                                        check[1] = false;
                                        goto AA;
                                    }
                                    else
                                    {
                                        MainWindow.mainWindow.WriteLog("어망 비우기 실패");
                                        goto AA;
                                    }
                                }
                                catch ( Exception e ) { MainWindow.mainWindow.WriteLog("문제3 : " + e); }
                            }
                            Thread.Sleep(1000);
                            check.Add(false);
                            check.RemoveAt(0);
                            continue;
                        }
                        check.Add(true);
                        check.RemoveAt(0);
                        MouseHooker.LeftClick(point);
                        Thread.Sleep(1000);

                        
                    }
                }
                catch { AbortList(FinderThreadList); }
            }
            #endregion

            private static void AbortList(List<Thread> li) { foreach ( var thread in li ) thread.Abort(); }
            private static void StartList(List<Thread> li) { foreach ( var thread in li ) thread.Start(); }

            public static void FinderThreadFunction()
            {
                List<Thread> FinderThreadList = new List<Thread>();
                try
                {
                    if ( AppSetting.RectList.Count == 0 )
                    {
                        MainWindow.mainWindow.WriteLog("설정된 렉트리스트가 없습니다");
                        throw new Exception("설정된 렉트리스트가 없음");
                    }

                    int SuccessCount = 0;
                    int FailCount = 0;
                    int BigMonsterCount = 0;
                    int FullFishCount = 0;
                    int MacroInputCount = 0;

                    MainWindow.mainWindow.ModifyGetSuccess("성공 : " + 0 + "(" + (StackSuccessCount) + ")");
                    MainWindow.mainWindow.ModifyGetFail("실패 : " + 0 + "(" + (StackFailCount) + ")");
                    MainWindow.mainWindow.ModifySellFullFish("어망 풀 : 0" + "(" + (StackFullFishCount) + ")");
                    MainWindow.mainWindow.ModifyBigMosnter("괴수 등장 : " + 0 + "(" + (StackBigMonsterCount) + ")");



                    FinderThreadList.Add(new Thread(() => ClickThread(
                        ref SuccessCount, GamePicture.Success, AppSetting.RectList[( int )RectEnum.SuccessFail]
                        )));
                    FinderThreadList.Add(new Thread(() => ClickThread(
                        ref FailCount, GamePicture.Fail, AppSetting.RectList[( int )RectEnum.SuccessFail]
                        )));
                    FinderThreadList.Add(new Thread(() => ClickThread(
                        ref BigMonsterCount, GamePicture.BigMonster, AppSetting.RectList[( int )RectEnum.BigMonster]
                        )));
                    FinderThreadList.Add(new Thread(() => FullFishCheckingThread(
                        ref FullFishCount, GamePicture.FullFish, AppSetting.RectList[( int )RectEnum.FullFishStart]
                        )));
                    FinderThreadList.Add(new Thread(() => MacroInputCheckingThread(
                        ref MacroInputCount, AppSetting.RectList[( int )RectEnum.MacroInputChecking]
                        )));

                    StartList(FinderThreadList);
                    Thread.Sleep(3600 * 1000 * 50);
                }
                catch(Exception e)
                {
                    AbortList(FinderThreadList);
                    FinderThread = null;
                }
            }

            

            public static void MacroThreadFunction()
            {
                try
                {
                    if ( AppSetting.InputList.Count == 0 )
                    {
                        MainWindow.mainWindow.WriteLog("설정된 매크로가 없습니다");
                        throw new Exception("설정된 매크로가 없음");
                    }

                    while (true)
                    {
                        for (int i=0 ; i< AppSetting.InputList.Count ; i++ )
                            AppSetting.InputList[i].Excecute();
                    }
                }
                catch ( Exception e ) { MacroThread = null; }
            }

            public static void Start()
            {
                if ( FinderThread == null && MacroThread == null )
                {
                    MainWindow.mainWindow.Dispatcher.Invoke(() =>
                    {
                        MainWindow.mainWindow.Button_Start.Content = "중지";
                    });
                    MainWindow.mainWindow.WriteLog("매크로 시작");

                    FinderThread = new Thread(FinderThreadFunction);
                    FinderThread.Start();

                    MacroThread = new Thread(MacroThreadFunction);
                    MacroThread.Start();
                }
            }

            public static void Stop()
            {
                if ( FinderThread != null || MacroThread != null )
                {
                    MainWindow.mainWindow.Dispatcher.Invoke(() =>
                    {
                        MainWindow.mainWindow.Button_Start.Content = "시작";
                    });
                    MainWindow.mainWindow.WriteLog("매크로 중지");

                   

                    FinderThread.Abort();
                    if (MacroThread != null)
                        MacroThread.Abort();
                }
            }

            public static bool IsWorking()
            {
                if ( FinderThread == null && MacroThread == null )
                    return false;
                else
                    return true;
            }
        }
    }
}
