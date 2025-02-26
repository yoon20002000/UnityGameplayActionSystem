using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Action))]
public class ActionEditor : Editor
{
    SerializedProperty actionProperty;
    SerializedProperty grantsProperty;
    SerializedProperty blockedProperty;

    private void OnEnable()
    {
        actionProperty = serializedObject.FindProperty("activationTag");
        grantsProperty = serializedObject.FindProperty("grantsTags");
        blockedProperty = serializedObject.FindProperty("blockedTags");
    }
    public override void OnInspectorGUI()
    {
        Action action = (Action)target;

        bool bIsDirty = false;

        drawActiveTag(action, ref bIsDirty);
        drawGrantTags(action, ref bIsDirty);
        drawBlokcedTags(action, ref bIsDirty);

        if (bIsDirty == true)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    protected void drawActiveTag(Action action, ref bool bIsDirty)
    {
        var selectedTag = (GameplayTags)actionProperty.enumValueFlag;
        var tag = (GameplayTags)EditorGUILayout.EnumPopup("Activation Tag", selectedTag);
        if (selectedTag != tag)
        {
            actionProperty.enumValueFlag = (int)tag;
            bIsDirty = true;
        }
    }
    protected void drawGrantTags(Action action, ref bool bIsDirty)
    {
        var selectedTags = (GameplayTags)grantsProperty.enumValueFlag;
        var newTags = (GameplayTags)EditorGUILayout.EnumFlagsField("Garnts Tags", selectedTags);
        if (selectedTags != newTags)
        {
            grantsProperty.enumValueFlag = (int)newTags;
            bIsDirty = true;
        }
    }
    protected void drawBlokcedTags(Action action, ref bool bIsDirty)
    {
        var selectedTags = (GameplayTags)blockedProperty.enumValueFlag;
        var newTags = (GameplayTags)EditorGUILayout.EnumFlagsField("Blocked Tags", selectedTags);
        if (selectedTags != newTags)
        {
            blockedProperty.enumValueFlag = (int)newTags;
            bIsDirty = true;
        }
    }
}
