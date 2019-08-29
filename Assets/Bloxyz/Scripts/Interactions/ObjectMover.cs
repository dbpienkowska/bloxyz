using UnityEngine;
using System.Collections.Generic;
using MEC;

namespace Bloxyz
{
    public class ObjectMover : MonoBehaviour
    {
        public static void MoveInstantly(Transform target, Vector3 translation)
        {
            if(target.gameObject.activeSelf)
                target.Translate(translation, Space.World);
        }

        public static void Move(Transform target, Vector3 translation, float duration)
        {
            if(target.gameObject.activeSelf)
                Timing.RunCoroutine(_MoveCoroutine(target, translation, duration));
        }

        private static IEnumerator<float> _MoveCoroutine(Transform target, Vector3 translation, float duration)
        {
            float time = 0;
            Vector3 targetPosition = target.transform.position + translation;

            while(time < duration)
            {
                target.transform.Translate(translation * Time.deltaTime / duration, Space.World);
                time += Time.deltaTime;
                yield return 0f;
            }

            target.transform.position = targetPosition;
        }
    }
}

