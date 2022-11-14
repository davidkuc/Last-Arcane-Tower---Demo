
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(BaseEnemyControl))]
public class BaseEnemyControl_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        BaseEnemyControl script = (BaseEnemyControl)target;


        EditorGUILayout.LabelField("Stats overriding", EditorStyles.boldLabel);

        // draw checkbox for the bool
        script.overrideMovementSpeed = EditorGUILayout.Toggle("Override Movement Speed", script.overrideMovementSpeed);
        if (script.overrideMovementSpeed )// if bool is true, show other fields
        {
            script.newMovementSpeed = EditorGUILayout.FloatField(script.newMovementSpeed);
        }
    }
}
#endif
