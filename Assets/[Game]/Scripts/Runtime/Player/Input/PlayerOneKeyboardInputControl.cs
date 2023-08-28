using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneKeyboardInputControl : MonoBehaviour
{
    #region Variables
    [SerializeField] bool isPlayerOne;
    private bool canMove = true;

    private bool canJump = true;
    private bool isJumping;
    [SerializeField] float moveForce; //5
    [SerializeField] float jumpPower; //110
    [SerializeField] float crouchPower; //50
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    const float JUMP_CHECK_RADIUS = .1f;
    #endregion
    #region Properties 
    private RagdollSelfControl ragdollSelfControl;
    private RagdollSelfControl RagdollSelfControl { get { return (ragdollSelfControl == null) ? ragdollSelfControl = GetComponent<RagdollSelfControl>() : ragdollSelfControl; } }
    #endregion
    #region MonoBehaviour Methods
    private void Update()
    {
        CheckJumping();
        if (isPlayerOne)
        {
            if (Input.GetKey(KeyCode.A))
            {
                RagdollSelfControl.ForceToBody(Vector3.back, moveForce);
            }
            if (Input.GetKey(KeyCode.D))
            {
                RagdollSelfControl.ForceToBody(Vector3.forward, moveForce);
            }
            if (Input.GetKey(KeyCode.S))
            {
                PlayerCrouch();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                PlayerJump();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                RagdollSelfControl.ForceToBody(Vector3.back, moveForce);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                RagdollSelfControl.ForceToBody(Vector3.forward, moveForce);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                PlayerCrouch();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PlayerJump();
            }
        }
    }
    #endregion
    #region My Methods
    public void PlayerJump()
    {
        if (canJump && !isJumping)
        {
            RagdollSelfControl.ForceToBody(Vector3.up, jumpPower);
            isJumping = true;
        }
    }
    public void PlayerCrouch()
    {
        RagdollSelfControl.ForceToBody(Vector3.down, crouchPower);
    }
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, JUMP_CHECK_RADIUS, groundLayer);
    }
    private void CheckJumping()
    {
        isJumping = !IsGrounded();
    }
    #endregion
}
