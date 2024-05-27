
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 根程序类
/// </summary>
/// <remarks>
/// 不参与模块编译，仅用于在编辑器中开发调试
/// </remarks>
public class Root : RootBase
{
    private List<string> directOpenInstanceS_ = new List<string>();
    private void Awake()
    {
        doAwake();
    }

    private void Start()
    {
        entry_.__DebugPreload(exportRoot);
    }

    private void OnDestroy()
    {
        doDestroy();
    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Input.mousePosition.x);
            Debug.Log(Input.mousePosition.y);
            entry_.__DebugDirectOpen(System.DateTime.UtcNow.ToString(), "rectangle", "assloud://", "XTC.IntegrationBoard/1", 0, Input.mousePosition.x, Input.mousePosition.y);
        }
        */
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 60, 30), "Create"))
        {
            entry_.__DebugCreate("test", "rectangle");
        }

        if (GUI.Button(new Rect(0, 30, 60, 30), "Open"))
        {
            entry_.__DebugOpen("test", "assloud://", "XTC.IntegrationBoard/1", 0.5f);
        }

        if (GUI.Button(new Rect(0, 60, 60, 30), "Show"))
        {
            entry_.__DebugShow("test", 0.5f);
        }

        if (GUI.Button(new Rect(0, 90, 60, 30), "Hide"))
        {
            entry_.__DebugHide("test", 0.5f);
        }

        if (GUI.Button(new Rect(0, 120, 60, 30), "Close"))
        {
            entry_.__DebugClose("test", 0.5f);
        }

        if (GUI.Button(new Rect(0, 150, 60, 30), "Delete"))
        {
            entry_.__DebugDelete("test");
        }

        if (GUI.Button(new Rect(0, 180, 100, 30), "DirectOpen"))
        {
            var uid = System.DateTime.UtcNow.ToString();
            directOpenInstanceS_.Add(uid);
            entry_.__DebugDirectOpen(uid, "circle", "assloud://", "XTC.IntegrationBoard/1", 0, Random.Range(-Screen.width / 2, Screen.width / 2), Random.Range(-Screen.height / 2, Screen.height / 2), "MainCanvas");
        }

        if (GUI.Button(new Rect(0, 210, 100, 30), "Refresh"))
        {
            entry_.__DebugRefresh("test", "assloud://", "XTC.IntegrationBoard", "2");
        }

        if (GUI.Button(new Rect(0, 240, 100, 30), "DirectClose"))
        {
            foreach (var uid in directOpenInstanceS_)
                entry_.__DebugDirectClose(uid, 0);
        }

        if (GUI.Button(new Rect(0, 270, 200, 30), "ResetAutoCloseTimer"))
        {
            foreach (var uid in directOpenInstanceS_)
                entry_.__DebugResetAutoCloseTimer(uid);
        }

        if (GUI.Button(new Rect(0, 300, 100, 30), "Close All"))
        {
            entry_.__DebugCloseAll();
        }

    }
}

