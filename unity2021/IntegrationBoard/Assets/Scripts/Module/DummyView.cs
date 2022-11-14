
using System;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.IntegrationBoard.LIB.Bridge;
using XTC.FMP.MOD.IntegrationBoard.LIB.MVCS;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

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
            addSubscriber(MySubject.DirectOpen, handleDirectOpen);
            addSubscriber(MySubject.DirectClose, handleDirectClose);
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

        private void handleDirectOpen(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle directopen {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            string style = "";
            string source = "";
            string uri = "";
            float delay = 0f;
            float position_x = 0f;
            float position_y = 0f;
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
                style = (string)data["style"];
                source = (string)data["source"];
                uri = (string)data["uri"];
                delay = (float)data["delay"];
                position_x = (float)data["position_x"];
                position_y = (float)data["position_y"];
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
                return;
            }

            runtime.DirectOpenInstanceAsync(uid, style, source, uri, delay, position_x, position_y);
        }

        private void handleDirectClose(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle directclose {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            float delay = 0f;
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
                delay = (float)data["delay"];
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
                return;
            }

            runtime.DirectCloseInstanceAsync(uid, delay);
        }
    }
}

