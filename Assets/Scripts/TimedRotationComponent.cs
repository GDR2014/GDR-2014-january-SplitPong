using System.Collections;
using UnityEngine;

namespace Assets.Scripts {

    public class TimedRotationComponent : MonoBehaviour {

        public float RemainingDuration { get; set; }
        private float rotationTime = 0.75f;
        private bool readyToDestroy = false;

        public IEnumerator Rotate( Quaternion rotation, float duration, float rotationSpeed ) {
            RemainingDuration = duration;
            rotationTime = rotationSpeed;
            Quaternion startRotation = transform.rotation;
            StartCoroutine( "RotateTo", rotation );
            while( RemainingDuration > 0.0f ) {
                RemainingDuration -= Time.deltaTime;
                yield return null;
            }
            StopCoroutine( "RotateTo" );
            readyToDestroy = true;
            StartCoroutine( "RotateTo", startRotation );
        }

        public void ActivateComponent( Quaternion rotation, float duration, float rotationSpeed = 0.75f ) {
            StartCoroutine( Rotate( rotation, duration, rotationSpeed ) );
        }

        private IEnumerator RotateTo( Quaternion endRotation ) {
            print( "RotateTo coroutine started!" );
            while( Vector3.Distance( transform.rotation.eulerAngles, endRotation.eulerAngles ) > 0.02f ) {
                print( "Vector distance = " + Vector3.Distance( transform.rotation.eulerAngles, endRotation.eulerAngles ) );
                transform.rotation = Quaternion.Lerp( transform.rotation, endRotation, rotationTime * Time.time );
                yield return null;
            }
            if( !this.readyToDestroy ) yield break;
            print( "Rotate to is ready to destroy. Destroying..." );
            Destroy( this );
        }
    }

}