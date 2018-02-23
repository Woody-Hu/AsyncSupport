using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncManger
{
    /// <summary>
    /// 进度条界面，提供进度条功能
    /// </summary>
    public partial class BaseProcessBarForm : Form
    {
        /// <summary>
        /// 使用的异步管理器
        /// </summary>
        private AsyncControlManger m_useAsyncManger = null;

        /// <summary>
        /// 关闭标示
        /// </summary>
        private bool m_CloseTag = false;

        /// <summary>
        /// 关闭标示
        /// </summary>
        internal bool CloseTag
        {
            get
            {
                return m_CloseTag;
            }
        }

        /// <summary>
        /// 使用的并发任务管理器
        /// </summary>
        internal AsyncControlManger UseAsyncManger
        {
            private get
            {
                return m_useAsyncManger;
            }

            set
            {
                m_useAsyncManger = value;
            }
        }

        public BaseProcessBarForm()
        {
            InitializeComponent();
        }

        private void BaseProcessBarForm_Load(object sender, EventArgs e)
        {
            //设置界面为持续刷新形式
            this.progressBar_MainBar.Style = ProgressBarStyle.Marquee;
            //设置界面刷新速度
            this.progressBar_MainBar.MarqueeAnimationSpeed = 40;
        }

        /// <summary>
        /// 取消按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            //关闭界面
            this.Close();
        }

        /// <summary>
        /// 用户关闭窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseProcessBarForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //设置关闭状态
            m_CloseTag = true;
            //设置强制关闭
            this.UseAsyncManger.IfNoneWait = true;
        }

        /// <summary>
        /// Dispose方法
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
