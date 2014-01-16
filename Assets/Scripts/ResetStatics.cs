using Assets.Scripts.Statics;
using UnityEngine;

/// <summary>
///     This script is just a dirty workaround for resetting static values when a scene loads.
///     It should be attached to a gameObject in order to work.
/// </summary>
public class ResetStatics : MonoBehaviour {

    private void Start() {
        ScoreManager.p1Score = 0;
        ScoreManager.p2Score = 0;
    }

}