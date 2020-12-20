#if UNITY_REGION
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Robot2Script))]
public class Robot2ScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Populate"))
        {
            ((Robot2Script)target).PopulateParts();
        }
    }
}
#endif