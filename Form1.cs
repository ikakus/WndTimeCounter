using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace WndTimeCounter
{
    public partial class Form1 : Form
    {


        delegate void UpdateLabelDelegate(String str, Label la);
        delegate void UpdateListviewDelegate(String window, String time,ListView list);
        delegate void UpdateFileredListviewDelegate(String window, String time, ListView list,string filter);
        delegate void UpdateFileredCombinedListviewDelegate(ListView list, string filter);
        delegate string GetTextBoxTextDelegate(TextBox txBox);
        delegate string GetLisviewSubitem0Delagate(ListView list, int i);


        public string GetLisviewSubitem0(ListView list,int i)
        {

            if (list.InvokeRequired)
            {

                return (string)list.Invoke(new GetLisviewSubitem0Delagate(GetLisviewSubitem0), new object[] { list, i });

            }
            else
            {
                return list.Items[i].SubItems[0].Text;
            }
        }

        public string GetTextBoxText(TextBox txBox)
        {

            //if (this.listView1.InvokeRequired)
            //    return (string)this.listView1.Invoke(new GetListText(GetText), new object[] { i, j });
            //else
            //{
            //    return listView1.Items[i].SubItems[j].Text;

            //}

            if (txBox.InvokeRequired)
            {
               
                    return (string)txBox.Invoke(new GetTextBoxTextDelegate(GetTextBoxText), new object[] { txBox });
               
            }
            else
            {
                return txBox.Text;
            }
        }


        public void UpdateFileredCombinedListview( ListView list, string filter)
        {

            if (list.InvokeRequired)
                try
                {
                    list.Invoke(new UpdateFileredCombinedListviewDelegate(UpdateFileredCombinedListview), new object[] {  list, filter });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("UpdateListview  " + ex.ToString());
                }
            else
            {
                if (filter.Length > 0)
                {
                    string[] str = new string[2];


                    label1.Text = WndTimeList.Count.ToString();
                    TimeSpan sTime = new TimeSpan();
                    for (int i = 0; i <= WndTimeList.Count - 1; i++)
                    {
                        if (WndTimeList[i].getWindowName().Contains(filter))
                        {
                            sTime += WndTimeList[i].getTimeElapsed();
                        }
                    }

                    if (sTime.ToString().Length > 8)
                    {
                        str[0] = sTime.ToString().Remove(8);
                    }
                    str[1] = filter;
                    ListViewItem itm = new ListViewItem(str);

                    listView1.Items.Clear();

                    this.listView1.Items.Add(itm);

                }
                
            }

 

        }

        public void UpdateFileredListview(String window, String time, ListView list,string filter)
        {

            if (list.InvokeRequired)
                try
                {
                    list.Invoke(new UpdateFileredListviewDelegate(UpdateFileredListview), new object[] { window, time, list, filter });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("UpdateListview  " + ex.ToString());
                }
            else
            {
                string[] str = new string[2];
                str[0] = time;
                str[1] = window;
                ListViewItem itm = new ListViewItem(str);
                removeNotExisting();
                label1.Text = WndTimeList.Count.ToString();
               // bool exists = false;


                for (int i = 0; i <= listView1.Items.Count - 1; i++)
                {

                    if (this.listView1.Items[i].SubItems[1].Text == window &&
                        this.listView1.Items[i].SubItems[1].Text.Contains(filter))// already exists in listview
                    {

                       // exists = true;
                        this.listView1.Items[i].SubItems[1].Text = window;
                        this.listView1.Items[i].SubItems[0].Text = time;


                        listView1.Sort();
                      //  TimeSpan timeSumm = new TimeSpan();
                    
                      
                     

                    }
                    else
                        if (this.listView1.Items[i].SubItems[0].Text.CompareTo("00:00:01") < 0 || !this.listView1.Items[i].SubItems[1].Text.Contains(filter))
                        {
                            this.listView1.Items[i].Remove();
                        }
                  


                }
                if (//exists == false &&
                    window != null && 
                    window.Contains(filter)&&
                    searchListbox(window)<0)
                {
                    this.listView1.Items.Add(itm);


                }


            }
 
        }
        public void UpdateListview(String window, String time,ListView list)
        {
            if (list.InvokeRequired)
                try
                {
                    list.Invoke(new UpdateListviewDelegate(UpdateListview), new object[] { window, time, list });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("UpdateListview  " + ex.ToString());
                }
            else
            {
                string[] str = new string[2];
                str[0] = time;
                str[1] = window;
                ListViewItem itm = new ListViewItem(str);
                removeNotExisting();
                label1.Text = WndTimeList.Count.ToString();
                bool exists = false;
                for (int i = 0; i <= listView1.Items.Count - 1; i++)
                {
                    
                    if (this.listView1.Items[i].SubItems[1].Text == window)
                    {

                        exists = true;
                        this.listView1.Items[i].SubItems[1].Text = window;
                        this.listView1.Items[i].SubItems[0].Text = time;

                      
                        listView1.Sort();


                    }
                    else
                        if (this.listView1.Items[i].SubItems[0].Text.CompareTo("00:00:01") < 0)
                        {
                            this.listView1.Items[i].Remove();
                        }




                }
                if (exists == false && window != null)
                {
                    this.listView1.Items.Add(itm);


                }
               

            }
        }


        public void UpdateLabel(String str, Label la)
        {

            if (la.InvokeRequired)
                try
                {
                    la.Invoke(new UpdateLabelDelegate(UpdateLabel), new object[] { str, la });
                }
                catch (Exception)
                { }
            else
                la.Text = str;//.AppendText(msg);

        }

      


        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }


        Thread LoopThread;
        Stopwatch watch = new Stopwatch();

        //WndTime[] WndTimeArray = new WndTime[200];
        List<WndTime> WndTimeList = new List<WndTime>();
        int count = 0;

        bool running = false;
                                    
        int searchListbox(string windowName)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[1].Text == windowName)
                {
                    return i;
                }
            }
            return -1;
        }

        int searchList(string windowName)
        {
            for (int i = 0; i < WndTimeList.Count; i++)
            {
                if (WndTimeList[i].getWindowName() == windowName)
                {
                    return i;
                }
            }
            return -1;
        }


        void removeNotExisting()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (searchList(listView1.Items[i].SubItems[1].Text)<0)
                {
                    listView1.Items[i].Remove();
                }
            }
        }

        //void removeNotExisting()
        //{
        //    for (int i = 0; i < WndTimeList.Count; i++)
        //    {
        //         if(searchListbox(WndTimeList[i].getWindowName())<0)
        //         {
        //             WndTimeList.RemoveAt(i);
        //         }
        //    }
        //}


        void stopAll()
        {
            for (int i = 0; i < WndTimeList.Count; i++)
            {
                WndTimeList[i].stopWatch();
            }
        }


        void WindowsStatisticTime()
        {
            while (true)
            {
                String caption = GetActiveWindowTitle();


                string filter = GetTextBoxText(textBox_filter);
                int indexof = searchList(caption);


                //for (int i = 0; WndTimeList.Count > i; i++)
                //{
                //    int ind = searchList(WndTimeList[i].getWindowName());
                //    UpdateFileredListview(WndTimeList[ind].getWindowName(),
                //        WndTimeList[ind].getTime(),
                //        listView1, filter);

                //}

                if (filter.Length > 0)
                {
                    UpdateFileredCombinedListview(listView1, filter);


                }
                else


                {

                    for (int i = 0; WndTimeList.Count > i; i++)
                    {
                        int ind = searchList(WndTimeList[i].getWindowName());
                        UpdateFileredListview(WndTimeList[ind].getWindowName(),
                            WndTimeList[ind].getTime(),
                            listView1, filter);

                    }
                }
                    TimeSpan summTime = new TimeSpan();

                    for (int i = 0; listView1.Items.Count > i; i++)
                    {
                        TimeSpan tmSpan = new TimeSpan();


                        TimeSpan.TryParse(GetLisviewSubitem0(listView1, i), out tmSpan);

                        summTime += tmSpan;
                        //   tmSpan=listView1.Items[i].SubItems[0];
                    }

                    if (summTime.ToString().Length >= 8)
                    {
                        UpdateLabel(summTime.ToString(), label_timeSummary);
                    }

                    // UpdateLabel(summTime.ToString().Remove(8), label_timeSummary);

                    if (indexof > -1)// if found update time
                    {
                        stopAll();
                        WndTimeList[indexof].startWatch();

                        // TimeSpan timeSumm = new TimeSpan();
                        TimeSpan time = WndTimeList[indexof].getTimeElapsed();
                        String TimeStr = time.ToString();
                        if (time.ToString().Length > 8)
                        {
                            WndTimeList[indexof].setTime(TimeStr.Remove(8));
                        }

                    //    UpdateListview(WndTimeList[indexof].getWindowName(),
                         //   WndTimeList[indexof].getTime(), listView1);

                        //UpdateFileredListview(WndTimeList[indexof].getWindowName(),
                        //    WndTimeList[indexof].getTime(),
                        //    listView1, filter);


                        //////////////////
                        //timeSumm += WndTimeList[indexof].getTimeElapsed();


                        //if (timeSumm.ToString().ToString().Length > 8)
                        //{
                        //    UpdateLabel(timeSumm.ToString().Remove(8), label_timeSummary);
                        //}


                    }
                    else //else add  new entry
                    {
                        if (caption != null)
                        {


                            //  UpdateFileredListview(caption, "00:00:00", listView1,filter);
                         //   UpdateListview(caption, "00:00:00", listView1);
                            WndTimeList.Add(new WndTime("00:00:00", caption));

                            //WndTimeList[count].setWindowName(caption);
                            count++;


                        }
                    }
                

              // UpdateFileredCombinedListview(listView1, filter);
               // UpdateListview(caption, "0",listView1);
                Thread.Sleep(900);
            }
        }

     

        public Form1()
        {
            InitializeComponent();
            //for (int i = 0; i < 200; i++)
            //{
            //    WndTimeArray[i] = new WndTime();
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LoopThread = new Thread(WindowsStatisticTime);
            LoopThread.IsBackground = true;
            LoopThread.Start();
            running = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            watch.Reset();

          //  UpdateLabel(watch.Elapsed.ToString(), this.label1);
        }


        class WndTime
        {
            private String windowName;
            private String Time;

            private Stopwatch watch = new Stopwatch();

            public void startWatch()
            {
                this.watch.Start();
            }

            public void stopWatch()
            {
                this.watch.Stop();
            }

            public void resetWatch()
            {
                this.watch.Reset();
            }


            public TimeSpan getTimeElapsed()
            {
              return  this.watch.Elapsed;
            }

            public WndTime()
            {
                setTime("00:00:00");
                setWindowName("noname");
            }

            public WndTime(string time, string caption)
            {
                setTime(time);
                setWindowName(caption);
            }
            public void setTime(string str)
            {
                this.Time = str;
            }

           public void setWindowName(string str)
            {
                this.windowName = str;
            }

           public String getTime()
            {
                return this.Time;
            }

           public String getWindowName()
            {
                return this.windowName;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (running == true)
            {
                try
                {
                    LoopThread.Suspend();
                    this.button1.Text = "Resume";
                    running = false;
                    stopAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (running== false)
            {
                try
                {
                    LoopThread.Resume();
                    this.button1.Text = "Pause";
                    running = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }
    }
}
