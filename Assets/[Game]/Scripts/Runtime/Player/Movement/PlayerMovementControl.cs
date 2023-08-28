using UnityEngine;
using Player.InputControl;
using Player.AnimationsControl;

namespace Player.MovementControl
{
    [RequireComponent(typeof(PlayerInputControl))]
    public class PlayerMovementControl : MonoBehaviour
    {
        #region Variables
        private bool canMove = true;
        [SerializeField] float moveSpeed;
        private Vector3 moveVector = Vector3.zero;

        private bool canJump = true;
        private bool isJumping;
        [SerializeField] float moveForce;
        [SerializeField] float jumpPower;
        [SerializeField] float crouchPower;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] Transform groundCheck;
        const float JUMP_CHECK_RADIUS = .1f;
        #endregion
        #region Properties
        private PlayerAnimationsControl animationsControl;
        private PlayerAnimationsControl AnimationsControl { get { return (animationsControl == null) ? animationsControl = GetComponent<PlayerAnimationsControl>() : animationsControl; } }
        private RagdollSelfControl ragdollSelfControl;
        private RagdollSelfControl RagdollSelfControl { get { return (ragdollSelfControl == null) ? ragdollSelfControl = GetComponent<RagdollSelfControl>() : ragdollSelfControl; } }
        #endregion
        #region MonoBehaviour Methods
        private void FixedUpdate()
        {
            PlayerMove();
            CheckJumping();
        }
        #endregion
        #region My Methods
        public void SetMoveVector(Vector2 moveVector)
        {
            if (canMove)
            {
                this.moveVector = moveVector.normalized;
            }
        }
        private void PlayerMove()
        {
            if (moveVector.x>0)
            {
                RagdollSelfControl.ForceToBody(Vector3.forward, moveForce);
            }
            else if (moveVector.x<0)
            {
                RagdollSelfControl.ForceToBody(Vector3.back, moveForce);
            }
            if (moveVector.y<0)
            {
                PlayerCrouch();
            }
        }
        public void PlayerJump()
        {
            if (canJump && IsGrounded())
            {
                RagdollSelfControl.ForceToBody(Vector3.up, jumpPower);
                isJumping = true;
            }
        }
        public void PlayerCrouch()
        {
            RagdollSelfControl.ForceToBody(Vector3.down, crouchPower);
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