using UnityEngine;

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
    float jumpCheckRadius = .1f;
    float airControlFactor = 0.5f;
    #endregion
    #region Properties
    private new Rigidbody rigidbody;
    private Rigidbody Rigidbody { get { return (rigidbody == null) ? rigidbody = GetComponent<Rigidbody>() : rigidbody; } }
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
            this.moveVector.z = moveVector.normalized.y;
        }
    }
    private void PlayerMove()
    {
        //Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.z) * moveSpeed * Time.fixedDeltaTime;
        //Rigidbody.MovePosition(Rigidbody.position + movement);
        if (!isJumping)
        {
            Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.z) * moveSpeed * Time.fixedDeltaTime;
            Rigidbody.MovePosition(Rigidbody.position + movement);
            Debug.Log("a");
        }
        else
        {
            Rigidbody.AddForce(moveVector * moveSpeed * Time.fixedDeltaTime);
            Vector3 movement = new Vector3(moveVector.x * airControlFactor, 0f, moveVector.z * airControlFactor) * moveSpeed / 2 * Time.fixedDeltaTime;
            Rigidbody.MovePosition(Rigidbody.position + movement);
            Debug.Log("b");
        }
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
        return Physics.CheckSphere(groundCheck.position, jumpCheckRadius, groundLayer);
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, jumpCheckRadius);
    }
}
