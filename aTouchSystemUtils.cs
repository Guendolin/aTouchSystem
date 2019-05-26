using UnityEngine;

namespace aSystem.aTouchSystem
{
    public static class aTouchSystemUtils
    {
        public static Ray ToRay(Vector2 screenPos, Camera camera)
        {
            return camera.ScreenPointToRay(screenPos);
        }

        /// <summary>
        /// Returns true if the plane and the ray intersects, positionOnPlane contains the intersection point if it exists otherwise Vector3.zero.
        /// </summary>
        /// <param name="distance">maximnum distance of the ray.</param>
        /// <returns></returns>
        public static bool PositionOnPlane(Plane plane, Ray ray, out Vector3 positionOnPlane, float distance = float.PositiveInfinity)
        {
            float enter;
            if (plane.Raycast(ray, out enter))
            {
                if(enter < distance)
                {
                    positionOnPlane = ray.GetPoint(enter);

                    return true;
                }
            }

            positionOnPlane = Vector3.zero;
            return false;
        }

        /// <summary>
        /// Returns the intersect point between plane and ray if it exists, otherwise the rays origin point.
        /// </summary>
        /// <param name="distance">maximnum distance of the ray.</param>
        public static Vector3 PositionOnPlane(Plane plane, Ray ray, float distance = float.PositiveInfinity)
        {
            float enter;
            if (plane.Raycast(ray, out enter))
            {
                if (enter < distance)
                {
                    return ray.GetPoint(enter);
                }
            }

            return ray.origin;
        }
    }
}
