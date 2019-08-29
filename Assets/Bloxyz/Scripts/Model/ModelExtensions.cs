using UnityEngine;

namespace Bloxyz
{
    public static class ModelExtensions
    {
        public static void RoundPosition(this Block block)
        {
            int x = Mathf.RoundToInt(block.transform.position.x);
            int y = Mathf.RoundToInt(block.transform.position.y);
            int z = Mathf.RoundToInt(block.transform.position.z);

            block.transform.ChangePosition(x, y, z);
        }

        public static void RoundPositionY(this Level level)
        {
            int y = Mathf.RoundToInt(level.transform.localPosition.y);
            level.transform.ChangeLocalPosition(y: y);
        }

        public static void ChangePosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            Vector3 position = transform.position;

            position.x = x ?? position.x;
            position.y = y ?? position.y;
            position.z = z ?? position.z;

            transform.position = position;
        }

        public static void ChangeLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            Vector3 position = transform.localPosition;

            position.x = x ?? position.x;
            position.y = y ?? position.y;
            position.z = z ?? position.z;

            transform.localPosition = position;
        }
    }
}

