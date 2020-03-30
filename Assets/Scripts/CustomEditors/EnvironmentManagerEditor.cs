using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvironmentManager))]
[CanEditMultipleObjects]
public class EnvironmentManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // draw the default inspector configurations
        base.OnInspectorGUI();

        // cast the target script into a usable object
        EnvironmentManager myEnvironmentManager = (EnvironmentManager)target;

        // Add useful information right into the inspector
        EditorGUILayout.LabelField("Maximum Trees: " + myEnvironmentManager.MaximumTrees);
        EditorGUILayout.LabelField("Current Trees: " + myEnvironmentManager.CurrentTrees);
        EditorGUILayout.LabelField("Alive: " + myEnvironmentManager.PercentageOfTreesAlive + "%");
        EditorGUILayout.LabelField("Dead: " + myEnvironmentManager.PercentageOfTreesDead + "%");
    }
}
