using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerupControllerScript : MonoBehaviour {

    public GameObject[] PossiblePowerups;
    public float spawnChance = 0.75f;
    public float powerupInterval = 10.0f;

    public Dictionary<String, BasicPowerUp> ManagedPowerups = new Dictionary<string, BasicPowerUp>();

    public float firstPossiblePowerupTime = 5.0f;
    public float nextPossiblePowerupTime;

    private BoxCollider2D bc1, bc2;

    private void Awake() {
        GameObject validSpawnAreas = GameObject.Find( "PowerUpController/ValidSpawnAreas" );
        BoxCollider2D[] bcs = validSpawnAreas.GetComponents<BoxCollider2D>();
        bc1 = bcs[0];
        bc2 = bcs[1];

        foreach( GameObject powerup in PossiblePowerups ) {
            BasicPowerUp powerupScript = powerup.GetComponent<BasicPowerUp>();
            powerupScript.controller = this;
        }
        nextPossiblePowerupTime = Time.time + firstPossiblePowerupTime;
    }

    private void Update() {
        if( PossiblePowerups.Length > 0 && Time.time >= nextPossiblePowerupTime )
            TrySpawnPowerup();
    }

    public void TrySpawnPowerup() {
        Debug.Log( "Attempting to spawn powerup..." );
        if( Random.value > spawnChance ) {
            nextPossiblePowerupTime += powerupInterval / 4.0f;
            Debug.Log( "Spawn failed. Trying again at " + nextPossiblePowerupTime );
            return;
        }
        float side = Random.value;
        print( "Spawn successful. side=" + side );
        if( side <= 0.5f ) DoSpawn( bc1 );
        else DoSpawn( bc2 );
        nextPossiblePowerupTime += powerupInterval;
    }

    private void DoSpawn( BoxCollider2D spawnArea ) {
        float halfWidth = spawnArea.size.x / 2;
        float halfHeight = spawnArea.size.y / 2;

        float x = Random.Range( -halfWidth, halfWidth ) + spawnArea.center.x;
        float y = Random.Range( -halfHeight, halfHeight ) + spawnArea.center.y;
        print( String.Format( "Spawning at ({0}, {1})", x, y ) );
        int powerupNumber = Random.Range( 0, PossiblePowerups.Length );
        Instantiate( PossiblePowerups[powerupNumber], new Vector2( x, y ), Quaternion.identity );
    }
}