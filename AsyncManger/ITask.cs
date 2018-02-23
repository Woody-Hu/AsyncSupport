using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncManger
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 获取任务的返回值
        /// </summary>
        object ReturnValue
        {
            get;
        }

        /// <summary>
        /// 在终止线程时调用，尝试清理资源，反正出现并发问题
        /// </summary>
        /// <returns></returns>
        bool TryAbort();

        /// <summary>
        /// 核心行为，执行任务
        /// </summary>
        void DoTask();

    }
}
