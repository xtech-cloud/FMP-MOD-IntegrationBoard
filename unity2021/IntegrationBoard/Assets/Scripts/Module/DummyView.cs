
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
            addSubscriber(MySubject.CloseAll, handleCloseAll);
            addSubscriber(MySubject.Refresh, handleRefresh);
            addSubscriber(MySubject.ResetAutoCloseTimer, handleResetAutoCloseTimer);
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
            string sender = "";
            string uid = "";
            string style = "";
            string source = "";
            string uiSlot = "";
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
                if (data.ContainsKey("uiSlot"))
                    uiSlot = (string)data["uiSlot"];
                if (data.ContainsKey("sender"))
                    sender = (string)data["sender"];
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
                return;
            }

            runtime.DirectOpenInstanceAsync(uid, style, source, uri, delay, position_x, position_y, uiSlot, sender);
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

        private void handleCloseAll(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle close all {0} with data", MyEntryBase.ModuleName);
            runtime.CloseAllInstanceAsync();
        }

        private void handleRefresh(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle refresh {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            string source = "";
            string bundle_uuid = "";
            string content_uuid = "";
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
                source = (string)data["source"];
                bundle_uuid = (string)data["bundle_uuid"];
                content_uuid = (string)data["content_uuid"];
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
                return;
            }

            MyInstance instance;
            if (!runtime.instances.TryGetValue(uid, out instance))
            {
                getLogger().Error("instance not found");
                return;
            }

            instance.RefreshContent(source, string.Format("{0}/{1}", bundle_uuid, content_uuid));
        }

        private void handleResetAutoCloseTimer(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle ResetAutoCloseTimer {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
                return;
            }

            MyInstance instance;
            if (!runtime.instances.TryGetValue(uid, out instance))
            {
                getLogger().Error("instance not found");
                return;
            }

            instance.ResetAutoCloseTimer();
        }
    }
}

