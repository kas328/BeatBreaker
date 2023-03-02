using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCollision : MonoBehaviour
{
    #region Serialize Field
    [SerializeField] private Collider[] _firstRowChains = null;
    [SerializeField] private Collider _boxingBag = null;
    #endregion

    private void Awake()
    {
        IgnoreCollisions();
        InitVariables();
    }

    #region Initialize
    private void IgnoreCollisions()
    {
        foreach (Collider chain in _firstRowChains)
        {
            Physics.IgnoreCollision(_boxingBag, chain, true);
        }
    }

    private void InitVariables()
    {
        
    }
    #endregion
}