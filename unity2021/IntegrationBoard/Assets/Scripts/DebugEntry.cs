
using System.Collections.Generic;
using UnityEngine;

namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    /// <summary>
    /// 调试入口
    /// </summary>
    /// <remarks>
    /// 不参与模块编译，仅用于在编辑器中开发调试
    /// </remarks>
    public class DebugEntry : MyEntry
    {
        /// <summary>
        /// 调试预加载
        /// </summary>
        public void __DebugPreload(GameObject _exportRoot)
        {
            processRoot(_exportRoot);
            runtime_.Preload((_percentage) =>
            {
            }, () =>
            {
                createInstances(() =>
                {
                    publishPreloadSubjects();
                });
            });
        }

        /// <summary>
        /// 调试创建
        /// </summary>
        /// <param name="_uid">实例的uid</param>
        /// <param name="_style">实例的样式名</param>
        public void __DebugCreate(string _uid, string _style)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            data["style"] = _style;
            data["uiSlot"] = "MainCanvas/[root]";
            data["worldSlot"] = "MainWorld/[root]";
            modelDummy_.Publish(MySubjectBase.Create, data);
        }

        /// <summary>
        /// 调试打开
        /// </summary>
        /// <param name="_uid">实例的uid</param>
        /// <param name="_source">内容的源的类型</param>
        /// <param name="_uri">内容的地址</param>
        /// <param name="_delay">延迟时间，单位秒</param>
        public void __DebugOpen(string _uid, string _source, string _uri, float _delay)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            data["source"] = _source;
            data["uri"] = _uri;
            data["delay"] = _delay;
            modelDummy_.Publish(MySubjectBase.Open, data);
        }

        /// <summary>
        /// 调试显示
        /// </summary>
        /// <param name="_uid">实例的uid</param>
        /// <param name="_delay">延迟时间，单位秒</param>
        public void __DebugShow(string _uid, float _delay)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            data["delay"] = _delay;
            modelDummy_.Publish(MySubjectBase.Show, data);
        }

        /// <summary>
        /// 调试隐藏
        /// </summary>
        /// <param name="_uid">实例的uid</param>
        /// <param name="_delay">延迟时间，单位秒</param>
        public void __DebugHide(string _uid, float _delay)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            data["delay"] = _delay;
            modelDummy_.Publish(MySubjectBase.Hide, data);
        }

        /// <summary>
        /// 调试关闭
        /// </summary>
        /// <param name="_uid">实例的uid</param>
        /// <param name="_delay">延迟时间，单位秒</param>
        public void __DebugClose(string _uid, float _delay)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            data["delay"] = _delay;
            modelDummy_.Publish(MySubjectBase.Close, data);
        }

        /// <summary>
        /// 调试删除
        /// </summary>
        /// <param name="_uid">实例的uid</param>
        public void __DebugDelete(string _uid)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            modelDummy_.Publish(MySubjectBase.Delete, data);
        }

        public void __DebugDirectOpen(string _uid, string _style, string _source, string _uri, float _delay, float _positionX, float _positionY, string _uiSlot)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            data["style"] = _style;
            data["source"] = _source;
            data["uri"] = _uri;
            data["delay"] = _delay;
            data["position_x"] = _positionX;
            data["position_y"] = _positionY;
            data["uiSlot"] = _uiSlot;
            modelDummy_.Publish(MySubject.DirectOpen, data);
        }

        public void __DebugDirectClose(string _uid, float _delay)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            data["delay"] = _delay;
            modelDummy_.Publish(MySubject.DirectClose, data);
        }

        public void __DebugCloseAll()
        {
            var data = new Dictionary<string, object>();
            modelDummy_.Publish(MySubject.CloseAll, data);
        }

        public void __DebugRefresh(string _uid, string _source, string _bundleUUID, string _contentUUID)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            data["source"] = _source;
            data["bundle_uuid"] = _bundleUUID;
            data["content_uuid"] = _contentUUID;
            modelDummy_.Publish(MySubject.Refresh, data);
        }

        public void __DebugResetAutoCloseTimer(string _uid)
        {
            var data = new Dictionary<string, object>();
            data["uid"] = _uid;
            modelDummy_.Publish(MySubject.ResetAutoCloseTimer, data);
        }
    }
}
