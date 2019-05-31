using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aSystem.aTouchSystem
{
    public class aTouchProxy : IaTouchSoruce
    {
        private const int MOUSE_INPUTS = 3;

        private aTouchSystem _touchSystem;

        public void Setup(aTouchSystem touchSystem)
        {
            _touchSystem = touchSystem;
        }

        public void Update()
        {
            //Update Proxy
            for (int i = 0; i < MOUSE_INPUTS; i++)
            {
                aTouch activeTouch = null;
                if (Input.GetMouseButtonDown(i))
                {
                    // Proxy start
                    activeTouch = _touchSystem.GetNewTouch();
                
                    activeTouch.id = -i;
                    activeTouch.touch = null;
                    activeTouch.startTime = Time.time;

                    activeTouch.touchState = aTouchState.Press;
                    activeTouch.startScreenPos = activeTouch.previusScreenPos = activeTouch.screenPos = Input.mousePosition;
                    activeTouch.deltaScreenPos = Vector2.zero;

                }
                else if (Input.GetMouseButtonUp(i))
                {
                    // proxy end
                    activeTouch = _touchSystem.GetActiveTouch(-i);

                    activeTouch.touchState = aTouchState.Release;
                    activeTouch.previusScreenPos = activeTouch.screenPos;
                    activeTouch.screenPos = Input.mousePosition;
                    activeTouch.deltaScreenPos = activeTouch.screenPos - activeTouch.previusScreenPos;
                }
                else if (Input.GetMouseButton(i))
                {
                    // proxy update
                    activeTouch = _touchSystem.GetActiveTouch(-i);

                    activeTouch.touchState = aTouchState.Hold;
                    activeTouch.previusScreenPos = activeTouch.screenPos;
                    activeTouch.screenPos = Input.mousePosition;
                    activeTouch.deltaScreenPos = activeTouch.screenPos - activeTouch.previusScreenPos;
                }

                if(activeTouch != null)
                {
                    activeTouch.Update();
                    _touchSystem.AddToActiveTouches(activeTouch);
                }
            }
        }
    }
}
