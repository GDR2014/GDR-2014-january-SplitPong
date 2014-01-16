using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts {
    public class GuiScript : MonoBehaviour {

        public GameObject gameController;
        private GameControlScript gc;

        public Rect menuRect = new Rect( 0.0f, 0.0f, 300.0f, 900.0f );
        public Rect contentRect = new Rect( 0.0f, 100.0f, 460.0f, 340.0f );

        private Rect r; // Reusable rect.

        private Matrix4x4 guiMatrix;

        public float nativeWidth = 960.0f;
        public float nativeHeight = 384.0f;

        public GUISkin skin;

        private String volumeInput;
        private int volumeSlider;

        private String scoreToWinInput;

        private void Awake() {
            r = new Rect();
            gc = gameController.GetComponent<GameControlScript>();
            volumeSlider = PlayerPrefs.GetInt( "Volume", 50 );
            volumeInput = volumeSlider.ToString();
            scoreToWinInput = PlayerPrefs.GetInt( "ScoreToWin", 25 ).ToString();
        }

        private void OnGUI() {
            GUI.skin = skin;

            float relativeWidth = Screen.width / nativeWidth;
            float relativeHeight = Screen.height / nativeHeight;
            guiMatrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( relativeWidth, relativeHeight, 1.0f ) );
            GUI.matrix = guiMatrix;

            switch( gc.state ) {

                case GameControlScript.GameState.PLAYING:
                    if( gc.paused ) drawPauseGUI();
                    break;
                case GameControlScript.GameState.MENU:
                    drawMenuGUI();
                    break;
                case GameControlScript.GameState.P1WIN:
                    drawGameOverGUI( "Player 1 wins!" );
                    break;
                case GameControlScript.GameState.P2WIN:
                    drawGameOverGUI( "Player 2 wins!" );
                    break;
                case GameControlScript.GameState.DRAW:
                    drawGameOverGUI( "It's a draw!" );
                    break;
            }
        }

        private void drawPauseGUI() {
            r.Set( nativeWidth / 2.0f - 50.0f, 100.0f, 300.0f, 50.0f );
            GUI.Label( r, "Press any key to unpause" );
        }

        private void drawMenuGUI() {
            GUI.skin = skin;
            float leftSide = nativeWidth / 2.0f - menuRect.width / 2 + menuRect.x;
            float topSide = nativeHeight / 2.0f - menuRect.height / 2 + menuRect.y;

            float contentLeft = nativeWidth / 2.0f - contentRect.width / 2 + contentRect.x;
            float contentTop = nativeHeight / 2.0f - contentRect.height / 2 + contentRect.y;

            r.Set( leftSide, topSide, menuRect.width, menuRect.height );
            GUI.Box( r, "" );
            GUI.Box( r, "Menu" ); // Double-up on the background to increase opacity.

            r.Set( contentLeft, contentTop, contentRect.width, contentRect.height );
            GUILayout.BeginArea( r );

            // Reset button. Nice and easy.
            if( GUILayout.Button( "Reset" ) )
                gc.ResetGame();

            // Score limit
            GUILayout.BeginHorizontal();
            GUILayout.Label( "Score needed to win:" );
            scoreToWinInput = GUILayout.TextField( scoreToWinInput );
            GUILayout.EndHorizontal();
            scoreToWinInput = Regex.Replace( scoreToWinInput, "[^0-9]", "" );
            int newScoreToWin;
            bool canParse = Int32.TryParse( scoreToWinInput, out newScoreToWin );
            if( canParse && ( gc.ScoreToWin != newScoreToWin ) ) {
                gc.ScoreToWin = newScoreToWin;
                PlayerPrefs.SetInt( "ScoreToWin", newScoreToWin );
            }

            // AI toggle
            bool useAI = PlayerPrefs.GetInt( "AI" ) == 1;
            PlayerPrefs.SetInt( "AI", GUILayout.Toggle( useAI, "Use AI for P2" ) ? 1 : 0 );


            // Volume controls from hell! :O
            GUILayout.BeginHorizontal();
            GUILayout.Label( "Volume: " );
            int volume = PlayerPrefs.GetInt( "Volume", 50 );
            volumeSlider = Mathf.RoundToInt( GUILayout.HorizontalSlider( volumeSlider, 0, 100, GUILayout.Width( 75.0f ) ) );

            volumeInput = GUILayout.TextField( volumeInput, 3, GUILayout.Width( 40.0f ) );
            volumeInput = Regex.Replace( volumeInput, "[^0-9]", "" );

            int newVolume;
            canParse = Int32.TryParse( volumeInput, out newVolume );
            if( canParse ) newVolume = Mathf.Min( newVolume, 100 );

            volume = volumeSlider != volume ? volumeSlider : newVolume;

            volumeInput = volume.ToString();
            volumeSlider = volume;

            PlayerPrefs.SetInt( "Volume", volume );

            GUILayout.EndHorizontal();


            // Resume button
            if( GUILayout.Button( "Resume" ) )
                gc.state = GameControlScript.GameState.PLAYING;

            GUILayout.EndArea();
        }

        private void drawGameOverGUI( String title ) {
            r.Set( nativeWidth / 2 - 280 / 2, nativeHeight / 2 - 80 / 2, 280, 100 );
            GUI.Box( r, title );
            r.Set( nativeWidth / 2 - 100 / 2, nativeHeight / 2, 100, 40 );
            if( GUI.Button( r, "Restart" ) )
                gc.ResetGame();
        }
    }
}