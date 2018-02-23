using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSupport
{
    /// <summary>
    /// 根据输入数量切分版协助器
    /// </summary>
    class SplitAndAllWaitHalfNSquareSupport : HalfNSquareSupport
    {
        public SplitAndAllWaitHalfNSquareSupport(IList<IElement> inputLstElement, int? useThreadCount) : base(inputLstElement, useThreadCount)
        {
        }

        public override void Calculate()
        {
            var useFactory = Task.Factory;

            int useCount = m_nUseCoreNumber;

            int span = m_lstUseElment.Count / useCount;

            //防止出现0次增长现象
            span = Math.Max(1, span);

            //临时列表
            List<Task> lstTempTask = new List<Task>();
            SupportBean tempBean;
            for (int tempIndex = 0; tempIndex < m_lstUseElment.Count; tempIndex = tempIndex + span)
            {
                //下次索引
                int tempEndIndex = Math.Min(m_lstUseElment.Count, tempIndex + span);
                tempBean = new SupportBean();
                tempBean.StartIndex = tempIndex;
                tempBean.EndIndex = tempEndIndex;

                lstTempTask.Add(useFactory.StartNew(SubCalculate, tempBean));
            }

            Task.WaitAll(lstTempTask.ToArray());

        }

        /// <summary>
        /// 分组子计算
        /// </summary>
        /// <param name="inputBean">使用的Bean</param>
        private void SubCalculate(object inputBean)
        {
            SupportBean useBean = inputBean as SupportBean;

            for (int tempIndex = useBean.StartIndex; tempIndex < useBean.EndIndex; tempIndex++)
            {
                CalculateByOneRow(tempIndex);
            }
        }

        /// <summary>
        /// 协助封装
        /// </summary>
        private class SupportBean
        {
            private int m_nStartIndex;

            private int m_nEndIndex;

            internal int StartIndex
            {
                get
                {
                    return m_nStartIndex;
                }

                set
                {
                    m_nStartIndex = value;
                }
            }

            internal int EndIndex
            {
                get
                {
                    return m_nEndIndex;
                }

                set
                {
                    m_nEndIndex = value;
                }
            }
        }

    }
}
