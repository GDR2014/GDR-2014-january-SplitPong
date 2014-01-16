using System.Collections;
using UnityEngine;

public class IncreaseSpawnratePowerup : BasicPowerUp {

    public float ResetDuration = 10.0f;
    public float RemainingDuration { get; set; }

    private void Start() {
        RemainingDuration = ResetDuration;
    }

    protected override void ApplyPowerUp( GameObject ball ) {
        BasicPowerUp currentPowerup;
        bool exists = controller.ManagedPowerups.TryGetValue( this.GetType().ToString(), out currentPowerup );
        if( exists ) {
            ( (IncreaseSpawnratePowerup) currentPowerup ).RemainingDuration = ResetDuration;
            return;
        }
        controller.ManagedPowerups.Add( this.GetType().ToString(), this );
        controller.StartCoroutine( IncreaseSpawnRate() );
    }

    private IEnumerator IncreaseSpawnRate() {
        float oldInterval = controller.powerupInterval;
        float oldChance = controller.spawnChance;

        controller.spawnChance = 1.0f;
        controller.powerupInterval = oldInterval / 5.0f;
        controller.nextPossiblePowerupTime = Time.time + 0.5f;

        while( RemainingDuration > 0 ) {
            RemainingDuration -= Time.deltaTime;
            yield return null;
        }

        controller.spawnChance = oldChance;
        controller.powerupInterval = oldInterval;
        controller.ManagedPowerups.Remove( this.GetType().ToString() );
    }
}