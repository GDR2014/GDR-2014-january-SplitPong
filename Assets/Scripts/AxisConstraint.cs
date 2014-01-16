using UnityEngine;

namespace Assets.Scripts {
    /// <summary>
    ///     Locks the movement of a RigidBody2D to a single axis by using SliderJoint2D.
    /// </summary>
    [RequireComponent( typeof( SliderJoint2D ) )]
    public class AxisConstraint : MonoBehaviour {

        public enum Axis {
            X,
            Y
        }

        public Axis restrain = Axis.X;

        private void Start() {
            SliderJoint2D slider = GetComponent<SliderJoint2D>();
            Vector2 anchor = slider.connectedAnchor;
            switch( this.restrain ) {
                case Axis.X:
                    anchor.x = transform.position.x;
                    break;
                case Axis.Y:
                    anchor.y = transform.position.y;
                    break;
            }
            slider.connectedAnchor = anchor;
        }
    }
}