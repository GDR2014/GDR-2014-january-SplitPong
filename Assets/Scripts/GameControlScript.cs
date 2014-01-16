using Assets.Scripts.Statics;
using UnityEngine;

namespace Assets.Scripts {
    public class GameControlScript : MonoBehaviour {

        public enum GameState {
            PLAYING,
            MENU,
            P1WIN,
            P2WIN,
            DRAW
        }

        public GameState state = GameState.PLAYING;
        public bool paused = false;

        public int ScoreToWin;

        private void Start() {
            state = GameState.MENU;
            paused = true;
            ScoreToWin = PlayerPrefs.GetInt( "ScoreToWin", 25 );
        }

        private void Update() {
            if( state == GameState.PLAYING ) CheckScore();

            if( paused && state == GameState.PLAYING ) {
                if( Input.anyKeyDown ) paused = false;
            } else if( Input.GetButtonDown( "Pause" ) ) {
                switch( state ) {
                    case GameState.PLAYING:
                        paused = !paused;
                        break;
                    case GameState.MENU:
                        break;
                }
            }

            if( Input.GetButtonDown( "Menu" ) ) {
                switch( state ) {
                    case GameState.PLAYING:
                        state = GameState.MENU;
                        paused = true;
                        break;
                    case GameState.MENU:
                        state = GameState.PLAYING;
                        break;
                }
            }

            if( Input.GetButtonDown( "Reset" ) ) {
                switch( state ) {
                    case GameState.PLAYING:
                        ResetGame();
                        break;
                    case GameState.MENU:
                        break;
                }
            }
            if( state == GameState.P1WIN || state == GameState.P2WIN )
                paused = true;

            Time.timeScale = paused ? 0.0f : 1.0f;
        }

        public void ResetGame() {
            foreach( GameObject powerup in GameObject.FindGameObjectsWithTag( "powerup" ) )
                Destroy( powerup );
            Application.LoadLevel( 0 );
        }

        public void CheckScore() {
            if( ScoreManager.p1Score >= ScoreToWin && ScoreManager.p1Score > ScoreManager.p2Score ) {
                state = GameState.P1WIN;
            } else if( ScoreManager.p2Score >= ScoreToWin && ScoreManager.p2Score > ScoreManager.p1Score ) {
                state = GameState.P2WIN;
            } else if( ScoreManager.p1Score >= ScoreToWin && ScoreManager.p1Score == ScoreManager.p2Score ) {
                state = GameState.DRAW;
            }
        }
    }
}