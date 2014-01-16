using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Statics {

    public class ZoomAndFade {

        /// <summary>
        ///     This script scales and fades a gameobject with a TextMesh.
        /// </summary>
        public static IEnumerator ApplyToTextMesh( GameObject g, float scaleMultiplier, float speed, float finishedThreshold, bool createCopy, bool autoDestroy ) {
            Transform oldTransform = g.transform;
            if( createCopy ) g = Object.Instantiate( g ) as GameObject;
            g.transform.parent = oldTransform.parent;
            g.transform.position = oldTransform.position;
            g.transform.localScale = oldTransform.localScale;
            g.transform.rotation = oldTransform.rotation;

            TextMesh mesh = g.GetComponent<TextMesh>();

            float oldZ = g.transform.localScale.z;
            Vector3 newScale = g.transform.localScale * scaleMultiplier;
            newScale.z = oldZ;

            Color c = mesh.color;
            while( mesh.color.a > finishedThreshold ) {
                c = mesh.color;
                c.a = Mathf.Lerp( c.a, 0, speed );
                mesh.color = c;

                Vector3 t = g.transform.localScale;
                t = Vector3.Lerp( t, newScale, speed );
                g.transform.localScale = t;

                yield return null;
            }

            if( autoDestroy ) Object.Destroy( g );
        }

        public static IEnumerator ApplyToSprite( GameObject g, float scaleMultiplier, float speed, float finishedThreshold, bool createCopy, bool autoDestroy ) {
            Transform oldTransform = g.transform;
            if( createCopy ) g = Object.Instantiate( g ) as GameObject;
            g.transform.parent = oldTransform.parent;
            g.transform.position = oldTransform.position;
            g.transform.localScale = oldTransform.localScale;
            g.transform.rotation = oldTransform.rotation;

            SpriteRenderer sprite = g.GetComponent<SpriteRenderer>();

            float oldZ = g.transform.localScale.z;
            Vector3 newScale = g.transform.localScale * scaleMultiplier;
            newScale.z = oldZ;

            Color c = sprite.color;
            while( sprite.color.a > finishedThreshold ) {
                c = sprite.color;
                c.a = Mathf.Lerp( c.a, 0, speed );
                sprite.color = c;

                Vector3 t = g.transform.localScale;
                t = Vector3.Lerp( t, newScale, speed );
                g.transform.localScale = t;

                yield return null;
            }

            if( autoDestroy ) Object.Destroy( g );
        }
    }
}