
using System;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.IntegrationBoard.LIB.Bridge;
using XTC.FMP.MOD.IntegrationBoard.LIB.MVCS;
using System.Collections.Generic;

namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    /// <summary>
    /// 虚拟视图，用于处理消息订阅
    /// </summary>
    public class DummyView : DummyViewBase
    {
        public DummyView(string _uid) : base(_uid)
        {
        }

        protected override void setup()
        {
            base.setup();
            addSubscriber(MySubject.ActivatePage, handleActivatePage);
        }

        private void handleActivatePage(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle {0} ", MySubject.ActivatePage);
            Dictionary<string, object> data = _data as Dictionary<string, object>;
            if (null == data)
            {
                getLogger().Error("data is null");
                return;
            }

            object objUid;
            string uid = "";
            if (data.TryGetValue("uid", out objUid))
            {
                uid = objUid as string;
            }
            if (string.IsNullOrEmpty(uid))
            {
                getLogger().Error("uid is null or empry");
                return;
            }

            object objPage;
            string page = "";
            if (data.TryGetValue("page", out objPage))
            {
                page = objPage as string;
            }
            if (string.IsNullOrEmpty(page))
            {
                getLogger().Error("page is null or empry");
                return;
            }

            MyInstance instance = null;
            runtime.instances.TryGetValue(uid, out instance);
            if (null == instance)
            {
                getLogger().Error("instance is null");
            }

            instance.ActivatePage(page);
        }
    }
}

