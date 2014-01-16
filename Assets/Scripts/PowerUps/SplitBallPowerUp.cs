using System.Collections;
using Assets.Scripts.Statics;
using UnityEngine;

public class SplitBallPowerUp : BasicPowerUp {

    public float SplitLifetime = 15.0f;

    protected override void ApplyPowerUp( GameObject ball ) {
        SpawnTimedBall(ball, SplitLifetime);
    }

    protected void SpawnTimedBall( GameObject originalBall, float timeToLive ) {
        GameObject ball2 = Instantiate( originalBall ) as GameObject;
        Vector2 ball2Velocity = originalBall.rigidbody2D.velocity;
        ball2Velocity.x *= -1;
        ball2.rigidbody2D.velocity = ball2Velocity;
        ball2.transform.position = transform.position;  // Move the ball out of the other ball. :P
        ball2.AddComponent<DestroyTimer>();
        DestroyTimer timer = ball2.GetComponent<DestroyTimer>();
        timer.StartCountdown();
    }
}