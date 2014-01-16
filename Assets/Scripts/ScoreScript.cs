using Assets.Scripts;
using Assets.Scripts.Statics;
using UnityEngine;

/// <summary>
///     This script handles the score event, fired by the trigger-zone between the paddles.
///     It handles the score count for the players as needed, as well as playing sounds and effects on the text meshes for
///     the score values.
/// </summary>
public class ScoreScript : MonoBehaviour {
    public GameControlScript gc;

    private AudioClip sndP1Score;
    private AudioClip sndP2Score;

    public GameObject p1ScoreObject;
    public GameObject p2ScoreObject;

    public Vector3 SoundEmmisionPoint = new Vector3( 0, 0, 0 );

    public float flashFinishThreshold = 0.02f;
    public float flashedSizeMultiplier = 1.5f;
    public float flashSpeed = 0.001f;

    private void Awake() {
        sndP1Score = Resources.Load<AudioClip>( "Sounds/score2" );
        sndP2Score = Resources.Load<AudioClip>( "Sounds/score1" );
    }

    private void OnTriggerExit2D( Collider2D c ) {
        bool movingRight = c.gameObject.rigidbody2D.velocity.x > 0;
        float audioVolume = PlayerPrefs.GetInt( "Volume", 50 ) / 100.0f;

        if( movingRight ) {
            ScoreManager.p2Score++;
            p2ScoreObject.GetComponent<TextMesh>().text = ScoreManager.p2Score.ToString();
            StartCoroutine( ZoomAndFade.ApplyToTextMesh( p2ScoreObject, flashedSizeMultiplier, flashSpeed, flashFinishThreshold, true, true ) );
            AudioSource.PlayClipAtPoint( sndP2Score, SoundEmmisionPoint, audioVolume );
        } else {
            ScoreManager.p1Score++;
            p1ScoreObject.GetComponent<TextMesh>().text = ScoreManager.p1Score.ToString();
            StartCoroutine( ZoomAndFade.ApplyToTextMesh( p1ScoreObject, flashedSizeMultiplier, flashSpeed, flashFinishThreshold, true, true ) );
            AudioSource.PlayClipAtPoint( sndP1Score, SoundEmmisionPoint, audioVolume );
        }
        gc.CheckScore();
    }
}