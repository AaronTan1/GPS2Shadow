using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RefreshStaticShadows))]
public class RefreshSSEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RefreshStaticShadows refreshSS = (RefreshStaticShadows)target;

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Refresh Static Shadows");
        if (GUILayout.Button("Refresh"))
        {
            refreshSS.RefreshAllStaticShadows();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Debug Button"))
        {
            //refreshSS.DebugChild();
        }
        GUILayout.EndHorizontal();
    }
}
