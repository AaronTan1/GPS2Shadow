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
        GUILayout.Label(new GUIContent("Regenerate All Static Shadows", "Replace old shadows and add shadows to objects without one, also refreshes their properties according to the original object"));
        if (GUILayout.Button(new GUIContent("Generate", "Replace old shadows and add shadows to objects without one, also refreshes their properties according to the original object")))
        {
            refreshSS.RefreshAllStaticShadows(true);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Generate Shadows for Empties", "Generate shadows for objects without one"));
        if (GUILayout.Button(new GUIContent("Generate", "Generate shadows for objects without one")))
        {
            refreshSS.RefreshAllStaticShadows(false);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Refresh Static Shadows Properties", "Refreshes shadow properties only, size and position according to referenced light source"));
        if (GUILayout.Button(new GUIContent("Refresh", "Refreshes shadow properties only, size and position according to referenced light source")))
        {
            refreshSS.RefreshAllShadowsProperties();
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
