using UnityEngine;

namespace Player.AnimationsControl
{
    public class PlayerAnimationsControl : MonoBehaviour
    {
        #region Variables
        #region PlayerDirectionVariabes
        const float RIGHT_DIRECTION = 90;
        const float LEFT_DIRECTION = -90;
        bool isFacingRight = true;
        #endregion

        #endregion
        #region Properties 

        #endregion
        #region MonoBehaviour Methods

        #endregion
        #region My Methods
        public void SetPlayerDirection(float x)
        {
            if (!isFacingRight && x > 0)
            {
                isFacingRight = true;
                transform.rotation = Quaternion.Euler(0, RIGHT_DIRECTION, 0);
            }
            else if (isFacingRight && x < 0)
            {
                isFacingRight = false;
                transform.rotation = Quaternion.Euler(0, LEFT_DIRECTION, 0);
            }
        }
        #endregion
    }
}