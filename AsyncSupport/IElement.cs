using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncSupport
{
    /// <summary>
    /// 使用的对象
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// 对象与对象之间的交互方法
        /// </summary>
        /// <param name="anotherElement">另一对象</param>
        /// <returns>获取的结果</returns>
        Object Interactive(IElement anotherElement);
    }
}
