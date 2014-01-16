using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Statics {

    public class DestroyTimer : MonoBehaviour {

        public Object ObjectToDestroy;
        public float TimeToLive = 15.0f;

        void Awake() {
            ObjectToDestroy = gameObject;
        }

        public static IEnumerator DestroyInSeconds( Object objectToDestroy, float seconds ) {
            yield return new WaitForSeconds( seconds );
            Object.Destroy( objectToDestroy );
        }

        public void StartCountdown() {
            if( ObjectToDestroy == null ) throw new NullReferenceException( "Object to destroy is null!" );
            if( TimeToLive < 0.0f ) throw new ArgumentOutOfRangeException("TimeToLive", "Time to Live must be greater than 0!");
            StartCoroutine( DestroyInSeconds( ObjectToDestroy, TimeToLive ) );
        }
    }

}