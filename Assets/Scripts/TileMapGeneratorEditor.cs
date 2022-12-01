using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(TileMapGenerator))]
public class TileMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var gen = (TileMapGenerator)target;

        if (GUILayout.Button("Set Up Tiles"))
        {
            gen.SetUpTiles();
        }
        
        if (GUILayout.Button("Check For Assets"))
        {
            if (gen.CheckForAssets())
                Debug.Log("All assets assigned");
            else
                Debug.Log("Not all assets assigned");
        }
        
        if (GUILayout.Button("SpawnTiles"))
        {
            if (gen.CheckForAssets())
                gen.GenerateTiles();
            else
                Debug.LogError("Not all assets assigned");
        }
    }
}
#endif
