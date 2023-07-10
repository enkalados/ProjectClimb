using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
    #region Variables
    private bool canMove = true;
    [SerializeField] float moveSpeed;
    private Vector3 moveVector = Vector3.zero;

    private bool canJump = true;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    float jumpCheckRadius = .1f;
    #endregion
    #region Properties
    private new Rigidbody rigidbody;
    private Rigidbody Rigidbody { get { return (rigidbody == null) ? rigidbody = GetComponent<Rigidbody>() : rigidbody; } }
    #endregion
    #region MonoBehaviour Methods

    #endregion
    #region My Methods
    public void SetMoveVector(Vector2 moveVector)
    {
        if (canMove)
        {
            this.moveVector.x = moveVector.normalized.x;
            this.moveVector.z = moveVector.normalized.y;
            Rigidbody.velocity = this.moveVector * moveSpeed * Time.fixedDeltaTime;
        }
    }
    public void PlayerJump()
    {
        if (canJump && IsGrounded())
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, jumpPower, Rigidbody.velocity.z);
        }
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
