using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private static PuzzleManager _instance;
    public static PuzzleManager Instance => _instance;

    [HideInInspector] public bool disableShadow;
    [HideInInspector] public bool disableMovement;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else 
            _instance = this;
    }
    
    [SerializeField] private Material matSwapColor;
    [SerializeField] private Material matOri;
    
    public Material ChangeMaterial()
    {
        return matSwapColor;
    }

    public Material ResetMaterial()
    {
        return matOri;
    }

    public bool DisableShadow(bool flag)
    {
        disableShadow = flag;
        return disableShadow;
    }
    
    public bool DisableMovement(bool flag)
    {
        disableMovement = flag;
        return disableMovement;
    }
    
}
