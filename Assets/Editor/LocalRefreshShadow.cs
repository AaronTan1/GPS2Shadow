using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaticFakeShadow))]

public class LocalRefreshShadow : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        StaticFakeShadow localStatic = (StaticFakeShadow)target;

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Refresh Shadow", "Refreshes shadow properties for this object"));
        if (GUILayout.Button(new GUIContent("Refresh", "Refreshes shadow properties for this object")))
        {
            localStatic.CastFakeShadow(null);
        }
        GUILayout.EndHorizontal();
    }
}
