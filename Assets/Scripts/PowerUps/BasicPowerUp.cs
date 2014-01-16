using Assets.Scripts.Statics;
using UnityEngine;

public class BasicPowerUp : MonoBehaviour {

    public PowerupControllerScript controller;
    public Sprite powerupSprite;

    // ---- Effect fields
    public float FADE_TO_SCALE = 5.0f;
    public float FADE_SPEED = 0.1f;
    public float FADE_FINISHED_THRESHOLD = 0.02f;

    protected virtual void ApplyPowerUp( GameObject ball ) {}

    private void Awake() {
        if( powerupSprite != null )
            GetComponent<SpriteRenderer>().sprite = powerupSprite;
        if( controller == null ) throw new MissingComponentException( "Specified powerup controller does not have a powerup controller script attached!" );
    }

    private void OnTriggerEnter2D( Collider2D collider ) {
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false; // Disable the collider, so that we don't run this code more than once.
        if( audio != null ) audio.Play();
        StartCoroutine( ZoomAndFade.ApplyToSprite( gameObject, FADE_TO_SCALE, FADE_SPEED, FADE_FINISHED_THRESHOLD, false, true ) );
        ApplyPowerUp( collider.gameObject );
    }
}