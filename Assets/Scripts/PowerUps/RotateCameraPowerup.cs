using Assets.Scripts;
using UnityEngine;

public class RotateCameraPowerup : BasicPowerUp {

    public float rotationDuration = 5.0f;

    protected override void ApplyPowerUp( GameObject ball ) {
        Camera mainCam = Camera.main;
        TimedRotationComponent trc = mainCam.GetComponent<TimedRotationComponent>();
        if( trc != null ) {
            trc.RemainingDuration = rotationDuration;
            return;
        }
        trc = mainCam.gameObject.AddComponent<TimedRotationComponent>();
        trc.RemainingDuration = rotationDuration;
        trc.ActivateComponent(Quaternion.Euler( 0, 0, 180 ), rotationDuration, 5f);
    }
}