using UnityEngine;
using UnityEngine.InputSystem;
using Player.MovementControl;

namespace Player.InputControl
{
    public class PlayerInputControl : MonoBehaviour
    {
        #region Variables
        #endregion
        #region Properties
        PlayerMovementControl playerMovementControl;
        PlayerMovementControl PlayerMovementControl { get { return (playerMovementControl == null) ? playerMovementControl = GetComponent<PlayerMovementControl>() : playerMovementControl; } }
        #endregion
        #region MonoBehaviour Methods

        #endregion
        #region InputMethods
        public void OnMovementPerformend(InputAction.CallbackContext value)
        {
            PlayerMovementControl.SetMoveVector(value.ReadValue<Vector2>());
        }

        public void OnJumpPerformend(InputAction.CallbackContext context)
        {
            PlayerMovementControl.PlayerJump();
        }
        #endregion
        #region Helpers

        #endregion
    }
}