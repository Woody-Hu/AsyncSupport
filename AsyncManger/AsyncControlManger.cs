using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncManger
{
    /// <summary>
    /// 异步委托类 无输入 无返回
    /// </summary>
    internal delegate void TaskDelegate();

    /// <summary>
    /// 异步模块核心类
    /// </summary>
    public class AsyncControlManger
    {
        #region 字段
        /// <summary>
        /// 管理类所在的线程
        /// </summary>
        private Thread m_thisThread = null;

        /// <summary>
        /// 是否不等待并行任务标示
        /// </summary>
        private bool m_bIfNone = false;

        /// <summary>
        /// 所有的任务列表
        /// </summary>
        private List<ITask> m_lstAllTask = new List<ITask>();

        /// <summary>
        /// 使用的任务装饰列表
        /// </summary>
        private List<TaskDecorator> m_lstAllTaskDecorator = new List<TaskDecorator>();

        /// <summary>
        /// 使用的异步委托列表
        /// </summary>
        private List<TaskDelegate> m_lstAllDelegates = new List<TaskDelegate>();

        /// <summary>
        /// 所有的并行任务结果
        /// </summary>
        private List<IAsyncResult> m_lstAsyncResult = new List<IAsyncResult>();

        /// <summary>
        /// 并行任务的结果
        /// </summary>
        private List<bool> m_lstAbortResult = new List<bool>();

        /// <summary>
        /// 并行任务数
        /// </summary>
        private int m_nTaskCount = 0;

        /// <summary>
        /// 使用的进度条界面
        /// </summary>
        private BaseProcessBarForm m_useProcessBarForm = null;

        /// <summary>
        /// 任务结果等待委托
        /// </summary>
        private TaskDelegate m_waitDelegate = null;

        /// <summary>
        /// 表明任务是否结束
        /// </summary>
        private bool m_bIfEnd = false;
        #endregion

        /// <summary>
        /// 任务强制关闭标示
        /// </summary>
        internal bool IfNoneWait
        {
            get
            {
                //线程锁
                lock (this)
                {
                    return m_bIfNone;
                }
            }

            set
            {
                //线程锁
                lock (this)
                {
                    m_bIfNone = value;
                }
            }
        }

        /// <summary>
        /// 所有的并行任务结果
        /// </summary>
        public List<IAsyncResult> LstAsyncResult
        {
            get
            {
                return m_lstAsyncResult;
            }

            private set
            {
                m_lstAsyncResult = value;
            }
        }

        /// <summary>
        /// 所有的任务
        /// </summary>
        public List<ITask> LstAllTask
        {
            get
            {
                return m_lstAllTask;
            }

            private set
            {
                m_lstAllTask = value;
            }
        }

        /// <summary>
        /// 表明任务是否结束
        /// </summary>
        public bool IfEnd
        {
            get
            {
                return m_bIfEnd;
            }

            private set
            {
                m_bIfEnd = value;
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="inputTaskes"></param>
        public AsyncControlManger(List<ITask> inputTaskes,BaseProcessBarForm useProcessBarForm = null)
        {
            //依附项目
            this.LstAllTask.AddRange(inputTaskes);
            //装配界面
            this.m_useProcessBarForm = useProcessBarForm;
            //执行任务准备
            PrepareTask();
        }

        /// <summary>
        /// 公开方法执行任务
        /// </summary>
        /// <param name="ifWait"></param>
        /// <param name="ifUseProcessBar"></param>
        public void AsyncDoWork(bool ifWait = true, bool ifUseProcessBar = true)
        {
            //带等待进度条
            if (true == ifWait && true == ifUseProcessBar)
            {
                AsyncDoWorkWithProcessBar();
            }
            //不带进度条的等待
            else if (true == ifWait)
            {
                AsyncDoWorkWait();
            }
            //不等待
            else
            {
                AsyncDoWorkNoneWait();
            }
        }

        /// <summary>
        /// 关闭进度条
        /// </summary>
        public void CloseProcessBar()
        {
            //尝试关闭界面
            try
            {
                m_useProcessBarForm.Close();
            }
            catch (Exception ex)
            {
                ;
            }
        }

        /// <summary>
        /// 终止任务
        /// </summary>
        internal void AbortAllTask()
        {
            //线程锁
            lock (this)
            {
                foreach (var oneTaskDecorator in m_lstAllTaskDecorator)
                {
                    //终止并发任务所在线程
                    m_lstAbortResult.Add(oneTaskDecorator.Abort());
                }
            }
        }

        #region 私有方法

        /// <summary>
        /// 执行并行任务，主线程不等待（直接顺序执行）
        /// </summary>
        private void AsyncDoWorkNoneWait()
        {
            //任务派发
            this.AsyncWork();
        }

        /// <summary>
        /// 执行并行任务，主线程等待（所有并发任务结束后顺序执行）
        /// </summary>
        private void AsyncDoWorkWait()
        {
            //任务派发
            this.AsyncWork();

            //等待任务
            WaitTask();

        }

        /// <summary>
        /// 执行带进度条的并发任务
        /// </summary>
        private void AsyncDoWorkWithProcessBar()
        {
            //若界面为空
            if (null == this.m_useProcessBarForm)
            {
                this.m_useProcessBarForm = new BaseProcessBarForm();
            }
            //为界面装配事务管理器
            this.m_useProcessBarForm.UseAsyncManger = this;

            //制作等待任务
            m_waitDelegate = new TaskDelegate(WaitTask);

            //任务派发
            this.AsyncWork();

            //派发等待任务 结束时自动关闭界面
            m_waitDelegate.BeginInvoke(null, null);

            //将进度条以非模态界面展示
            this.m_useProcessBarForm.ShowDialog();
        }

        /// <summary>
        /// 等待任务的完成，尝试关闭进度条界面
        /// </summary>
        private void WaitTask()
        {
            //轮询等待任务完成
            while (false == IfNoneWaitCheck())
            {
                ;
            }

            //联动关闭进度条界面
            if (null != m_useProcessBarForm &&
                false == m_useProcessBarForm.CloseTag)
            {
                CloseProcessBar();
            }
        }

        /// <summary>
        /// 进行任务派发前的准备
        /// </summary>
        private void PrepareTask()
        {
            //获取当前线程
            m_thisThread = Thread.CurrentThread;

            //获取任务数
            m_nTaskCount = LstAllTask.Count;

            //制作任务装饰
            foreach (var oneTask in LstAllTask)
            {
                //准备装饰类
                m_lstAllTaskDecorator.Add(new TaskDecorator(oneTask));
            }

            //制作并行委托
            foreach (var oneTaskDecorator in m_lstAllTaskDecorator)
            {
                //并行任务制作任务委托
                m_lstAllDelegates.Add(new TaskDelegate(oneTaskDecorator.DoTask));
            }
        }

        /// <summary>
        /// 执行并行任务
        /// </summary>
        private void AsyncWork()
        {
            //派发异步，记录结果引用
            foreach (var oneDelegate in m_lstAllDelegates)
            {
                LstAsyncResult.Add(oneDelegate.BeginInvoke(null, null));
            }
        }

        /// <summary>
        /// 判断是否不需要等待
        /// </summary>
        /// <returns></returns>
        private bool IfNoneWaitCheck()
        {
            bool returnValue = true;

            //轮询每个并行任务的状态
            foreach (var oneAsyncResult in LstAsyncResult)
            {
                returnValue = returnValue && oneAsyncResult.IsCompleted;
            }

            //若任务都结束了
            if (true == returnValue)
            {
                //线程锁
                lock (this)
                {
                    this.IfEnd = true;
                }
            }
           
            //若强制不等待
            if (true == IfNoneWait)
            {
                //终止目前正在执行的行为
                AbortAllTask();
                return true;
            }
            else
            {
                return returnValue;
            }
        } 
        #endregion
    }
}
