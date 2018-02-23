using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncManger
{
    /// <summary>
    /// 任务装饰类 - 获取任务的线程
    /// </summary>
    internal class TaskDecorator
    {
        /// <summary>
        /// 使用的任务的线程
        /// </summary>
        private Thread m_thisUseThead = null;

        /// <summary>
        /// 被装饰的任务类
        /// </summary>
        private ITask m_thisTask = null;

        /// <summary>
        /// 任务是否被终止
        /// </summary>
        private bool m_bIfHasAborted = false;

        /// <summary>
        /// 任务是否被终止
        /// </summary>
        internal bool IfHasAborted
        {
            get
            {
                return m_bIfHasAborted;
            }
        }

        /// <summary>
        /// 任务核心接口
        /// </summary>
        internal ITask ThisTask
        {
            get
            {
                return m_thisTask;
            }
            private set
            {
                this.m_thisTask = value;
            }
        }

        /// <summary>
        /// 获取任务返回值
        /// </summary>
        internal object ReturnValue
        {
            get
            {
                return m_thisTask.ReturnValue;
            }
        }

        internal TaskDecorator(ITask coreTask)
        {
            this.ThisTask = coreTask;
        }

        /// <summary>
        /// 公开方法，获取异步线程，派发任务给核心接口
        /// </summary>
        internal void DoTask()
        {
            //获取当前使用的线程
            m_thisUseThead = Thread.CurrentThread;

            //派发任务给接口
            ThisTask.DoTask();
        }

        /// <summary>
        /// 终止当前执行的任务
        /// </summary>
        internal bool Abort()
        {
            //尝试终止线程并清理任务资源
            try
            {
                //终止线程
                m_thisUseThead.Abort();
                //等待线程完全停止
                while (!(m_thisUseThead.ThreadState == ThreadState.Aborted || m_thisUseThead.ThreadState == ThreadState.Stopped))
                {
                    ;
                }
                bool returnValue;
                returnValue = ThisTask.TryAbort();
                return returnValue;
            }
            //任何问题返回False
            catch (Exception)
            {
                return false;
            }
        }
    }
}
