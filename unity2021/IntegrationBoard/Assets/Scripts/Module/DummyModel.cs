
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
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
            public Dictionary<string, int> likeStatusS { get; set; } = new Dictionary<string, int>();
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

        protected override void postSetup()
        {
            base.postSetup();
            //TODO 临时方案
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                return;
            var file = Path.Combine(Application.persistentDataPath, "like.json");
            if (File.Exists(file))
            {
                status.likeStatusS = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(file));
            }
        }

        public void SaveAddLike(string _content)
        {
            int count = 0;
            status.likeStatusS.TryGetValue(_content, out count);
            count += 1;
            status.likeStatusS[_content] = count;

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                return;
            var file = Path.Combine(Application.persistentDataPath, "like.json");
            try
            {
                File.WriteAllText(file, JsonConvert.SerializeObject(status.likeStatusS));
            }
            catch (System.Exception ex)
            {
                getLogger().Exception(ex);
            }
        }
    }
}

