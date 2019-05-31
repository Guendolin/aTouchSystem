using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aSystem.aTouchSystem
{
    public class aTouchSystem : MonoBehaviour
    {
        public static aTouchSystem Instance;

        public Camera touchCamera;

        private List<IaTouchSoruce> _touchSoruces;
        private Queue<aTouch> _touchPool;
        private List<aTouch> _activeTouches;

        public delegate void OnATouchUpdate(List<aTouch> activeTouches);
        private event OnATouchUpdate _aTouchUpdateEvent;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            if (touchCamera == null)
            {
                touchCamera = Camera.main;
            }

            _touchPool = new Queue<aTouch>();
            _activeTouches = new List<aTouch>();
            _touchSoruces = new List<IaTouchSoruce>();

            _touchSoruces.Add(new aTouchSource());

 #if UNITY_EDITOR
            _touchSoruces.Add(new aTouchProxy());
#endif

            for (int i = 0; i < _touchSoruces.Count; i++)
            {
                _touchSoruces[i].Setup(this);
            }
        }

        void Update()
        {
            for (int i = 0; i < _touchSoruces.Count; i++)
            {
                _touchSoruces[i].Update();
            }
       
            for (int i = _activeTouches.Count-1; i >= 0 ; i--)
            {
                if (!_activeTouches[i].ActiveThisFrame())
                {
                    aTouch aTouch = _activeTouches[i];
                    _activeTouches.RemoveAt(i);
                    _touchPool.Enqueue(aTouch);
                }
            }

            if(_activeTouches.Count > 0 && _aTouchUpdateEvent != null)
            {
                _aTouchUpdateEvent(_activeTouches);
            }
        }

        public void SubscripeToTouches(OnATouchUpdate touchUpdate)
        {
            _aTouchUpdateEvent += touchUpdate;
        }

        public void DesubscripeToTouches(OnATouchUpdate touchUpdate)
        {
            _aTouchUpdateEvent -= touchUpdate;
        }

        public aTouch GetNewTouch()
        {
            if(_touchPool.Count > 0)
            {
                return _touchPool.Dequeue();
            }     
            return new aTouch();
        }
  
        public aTouch GetActiveTouch(int touchId)
        {
            for(int i = 0; i < _activeTouches.Count; i++)
            {
                aTouch aTouch = _activeTouches[i];
                if(aTouch.id == touchId)
                {
                    return aTouch; 
                }
            }
            return null;
        }

        public void AddToActiveTouches(aTouch touch)
        {
            if (!_activeTouches.Contains(touch))
            {
                _activeTouches.Add(touch);
            }
        }
    }
}
