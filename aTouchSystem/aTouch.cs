using UnityEngine;

namespace aSystem.aTouchSystem
{
    public enum aTouchState { Press, Hold, Release};

    public class aTouch
    {
        public int id;
        public Touch? touch;

        public aTouchState touchState;
        public Vector2 screenPos;
        public Vector2 previusScreenPos;
        public Vector2 startScreenPos;
        public Vector2 deltaScreenPos;
        public float startTime;

        public float touchTime
        {
            get {
                return Time.time - startTime;
            }
        }

        public Vector2 fromStart
        {
            get
            {
                return screenPos - startScreenPos;
            }
        }

        public const float TAPP_MOVE_LIMIT = 20 * 20f; // Limit move for 20 pixels. This is dangerous because this changed depending on screen rez.
        public const float TAPP_TOUCH_LIMIT = 0.35f;

        public bool Tapp
        {
            get
            {
                return fromStart.sqrMagnitude < TAPP_MOVE_LIMIT && touchTime < TAPP_TOUCH_LIMIT;
            }
        }

        //This way of checjking 
        private int _lastFrameCount;

        // Should maybe be renamed
        public void Update()
        {
            _lastFrameCount = Time.frameCount;
        }

        // Should maybe be renamed.
        public bool ActiveThisFrame()
        {
            return _lastFrameCount == Time.frameCount;
        }

        public Ray ToRay(Camera camera = null, bool previus = false)
        {
            if (camera == null)
            {
                camera = aTouchSystem.Instance.touchCamera;
            }

            Vector2 pos = previus ? previusScreenPos : screenPos;  
            return aTouchSystemUtils.ToRay(pos, camera);
        }

        public bool PositionOnPlane(Plane plane, out Vector3 postionOnPlane, Camera camera = null, bool previus = false)
        {
            return aTouchSystemUtils.PositionOnPlane(plane, ToRay(camera, previus), out postionOnPlane);
        }

        public Vector3 PositionOnPlane(Plane plane, Camera camera = null, bool previus = false)
        {
            return aTouchSystemUtils.PositionOnPlane(plane, ToRay(camera, previus));
        }
    }
}
