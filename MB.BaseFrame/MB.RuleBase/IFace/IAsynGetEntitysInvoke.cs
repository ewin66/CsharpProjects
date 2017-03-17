﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MB.Util.Model;
using System.ServiceModel;

namespace MB.RuleBase.IFace {
    /// <summary>
    /// 基于实体类大数据获取必须实现的接口。
    /// 在生成客户端的时候注意要生成异步的访问方式。
    /// </summary>
    [ServiceContract]
    public interface IAsynGetEntitysInvoke {
        /// <summary>
        /// 实现该方法,并根据不同的参数进行相应的处理。 
        /// </summary>
        /// <param name="invokeParams">异步调用的参数。</param>
        /// <returns>GreatCapacityResult</returns>
        [FaultContract(typeof(MB.Util.Model.WcfFaultMessage))]
        [OperationContract]
        GreatCapacityResult ReceiveGreatDataInvoke(GreatCapacityInvokeParamInfo invokeParams);
    }
}
