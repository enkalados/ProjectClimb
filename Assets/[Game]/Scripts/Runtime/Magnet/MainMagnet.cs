using Enums;
using System.Collections.Generic;
using UnityEngine;

public class MainMagnet : MonoBehaviour
{
    #region Variables
    [SerializeField] PublicEnums.MagneticPole magneticPole;
    List<IMagnetable> magnetableObjects = new List<IMagnetable>();
    #endregion
    #region Properties 

    #endregion
    #region MonoBehaviour Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMagnetable magnetObject))
        {
            AddMagnetableObjec(magnetObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IMagnetable magnetObject))
        {
            RemoveMagnetableObject(magnetObject);
        }
    }
    private void FixedUpdate()
    {
        RuntheMagnet();
    }
    #endregion
    #region My Methods
    void RuntheMagnet()
    {
        if (magnetableObjects.Count==0)
        {
            return;
        }
        foreach (IMagnetable magnetObject in magnetableObjects)
        {
            CheckMagnetPole(magnetObject);
        }
    }
    void CheckMagnetPole(IMagnetable magnetObject)
    {
        if (magneticPole == magnetObject.GetMagneticPole())
        {
            magnetObject.Push(gameObject);
        }
        else
        {
            magnetObject.Pull(gameObject);
        }
    }
    void AddMagnetableObjec(IMagnetable magnetObject)
    {
        if (!magnetableObjects.Contains(magnetObject))
        {
            magnetableObjects.Add(magnetObject);
        }
    }
    void RemoveMagnetableObject(IMagnetable magnetObject)
    {
        if (magnetableObjects.Contains(magnetObject))
        {
            magnetableObjects.Remove(magnetObject);
        }
    }
    #endregion
}
