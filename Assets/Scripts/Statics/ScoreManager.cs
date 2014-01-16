using UnityEngine;

namespace Assets.Scripts.Statics {
    /// <summary>
    ///     This is just a place to put the players' scores.
    /// </summary>
    public class ScoreManager : MonoBehaviour {

        public static int p1Score = 0;
        public static int p2Score = 0;

        private void Start() {
            p1Score = 0;
            p2Score = 0;
        }
    }
}