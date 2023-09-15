using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    public class AutoCloseMono : MonoBehaviour
    {
        public Action onTimeout { get; set; }

        public Func<Vector2, bool> checkFunc { get; set; }

        public int Timeout { get; set; } = 30;

        private float timer_ = 0;
        private bool stoped = true;

        public void Restart()
        {
            timer_ = 0;
            stoped = false;
        }

        public void Stop()
        {
            if (stoped)
                return;
            stoped = true;
            onTimeout();
        }

        // Update is called once per frame
        void Update()
        {
            if (stoped)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (checkFunc(Input.mousePosition))
                {
                    timer_ = 0;
                }
            }

            timer_ += Time.deltaTime;
            if (timer_ > Timeout)
            {
                stoped = true;
                onTimeout();
            }

        }
    }
}
