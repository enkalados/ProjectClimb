using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput.CustomInput;
using System;

[RequireComponent(typeof(PlayerMovementControl))]
public class PlayerInputControl : MonoBehaviour
{
    #region Variables
    private CustomInput customInput = null;
    #endregion
    #region Properties
    PlayerMovementControl playerMovementControl;
    PlayerMovementControl PlayerMovementControl { get { return (playerMovementControl == null) ? playerMovementControl = GetComponent<PlayerMovementControl>() : playerMovementControl; } }
    #endregion
    #region MonoBehaviour Methods
    private void Awake()
    {
        customInput = new CustomInput();
    }
    private void OnEnable()
    {
        OpenInput();
        customInput.Player.Movement.performed += OnMovementPerformend;
        customInput.Player.Movement.canceled += OnMovementCancelled;
        customInput.Player.Jump.performed += OnJumpPerformend;
    }
    private void OnDisable()
    {
        CloseInput();
        customInput.Player.Movement.performed -= OnMovementPerformend;
        customInput.Player.Movement.canceled -= OnMovementCancelled;
        customInput.Player.Jump.performed -= OnJumpPerformend;
    }
    #endregion
    #region InputMethods
    private void OnMovementPerformend(InputAction.CallbackContext value)
    {
        PlayerMovementControl.SetMoveVector(value.ReadValue<Vector2>());
    }
    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        PlayerMovementControl.SetMoveVector(Vector2.zero);
    }
    private void OnJumpPerformend(InputAction.CallbackContext context)
    {
        PlayerMovementControl.PlayerJump();
    }
    #endregion
    #region Helpers

    private void OpenInput()
    {
        customInput.Enable();
    }
    private void CloseInput()
    {
        customInput.Disable();
    }
    #endregion
}
