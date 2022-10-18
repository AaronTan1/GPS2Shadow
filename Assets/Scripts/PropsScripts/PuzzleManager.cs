using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private static PuzzleManager _instance;
    public static PuzzleManager Instance { get { return _instance; } }
    
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
}
