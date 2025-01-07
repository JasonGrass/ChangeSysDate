using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace ChangeSysTimeFrmApp
{
    public partial class MainForm : Form
    {
        private ChangeDate changeDateHandler;

        public MainForm()
        {
            InitializeComponent();
            changeDateHandler = new ChangeDate();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 界面显示初始化
            nudYear.Value = DateTime.Now.Year;
            nudMonth.Value = DateTime.Now.Month;
            nudDay.Value = DateTime.Now.Day;

            EventHandler eh = (o, args) =>
            {
                int timelength =
                    (int)nudHour.Value * 3600 + (int)nudMinute.Value * 60 + (int)nudSecond.Value;
                lbLeftTime.Text = timelength.ToString();
            };

            nudSecond.ValueChanged += eh;
            nudMinute.ValueChanged += eh;
            nudHour.ValueChanged += eh;

            nudSecond.Value = 20;

            // 托盘菜单
            showMianFormMenuItem.Click += ShowMianFormMenuItemOnClick;
            exitMenuItem.Click += ExitMenuItemOnClick;

            // 倒计时
            changeDateHandler.CountDown += time =>
            {
                this.Invoke(
                    new Action(() =>
                    {
                        this.lbLeftTime.Text = time.ToString();
                    })
                );
            };

            btnAdd60Second.Enabled = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changeDateHandler.IsRunning)
            {
                e.Cancel = true;
                HideMainForm();
            }
            else
            {
                this.notifyIcon.Visible = false;
                this.Dispose();
                Application.Exit();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int timelength =
                (int)nudHour.Value * 3600 + (int)nudMinute.Value * 60 + (int)nudSecond.Value;
            changeDateHandler.ContinueTime = timelength;
            changeDateHandler.TargetDate = new DateTime(
                (int)nudYear.Value,
                (int)nudMonth.Value,
                (int)nudDay.Value
            );
            changeDateHandler.Run();
            btnOk.Enabled = false;
            btnAdd60Second.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            changeDateHandler.Stop();
            btnOk.Enabled = true;
            btnAdd60Second.Enabled = false;
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            HideMainForm();
        }

        private void ExitMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            this.Close();
        }

        private void ShowMianFormMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            ShowMainForm();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;

                HideMainForm();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                ShowMainForm();
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            ShowMainForm();
        }

        #region 私有方法　处理窗体的　显示　隐藏　关闭(退出)

        private void HideMainForm()
        {
            this.Hide();
        }

        private void ShowMainForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
        #endregion

        private void btnAdd60Second_Click(object sender, EventArgs e)
        {
            changeDateHandler.AddContinueTime(60);
        }
    }
}
