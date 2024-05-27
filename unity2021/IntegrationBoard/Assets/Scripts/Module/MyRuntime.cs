
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

        public void DirectOpenInstanceAsync(string _uid, string _style, string _source, string _uri, float _delay, float _positionX, float _positionY, string _uiSlot, string _sender)
        {
            mono_.StartCoroutine(directOpenInstanceAsync(_uid, _style, _source, _uri, _delay, _positionX, _positionY, _uiSlot, _sender));
        }

        public void DirectCloseInstanceAsync(string _uid, float _delay)
        {
            mono_.StartCoroutine(directCloseInstanceAsync(_uid, _delay));
        }

        public void CloseAllInstanceAsync()
        {
            mono_.StartCoroutine(closeAllInstanceAsync());
        }

        private IEnumerator directOpenInstanceAsync(string _uid, string _style, string _source, string _uri, float _delay, float _positionX, float _positionY, string _uiSlot, string _sender)
        {
            logger_.Debug("directopen instance of {0}, uid is {1}, style is {2}, uiSlot is {3}, sender is {4}", MyEntryBase.ModuleName, _uid, _style, _uiSlot, _sender);
            // 延时一帧执行，在发布消息时不能动态注册
            yield return new WaitForEndOfFrame();

            MyInstance instance;
            if (instances.TryGetValue(_uid, out instance))
            {
                yield break;
            }

            // 获取ui挂载点
            Transform parentUi = instanceUI.transform.parent;
            if (!string.IsNullOrEmpty(_uiSlot))
            {
                parentUi = GameObject.Find(_uiSlot).transform;
                if (null == parentUi)
                {
                    logger_.Error("uiSlot {0} not found", _uiSlot);
                    parentUi = instanceUI.transform.parent;
                }
            }
            // 获取世界挂载点
            Transform parentWorld = instanceWorld.transform.parent;

            instance = new MyInstance(_uid, _style, config_, catalog_, logger_, settings_, entry_, mono_, rootAttachment);
            instance.sender = _sender;
            Vector2 viewportSize = parentUi.GetComponent<RectTransform>().rect.size;
            instance.GenerateAlignGrid(viewportSize);
            // 矫正坐标
            instance.AdjustPosition(_positionX, _positionY, viewportSize);
            // 检测吸附后的坐标是否和其他已存在的实例产生交集
            if (detectCross(instance))
            {
                logger_.Debug("detectCross == true, so ignore open");
                yield break;
            }

            instance.preloadsRepetition = new Dictionary<string, object>(preloads_);
            instances[_uid] = instance;

            // 实例化ui
            instance.InstantiateUI(instanceUI, parentUi);
            // 实例化world
            instance.InstantiateWorld(instanceWorld, parentWorld);

            instance.themeObjectsPool.Prepare();
            instance.HandleCreated();
            yield return new WaitForEndOfFrame();

            // 动态注册直系的MVCS
            entry_.DynamicRegister(_uid, logger_);
            instance.SetupBridges();
            yield return new WaitForSeconds(_delay);
            instance.assetObjectsPool.Prepare();
            instance.rootUI.transform.Find("Board").GetComponent<RectTransform>().anchoredPosition = new Vector2(instance.adjustedX, instance.adjustedY);
            instance.HandleOpened(_source, _uri);
            // 覆盖自动关闭超时事件
            instance.onAutoCloseTimeout = () =>
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["uid"] = _uid;
                parameters["delay"] = 0f;
                entry_.getDummyModel().Publish(MySubject.DirectClose, parameters);
            };
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
            instance.assetObjectsPool.Dispose();
            // 延时一帧执行，在发布消息时不能动态注销
            yield return new WaitForEndOfFrame();
            instance.HandleDeleted();
            GameObject.Destroy(instance.rootUI);
            instances.Remove(_uid);
            instance.themeObjectsPool.Dispose();
            // 动态注销直系的MVCS
            entry_.DynamicCancel(_uid, logger_);
        }

        private IEnumerator closeAllInstanceAsync()
        {
            yield return new WaitForEndOfFrame();

            foreach (MyInstance instance in instances.Values)
            {
                instance.HandleClosed();
                instance.assetObjectsPool.Dispose();
                // 延时一帧执行，在发布消息时不能动态注销
                yield return new WaitForEndOfFrame();
                instance.HandleDeleted();
                GameObject.Destroy(instance.rootUI);
                instance.themeObjectsPool.Dispose();
                // 动态注销直系的MVCS
                entry_.DynamicCancel(instance.uid, logger_);
            }
            instances.Clear();
        }

        private bool detectCross(MyInstance _instance)
        {
            bool hasCross = false;
            // 遍历所有实例
            //TODO 只检测同深度值(depth)的实例
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

