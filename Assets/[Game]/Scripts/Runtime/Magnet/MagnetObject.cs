using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class MagnetObject : MonoBehaviour, IMagnetable
{
    #region Variables
    [SerializeField] PublicEnums.MagneticPole magneticPole;
    Vector3 forceDirection;
    const float PULL_POWER = 2;
    const float PUSH_POWER = 40;
    #endregion
    #region Properties 
    Rigidbody rb;
    Rigidbody Rigidbody { get { return(rb==null)?rb=GetComponent<Rigidbody>() :rb; } }
    #endregion
    #region MonoBehaviour Methods

    #endregion
    #region My Methods
    public void Pull(GameObject mainMagnet)
    {
        if (CanPull(mainMagnet))
        {
            forceDirection = (mainMagnet.transform.position - transform.position).normalized;
            Rigidbody.AddForce(forceDirection * PULL_POWER, ForceMode.Impulse);
        }
    }

    public void Push(GameObject mainMagnet)
    {
        transform.parent = null;
        forceDirection = (transform.position - mainMagnet.transform.position).normalized;
        Rigidbody.AddForce(forceDirection * PUSH_POWER, ForceMode.Force);
    }
    public PublicEnums.MagneticPole GetMagneticPole()
    {
        return magneticPole;
    }
    bool CanPull(GameObject mainMagnet)
    {
        if (transform.parent == mainMagnet.transform) return false;
        return true;
    }
    #endregion
}
