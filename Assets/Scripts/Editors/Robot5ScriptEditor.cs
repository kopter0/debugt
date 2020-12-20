#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Robot5Script))] 
public class Robot5ScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("populate"))
        {
            ((Robot5Script)target).PopulateParts();
        }
    }
}
#endif
