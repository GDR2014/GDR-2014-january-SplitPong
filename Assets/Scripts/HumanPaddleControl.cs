using UnityEngine;

namespace Assets.Scripts {

    /// <summary>
    ///     This script adds human control to a paddle.
    /// </summary>
    public class HumanPaddleControl : MonoBehaviour {

        public string verticalAxisName;
        public float moveSpeed = 10.0f;

        private Vector2 velocity;

        private void Start() {
            this.velocity = new Vector2();
        }

        private void FixedUpdate() {
            float vInput = Input.GetAxis( this.verticalAxisName );
            float vSpeed = vInput * this.moveSpeed;
            this.velocity.Set( rigidbody2D.velocity.x, vSpeed );
            rigidbody2D.velocity = this.velocity;
        }
    }
}