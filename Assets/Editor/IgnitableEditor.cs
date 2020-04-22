using System.Linq;
using UnityEditor;

[CustomEditor(typeof(Ignitable))]
[CanEditMultipleObjects]
public class IgnitableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // draw the default inspector configurations
        base.OnInspectorGUI();

        // cast the target script into a usable object
        Ignitable myIgnitable = (Ignitable)target;

        // add a slider for the blackLevel
        myIgnitable.blackLevel = EditorGUILayout.Slider(myIgnitable.blackLevel, 0, 1, null);
    }
}
