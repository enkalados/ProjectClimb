using UnityEngine;
using Player.InputControl;
using Player.AnimationsControl;

namespace Player.MovementControl
{
    [RequireComponent(typeof(PlayerInputControl))]
    [RequireComponent(typeof(PlayerAnimationsControl))]
    public class PlayerMovementControl : MonoBehaviour
    {
        #region Variables
        private bool canMove = true;
        [SerializeField] float moveSpeed;
        private Vector3 moveVector = Vector3.zero;

        private bool canJump = true;
        private bool isJumping;
        [SerializeField] float jumpPower;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] Transform groundCheck;
        const float JUMP_CHECK_RADIUS = .1f;
        const float AIR_CONTROL_FACTOR = .5f;

        #endregion
        #region Properties
        private Rigidbody rigidbody;
        private Rigidbody Rigidbody { get { return (rigidbody == null) ? rigidbody = GetComponent<Rigidbody>() : rigidbody; } }
        private PlayerAnimationsControl animationsControl;
        private PlayerAnimationsControl AnimationsControl { get { return (animationsControl == null) ? animationsControl = GetComponent<PlayerAnimationsControl>() : animationsControl; } }
        #endregion
        #region MonoBehaviour Methods
        private void FixedUpdate()
        {
            PlayerMove();
            CheckJumping();
            Debug.Log("ground " + IsGrounded());
        }
        #endregion
        #region My Methods
        public void SetMoveVector(Vector2 moveVector)
        {
            if (canMove)
            {
                this.moveVector.x = moveVector.normalized.x;
            }
        }
        private void PlayerMove()
        {
            Vector3 movement = new Vector3(moveVector.x, 0f, 0f) * moveSpeed * Time.fixedDeltaTime;
            Rigidbody.MovePosition(Rigidbody.position + movement);
            SetPlayerDirection(moveVector.x);
        }
        private void SetPlayerDirection(float x)
        {
            AnimationsControl.SetPlayerDirection(x);
        }
        public void PlayerJump()
        {
            if (canJump && IsGrounded())
            {
                Rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                isJumping = true;
            }
        }
        private void CheckJumping()
        {
            isJumping = !IsGrounded();
        }
        private bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, JUMP_CHECK_RADIUS, groundLayer);
        }
        #endregion
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, JUMP_CHECK_RADIUS);
        }
    }
}