using UnityEngine;

namespace Assets.Scripts {

    /// <summary>
    ///     This class just adds some very basic pausing and resetting functionality to the game.
    /// </summary>
    public class DebugScript : MonoBehaviour {

        private bool paused = false;

        private void Update() {
            if( Input.GetKeyUp( KeyCode.R ) )
                Application.LoadLevel( 0 );     // Reload the scene
            if( Input.GetKeyUp( KeyCode.P ) )
                this.paused = !this.paused;

            Time.timeScale = this.paused ? 0 : 1;
        }
    }
}