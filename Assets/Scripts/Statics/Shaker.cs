using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Statics {

    public class Shaker : MonoBehaviour {

        /// <summary>
        ///     This script shakes whatever transform is passed. Initially written to create a camera shake effect, but could in
        ///     theory be applied to any transform.
        /// </summary>
        public static IEnumerator Shake( Transform target, float duration, Vector2 magnitude ) {

            float elapsed = 0.0f;
            Vector3 originalPos = target.position;

            while( elapsed < duration ) {

                elapsed += Time.deltaTime;

                float percentComplete = elapsed / duration;
                float damper = 1.0f - Mathf.Clamp( 4.0f * percentComplete - 3.0f, 0.0f, 1.0f );

                // map noise to [-1, 1]
                float x = Random.value * 2.0f - 1.0f;
                float y = Random.value * 2.0f - 1.0f;
                x *= magnitude.x * damper;
                y *= magnitude.y * damper;

                target.position = new Vector3( x, y, originalPos.z );

                yield return null;
            }

            target.position = originalPos;
        }
    }
}