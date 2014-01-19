using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class AIPaddleControl : MonoBehaviour {

    public float moveSpeed = 10.0f;
    public float fakeInputAcceleration = 0.2f;
    public float fakeAxisLimit = 1.0f;
    private float fakeInputTarget = 0.0f;
    private float fakeVerticalInputAxis = 0.0f;
    private bool snapAxis = false;

    private Vector2 velocity = new Vector2();
    private Vector2 point;

    private BoxCollider2D boxCollider;
    private HumanPaddleControl humanScript;
    private bool useAI = false;

    private GameObject highestThreat;

    private List<Vector2> bouncePoints = new List<Vector2>();
    public int BOUNCE_COUNT_MAX = 3;
    public float EXTRA_COMPENSATION = 0.2f;
    private float COLLIDER_SHRINKAGE = 0.2f;

    private void Awake() {
        BoxCollider2D originalCollider = GetComponent<BoxCollider2D>();
        boxCollider = gameObject.AddComponent<BoxCollider2D>();

        // This code prevents exporting. Component-copying appearently has to be done manually as below. :(
        //ComponentUtility.CopyComponent( GetComponent<BoxCollider2D>() );
        //ComponentUtility.PasteComponentAsNew( gameObject );

        Vector2 newSize = originalCollider.size;
        newSize.y -= 2 * COLLIDER_SHRINKAGE;
        boxCollider.size = newSize;
        boxCollider.isTrigger = true;
        boxCollider.transform.position = originalCollider.transform.position;

        humanScript = GetComponent<HumanPaddleControl>();

        // TODO: Move this to a function, and replace it in update as well, to reduce calls to playerprefs
        bool ai = PlayerPrefs.GetInt( "AI", 1 ) == 1;
        humanScript.enabled = !ai;
        useAI = ai;
    }

    private void Start() {
        this.point = transform.position;
    }

    private void Update() {
        if( PlayerPrefs.GetInt( "AI", 1 ) == 1 ) {
            useAI = true;
            humanScript.enabled = false;
        } else {
            useAI = false;
            humanScript.enabled = true;
        }
        if( !useAI ) return;

        highestThreat = findHighestThreat();

        point.y = PredictBallY( highestThreat );

        if( boxCollider.OverlapPoint( point ) ) fakeInputTarget = 0.0f;
        else {
            float newTarget = transform.position.y > point.y ? -fakeAxisLimit : fakeAxisLimit;
            snapAxis = newTarget * fakeInputTarget <= 0;
            fakeInputTarget = newTarget;
        }
    }

    private float PredictBallY( GameObject ball ) {
        if( ball == null ) return 0.0f;
        Vector2 bp = ball.transform.position;
        if( bp.x < transform.position.x ) return 0.0f; // Only do calculations, if the ball is on the right side of the screen
        Vector2 bv = ball.rigidbody2D.velocity;
        return BounceRay( bp, bv, ball.GetComponent<CircleCollider2D>().radius );
    }

    private float BounceRay( Vector2 origin, Vector2 direction, float compensation ) {
        bouncePoints.Clear();
        float result = 0.0f;
        bool done = false;
        int count = 0;
        bouncePoints = new List<Vector2> {origin};
        while( !done && count < BOUNCE_COUNT_MAX ) {
            count++;
            RaycastHit2D hit = Physics2D.Raycast( origin, direction );
            bouncePoints.Add( hit.point );
            done = hit.transform.tag != "bounds";
            origin = hit.point;
            origin.y -= ( compensation + EXTRA_COMPENSATION + compensation ) * Mathf.Sign( origin.y );
            direction = Vector3.Reflect( direction, hit.normal );
        }
        if( bouncePoints.Count == 0 ) return result;
        result = bouncePoints[0].y;
        for( int i = 1; i < bouncePoints.Count; i++ ) {
            Debug.DrawLine( bouncePoints[i], bouncePoints[i - 1], Color.green );
            result = bouncePoints[i].y;
        }
        return result;
    }

    private void FixedUpdate() {
        if( !useAI ) return;
        if( snapAxis ) fakeVerticalInputAxis = 0; // Snap to zero if direction changed
        fakeVerticalInputAxis = Mathf.MoveTowards( fakeVerticalInputAxis, fakeInputTarget, fakeInputAcceleration * Time.deltaTime );
        float vSpeed = fakeVerticalInputAxis * moveSpeed;
        this.velocity.Set( rigidbody2D.velocity.x, vSpeed );
        rigidbody2D.velocity = velocity;
    }

    private GameObject findHighestThreat() {
        GameObject[] balls = GameObject.FindGameObjectsWithTag( "ball" );
        if( balls == null ) return null;

        GameObject highestThreat = null;
        float lowestVelocityX = 0;

        foreach( GameObject ball in balls ) {
            float ballVelocityX = ball.rigidbody2D.velocity.x;
            if( !( ballVelocityX < lowestVelocityX ) ) continue;
            lowestVelocityX = ballVelocityX;
            highestThreat = ball;
        }

        return highestThreat;
    }
}