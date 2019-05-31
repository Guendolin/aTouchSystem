using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aSystem.aTouchSystem
{
    public class aTouchSource : IaTouchSoruce
    {
        private aTouchSystem _touchSystem;

        public void Setup(aTouchSystem touchSystem)
        {
            _touchSystem = touchSystem;
        }

        public void Update()
        {
            //Update Touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.touches[i];
                aTouch activeTouch = null;
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        //Setup new touch
                        activeTouch = _touchSystem.GetNewTouch();

                        activeTouch.id = touch.fingerId;
                        activeTouch.touch = touch;
                        activeTouch.touchState = aTouchState.Press;
                        activeTouch.startTime = Time.time;

                        activeTouch.startScreenPos = activeTouch.previusScreenPos = activeTouch.screenPos = touch.position;
                        activeTouch.deltaScreenPos = touch.deltaPosition;
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        //update old touch
                        activeTouch = _touchSystem.GetActiveTouch(touch.fingerId);

                        activeTouch.touchState = aTouchState.Hold;

                        activeTouch.previusScreenPos = activeTouch.screenPos;
                        activeTouch.screenPos = touch.position;
                        activeTouch.deltaScreenPos = touch.deltaPosition;
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                    default:
                        activeTouch = _touchSystem.GetActiveTouch(touch.fingerId);

                        activeTouch.touchState = aTouchState.Release;
                        activeTouch.previusScreenPos = activeTouch.screenPos;
                        activeTouch.screenPos = touch.position;
                        activeTouch.deltaScreenPos = touch.deltaPosition;
                        break;
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
