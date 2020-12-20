#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[CustomEditor(typeof(NPCScript))]
public class NPCScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Populate Damagable Parts"))
        {
            ((NPCScript)target).PopulateParts();
        }
    }
}
#endif 