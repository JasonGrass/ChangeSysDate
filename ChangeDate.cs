﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace ChangeSysTimeFrmApp
{
    class ChangeDate
    {
        /*
         * 修改系统时间到指定的 年份
         */

        //-----------------------------
        //Static field

        /// <summary>
        /// 计时器触发间隔时间，毫秒
        /// </summary>
        public static int TriggerTimeSpan = 200;

        //-----------------------------
        //Public field

        /// <summary>
        /// 持续时间，秒
        /// </summary>
        public int ContinueTime
        {
            get { return timerCount / (1000 / TriggerTimeSpan); }
            set { timerCount = value * (1000 / TriggerTimeSpan); }
        }

        /// <summary>
        /// 目标年份
        /// </summary>
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// 系统是否正在运行
        /// </summary>
        public bool IsRunning
        {
            get { return timer.Enabled; }
        }

        //-----------------------------
        //private field

        /// <summary>
        /// 定时器触发计数
        /// </summary>
        private int timerCount;

        /// <summary>
        /// 时间日期
        /// </summary>
        private DateTime realDate;

        private System.Timers.Timer timer;

        private string targetDateTime;

        private string realDateTime;

        //-----------------------------
        //Event

        /// <summary>
        /// 时间更改结束
        /// </summary>
        public event Action ChangeFinished;

        /// <summary>
        /// 倒计时
        /// </summary>
        public event Action<int> CountDown;

        //-----------------------------
        //Public method

        public ChangeDate()
        {
            ContinueTime = 200; // 默认200秒
            TargetDate = DateTime.Now; // 默认

            realDate = DateTime.Now;
            timer = new Timer(TriggerTimeSpan);
            timer.Elapsed += TimerOnElapsed;
        }

        /// <summary>
        /// 增加持续时间
        /// </summary>
        /// <param name="second">时间，秒</param>
        public void AddContinueTime(int second)
        {
            int count = second * (1000 / TriggerTimeSpan);
            timerCount += count;
        }

        /// <summary>
        /// 执行
        /// </summary>
        public void Run()
        {
            timer.Start();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            realDateTime = realDate.ToString("yyyy-MM-dd") + DateTime.Now.ToString(" HH:mm:ss");
            SysTimePro.SetLocalTimeByStr(realDateTime);
        }

        //-----------------------------
        //Private method

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            targetDateTime = TargetDate.ToString("yyyy-MM-dd ") + DateTime.Now.ToString("HH:mm:ss");
            SysTimePro.SetLocalTimeByStr(targetDateTime);

            if (--timerCount == 0)
            {
                timer.Stop();
                realDateTime = realDate.ToString("yyyy-MM-dd") + DateTime.Now.ToString(" HH:mm:ss");
                SysTimePro.SetLocalTimeByStr(realDateTime);
                if (ChangeFinished != null)
                {
                    ChangeFinished();
                }
            }
            else
            {
                if (timerCount % (1000 / TriggerTimeSpan) == 0)
                {
                    if (CountDown != null)
                    {
                        CountDown(ContinueTime);
                    }
                }
            }
        }
    }
}
