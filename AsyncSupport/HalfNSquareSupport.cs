using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSupport
{
    /// <summary>
    /// 半数O(n*n)算法协助管理器
    /// IElement的Interactive 要符合交换率
    /// </summary>
    public abstract class HalfNSquareSupport
    {
        #region 私有对象
        /// <summary>
        /// 输入的ELement列表
        /// </summary>
        protected IList<IElement> m_lstInputElement;

        /// <summary>
        /// 输入的对象
        /// </summary>
        protected IList<IElement> m_lstUseElment;

        /// <summary>
        /// 使用的核心数
        /// </summary>
        protected int m_nUseCoreNumber;

        /// <summary>
        /// 使用的结果对象
        /// </summary>
        protected Dictionary<IElement, Dictionary<IElement, object>> m_dicResults;
        #endregion

        /// <summary>
        /// 使用的结果对象
        /// </summary>
        public Dictionary<IElement, Dictionary<IElement, object>> DicResults
        {
            get
            {
                return m_dicResults;
            }

            private set
            {
                m_dicResults = value;
            }
        }

        /// <summary>
        /// 计算
        /// </summary>
        public abstract void Calculate();

        /// <summary>
        /// 获取一个辅助器
        /// </summary>
        /// <param name="inputLstElement">输入的Element列表</param>
        /// <param name="useThreadCount">使用的核心数</param>
        /// <returns>获取的辅助器</returns>
        public static HalfNSquareSupport GetOneManger(IList<IElement> inputLstElement, int? useThreadCount)
        {
            return new SplitAndAllWaitHalfNSquareSupport(inputLstElement, useThreadCount);
        }

        #region 私有与保护方法
        /// <summary>
        /// 准备ELement列表
        /// </summary>
        /// <param name="input">输入的列表</param>
        /// <returns>调整后的列表</returns>
        private IList<IElement> PrepareLstElement(IList<IElement> input)
        {
            List<IElement> retrunValue = (from n in input where null != n select n).Distinct().ToList();

            return retrunValue;
        }

        /// <summary>
        /// 获取计算机核心数
        /// </summary>
        /// <returns></returns>
        private int GetMachineCoreNumber()
        {
            return Environment.ProcessorCount;
        }

        /// <summary>
        /// 单行计算
        /// </summary>
        /// <param name="inputIndex">使用的当前索引</param>
        /// <returns></returns>
        protected void CalculateByOneRow(object inputIndex)
        {
            int useIndex = (int)inputIndex;
            CalculateByOneRow(useIndex);
        }

        /// <summary>
        /// 单行计算
        /// </summary>
        /// <param name="inputIndex">使用的当前索引</param>
        /// <returns></returns>
        protected void CalculateByOneRow(int useIndex)
        {
            IElement useElement = m_lstUseElment[useIndex];
            //返回值
            Dictionary<IElement, object> returnDic = new Dictionary<IElement, object>();

            object tempObj = null;

            //单行循环
            for (int tempIndex = useIndex + 1; tempIndex < m_lstUseElment.Count; tempIndex++)
            {
                try
                {
                    //尝试计算
                    tempObj = useElement.Interactive(m_lstUseElment[tempIndex]);
                }
                catch (Exception)
                {
                    tempObj = null;
                }

                //赋值
                if (null != tempObj)
                {
                    returnDic.Add(m_lstUseElment[tempIndex], tempObj);
                }

            }

            //并发赋值（没有行级冲突）
            DicResults[useElement] = returnDic;
            return;
        }

        /// <summary>
        /// 构造半数n^2算法协助器
        /// </summary>
        /// <param name="inputLstElement">输入的Element列表</param>
        /// <param name="useThreadCount">使用的核心数</param>
        protected HalfNSquareSupport(IList<IElement> inputLstElement, int? useThreadCount)
        {
            m_lstInputElement = inputLstElement;
            m_lstUseElment = PrepareLstElement(m_lstInputElement);

            if (null == useThreadCount)
            {
                m_nUseCoreNumber = GetMachineCoreNumber();
            }
            else
            {
                m_nUseCoreNumber = useThreadCount.Value;
            }

            //初始化结果列表
            m_dicResults = m_lstUseElment.ToDictionary(k => k, new Func<IElement, Dictionary<IElement, object>>(k => null));
        }
        #endregion

    }
}
