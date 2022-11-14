
using System.Collections.Generic;
using UnityEngine;
using LibMVCS = XTC.FMP.LIB.MVCS;
using System.Collections;

namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    /// <summary>
    /// 运行时类
    /// </summary>
    ///<remarks>
    /// 存储模块运行时创建的对象
    ///</remarks>
    public class MyRuntime : MyRuntimeBase
    {
        public MyRuntime(MonoBehaviour _mono, MyConfig _config, MyCatalog _catalog, Dictionary<string, LibMVCS.Any> _settings, FMP.LIB.MVCS.Logger _logger, MyEntryBase _entry)
            : base(_mono, _config, _catalog, _settings, _logger, _entry)
        {
        }

        public void DirectOpenInstanceAsync(string _uid, string _style, string _source, string _uri, float _delay, float _positionX, float _positionY)
        {
            mono_.StartCoroutine(directOpenInstanceAsync(_uid, _style, _source, _uri, _delay, _positionX, _positionY));
        }

        public void DirectCloseInstanceAsync(string _uid, float _delay)
        {
            mono_.StartCoroutine(directCloseInstanceAsync(_uid, _delay));
        }

        private IEnumerator directOpenInstanceAsync(string _uid, string _style, string _source, string _uri, float _delay, float _positionX, float _positionY)
        {
            logger_.Debug("directopen instance of {0}, uid is {1}, style is {2}", MyEntryBase.ModuleName, _uid, _style);
            // 延时一帧执行，在发布消息时不能动态注册
            yield return new WaitForEndOfFrame();

            MyInstance instance;
            if (instances.TryGetValue(_uid, out instance))
            {
                yield break;
            }

            instance = new MyInstance(_uid, _style, config_, catalog_, logger_, settings_, entry_, mono_, rootAttachment);
            instance.GenerateAlignGrid(rootUI.GetComponent<RectTransform>().rect.size);
            // 获取吸附后的坐标
            instance.StickAlignGrid(_positionX, _positionY);
            // 检测吸附后的坐标是否和其他已存在的实例产生交集
            if (detectCross(instance))
                yield break;

            instance.preloadsRepetition = new Dictionary<string, object>(preloads_);
            instances[_uid] = instance;
            instance.InstantiateUI(instanceUI);
            instance.themeObjectsPool.Prepare();
            instance.HandleCreated();
            // 动态注册直系的MVCS
            entry_.DynamicRegister(_uid, logger_);
            instance.SetupBridges();
            yield return new WaitForSeconds(_delay);
            instance.contentObjectsPool.Prepare();
            instance.rootUI.transform.Find("Board").GetComponent<RectTransform>().anchoredPosition = new Vector2(instance.stickedX, instance.stickedY);
            instance.HandleOpened(_source, _uri);
        }

        private IEnumerator directCloseInstanceAsync(string _uid, float _delay)
        {
            logger_.Debug("directclose instance of {0}, uid is {1}", MyEntryBase.ModuleName, _uid);
            MyInstance instance;
            if (!instances.TryGetValue(_uid, out instance))
            {
                yield break;
            }
            yield return new WaitForSeconds(_delay);
            instance.HandleClosed();
            instance.contentObjectsPool.Dispose();
            // 延时一帧执行，在发布消息时不能动态注销
            yield return new WaitForEndOfFrame();
            instance.HandleDeleted();
            GameObject.Destroy(instance.rootUI);
            instances.Remove(_uid);
            instance.themeObjectsPool.Dispose();
            // 动态注销直系的MVCS
            entry_.DynamicCancel(_uid, logger_);
        }

        private bool detectCross(MyInstance _instance)
        {
            bool hasCross = false;
            foreach (var instance in instances.Values)
            {
                if (null == instance.stickedAlignGrid)
                    continue;
                if (instance.stickedAlignGrid.Row == _instance.stickedAlignGrid.Row && instance.stickedAlignGrid.Column == _instance.stickedAlignGrid.Column)
                {
                    hasCross = true;
                    break;
                }
            }
            return hasCross;
        }

    }
}

