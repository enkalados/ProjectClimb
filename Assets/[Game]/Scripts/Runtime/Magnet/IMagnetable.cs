using UnityEngine;

public interface IMagnetable
{
    #region Variables

    #endregion
    #region Properties 

    #endregion
    #region MonoBehaviour Methods

    #endregion
    #region My Methods
    public void Push(GameObject mainMagnet);
    public void Pull(GameObject mainMagnet);
    public Enums.PublicEnums.MagneticPole GetMagneticPole();
    #endregion
}
