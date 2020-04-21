using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

[CustomEditor(typeof(Well))]
[CanEditMultipleObjects]
public class WellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // draw the default inspector configurations
        base.OnInspectorGUI();
        // cast the target script into a usable object
        Well myWell = (Well)target;

        // Add options for the bucket
        EditorGUILayout.BeginHorizontal();
        myWell.bucketAttached = EditorGUILayout.Toggle("Attached Bucket", myWell.bucketAttached);
        myWell.attachedBucketFillAmount = EditorGUILayout.Slider(myWell.attachedBucketFillAmount, 0, myWell.attachedBucketMaxFillAmount);
        EditorGUILayout.EndHorizontal();
        myWell.bucketHeight = EditorGUILayout.Slider(myWell.bucketHeight, 0, 1);

    }
}
