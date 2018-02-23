using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSupport
{
    /// <summary>
    /// 全等待版的计算协助器
    /// </summary>
    internal class AllWaitHalfNSquareSupport : HalfNSquareSupport
    {
        public AllWaitHalfNSquareSupport(IList<IElement> inputLstElement, int? useThreadCount) 
            : base(inputLstElement, useThreadCount)
        {
            
        }

        public override void Calculate()
        {
            //临时列表
            List<Task> lstTempTask = new List<Task>();
            for (int useIndex = 0; useIndex < m_lstUseElment.Count; useIndex++)
            {
                //添加异步任务
                lstTempTask.Add(Task.Factory.StartNew(CalculateByOneRow, useIndex));
            }

            //休眠
            Task.WaitAll(lstTempTask.ToArray());
        }
    }
}
