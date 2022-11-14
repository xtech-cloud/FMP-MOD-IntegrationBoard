
using System;
using System.Collections.Generic;
using LibMVCS = XTC.FMP.LIB.MVCS;

namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    /// <summary>
    /// 虚拟数据
    /// </summary>
    public class DummyModel : DummyModelBase
    {

        public class DummyStatus : DummyStatusBase
        {
        }

        public DummyModel(string _uid) : base(_uid)
        {
        }

        private DummyStatus status
        {
            get
            {
                return status_ as DummyStatus;
            }
        }
    }
}

