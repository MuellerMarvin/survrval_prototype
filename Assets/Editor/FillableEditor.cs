using System.Linq;
using UnityEditor;

[CustomEditor(typeof(Fillable))]
[CanEditMultipleObjects]
public class FillableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // draw the default inspector configurations
        base.OnInspectorGUI();

        // cast the target script into a usable object
        Fillable myFillable = (Fillable)target;

        // add a slider for the blackLevel
        myFillable.currentFillAmount = EditorGUILayout.Slider(myFillable.currentFillAmount, 0, myFillable.maxFillAmount, null);
    }
}
