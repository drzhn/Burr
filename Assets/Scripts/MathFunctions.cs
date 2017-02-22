using UnityEngine;
using System.Collections;

namespace MathFunctions
{
    public class MathFunctions
    {
        public MathFunctions() { }
        public float Angle(Vector3 from, Vector3 to, Vector3 axis)
        {
            axis = axis.normalized;
            if (Vector3.Cross(from, to).normalized == axis)
                return Vector3.Angle(from, to);
            else
                return -Vector3.Angle(from, to);
        }
        public Vector3 RandomNormal(Vector3 normal)
        {
            if (normal.x != 0 && normal.y != 0 && normal.z != 0)
            {
                float x = Random.Range(0.1f, 1f);
                float y = Random.Range(0.1f, 1f);
                float z = (normal.x * x + normal.y * y) / (-1 * normal.z);
                return new Vector3(x, y, z);
            }
            else
            {
                float x, y, z;
                if (normal.x == 0)
                    x = Random.Range(0.1f, 1f);
                else x = 0;
                if (normal.y == 0)
                    y = Random.Range(0.1f, 1f);
                else y = 0;
                if (normal.z == 0)
                    z = Random.Range(0.1f, 1f);
                else z = 0;
                return new Vector3(x, y, z);
            }
        }
        public Vector3 RayCrossPlane(Ray ray, Vector3 originPlane, Vector3 normalPlane)
        {
            Vector3 originRay = ray.origin;
            Vector3 directionRay = ray.direction;
            return originRay + directionRay * (-Vector3.Dot(originRay - originPlane, normalPlane) / Vector3.Dot(directionRay, normalPlane));
        }

        public Vector3 RoundVector(Vector3 v)
        {
            return new Vector3(
                x: Mathf.Round(v.x),
                y: Mathf.Round(v.y),
                z: Mathf.Round(v.z));
        }
        /// <summary>
        /// преобрзует вектор направления движения пальца по экрану
        /// в одно из шести направлений в 3D
        /// </summary>
        /// <param name="CamTransform">Transform камеры</param>
        /// <param name="cursorDirection">Разница между конечным и начальным положениями курсора мыши или пальца</param>
        /// <returns></returns>
        public Vector3 CursorDirectionTo3DDirection(Transform CamTransform, Vector3 cursorDirection)
        {
            Vector3[] vectorProjects = {
                                                    CamTransform.InverseTransformVector(Vector3.ProjectOnPlane(Vector3.up, Vector3.zero - CamTransform.position)),
                                                    CamTransform.InverseTransformVector(Vector3.ProjectOnPlane(Vector3.down, Vector3.zero - CamTransform.position)),
                                                    CamTransform.InverseTransformVector(Vector3.ProjectOnPlane(Vector3.forward, Vector3.zero - CamTransform.position)),
                                                    CamTransform.InverseTransformVector(Vector3.ProjectOnPlane(Vector3.back, Vector3.zero - CamTransform.position)),
                                                    CamTransform.InverseTransformVector(Vector3.ProjectOnPlane(Vector3.right, Vector3.zero - CamTransform.position)),
                                                    CamTransform.InverseTransformVector(Vector3.ProjectOnPlane(Vector3.left, Vector3.zero - CamTransform.position))};
            int minIndex = 0;
            float minAngle = Vector3.Angle(vectorProjects[0], cursorDirection);
            for (int i = 1; i < 6; i++) //перемещаться можно в шесть сторон
            {
                float angle = Vector3.Angle(vectorProjects[i], cursorDirection);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    minIndex = i;
                }
            }
            Vector3 result = Vector3.zero;
            switch (minIndex)
            {
                case 0:
                    result = Vector3.up;
                    break;
                case 1:
                    result = Vector3.down;
                    break;
                case 2:
                    result = Vector3.forward;
                    break;
                case 3:
                    result = Vector3.back;
                    break;
                case 4:
                    result = Vector3.right;
                    break;
                case 5:
                    result = Vector3.left;
                    break;
            }
            return result;
        }
    }

}