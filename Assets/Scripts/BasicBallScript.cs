using Assets.Scripts.Statics;
using UnityEngine;

namespace Assets.Scripts {

    /// <summary>
    ///     This script handles the ball behaviour.
    /// </summary>
    public class BasicBallScript : MonoBehaviour {

        public float resetThreshold = 0.1f;
        public float maxSpeed = 10.0f;
        public float speed = 5.0f;
        public float speedIncrement = 0.5f;
        public Vector2 minSpeed = new Vector2( 2.5f, 0.0f );

        public float screenShakeEffect = 0.05f;

        private AudioClip[] boundSounds;

        private Camera cam;
        private float camHalfWidth;

        private Vector2 ballVelocity;

        private void Awake() {
            this.cam = Camera.main;
            this.camHalfWidth = this.cam.orthographicSize * this.cam.aspect;

            this.boundSounds = new AudioClip[] {
                Resources.Load<AudioClip>( "Sounds/bounds1" ),
                Resources.Load<AudioClip>( "Sounds/bounds2" ),
                Resources.Load<AudioClip>( "Sounds/bounds3" ),
                Resources.Load<AudioClip>( "Sounds/bounds4" )
            };
        }

        private void Start() {
            Vector2 testVel = new Vector2( 5, 0 );
            rigidbody2D.velocity = testVel;
        }

        private void Update() {
            // Save a copy of the position and velocity for editing
            Vector2 pos = transform.position;
            Vector2 vel = rigidbody2D.velocity;

            ballVelocity = vel;
            // Make sure the ball is moving horizontally at all times
            if( Mathf.Abs( vel.x ) < this.minSpeed.x )
                vel.x = this.minSpeed.x * Mathf.Sign( vel.x );
            if( Mathf.Abs( vel.y ) < this.minSpeed.y )
                vel.y = this.minSpeed.y * Mathf.Sign( vel.y );

            // Move the ball to the other side of the screen, when we exit through the other
            if( pos.x < -( this.camHalfWidth + this.resetThreshold ) && vel.x < 0 ) {
                pos.x = this.camHalfWidth + this.resetThreshold;
            } else if( pos.x > ( this.camHalfWidth + this.resetThreshold ) && vel.x > 0 ) {
                pos.x = -( this.camHalfWidth + this.resetThreshold );
            }

            // ApplyToTextMesh the modified position and velocity
            transform.position = pos;
            rigidbody2D.velocity = vel.normalized * speed;
        }

        private void OnCollisionEnter2D( Collision2D c ) {
            // When a collision is detected, we reflect the velocity of the ball, making it bounce off whatever it hit
            // (This might already be handled by Unity, I'm not sure. It seems to work better with this code.)
            rigidbody2D.velocity = Vector3.Reflect( c.relativeVelocity, c.contacts[0].normal );
            switch( c.gameObject.tag ) {
                case "paddle": // If we hit a paddle...
                    if( c.gameObject.audio != null ) // ...and the paddle has a sound...
                    {
                        c.gameObject.audio.volume = PlayerPrefs.GetInt( "Volume", 50 ) / 100.0f;
                        c.gameObject.audio.Play(); // ...play that sound.
                    }
                    if( this.speed < this.maxSpeed ) // If the current speed is less than the maximum speed...
                        this.speed += this.speedIncrement; // ...increase the speed of the ball.

                    // Run awesome screen shake effect. :D
                    Vector2 shakeMagnitude = c.relativeVelocity.normalized * this.screenShakeEffect;
                    StartCoroutine( Shaker.Shake( Camera.main.transform, .25f, shakeMagnitude ) );

                    break;
                case "bounds":
                    // Play a random bound sound if we collide with the roof/floor bounds.
                    int sndIdx = Random.Range( 0, this.boundSounds.Length );
                    audio.clip = this.boundSounds[sndIdx];
                    audio.volume = PlayerPrefs.GetInt( "Volume", 50 ) / 100.0f;
                    audio.Play();
                    break;
            }

            // Make sure the speed doesn't exceed the maximum
            if( this.speed > this.maxSpeed )
                this.speed = this.maxSpeed;
        }
    }
}