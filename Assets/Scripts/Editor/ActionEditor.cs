using UnityEditor;
using UnityEngine;
//[CustomEditor(typeof(Action))]
public class ActionEditor : Editor
{
    SerializedProperty actionProperty;
    SerializedProperty grantsProperty;
    SerializedProperty cancelProperty;
    SerializedProperty blockedProperty;

    private void OnEnable()
    {
        actionProperty = serializedObject.FindProperty("activationTag");
        grantsProperty = serializedObject.FindProperty("grantsTags");
        cancelProperty = serializedObject.FindProperty("cancelTags");
        blockedProperty = serializedObject.FindProperty("blockedTags");
    }
    public override void OnInspectorGUI()
    {
        Action action = (Action)target;

        bool bIsDirty = false;

        drawActiveTag(action, ref bIsDirty);
        drawGrantTags(action, ref bIsDirty);
        drawCancelTags(action, ref bIsDirty);
        drawBlokcedTags(action, ref bIsDirty);

        if (bIsDirty == true)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
    protected void drawActiveTag(Action action, ref bool bIsDirty)
    {
        var selectedTag = (EGameplayTags)actionProperty.enumValueFlag;
        var tag = (EGameplayTags)EditorGUILayout.EnumPopup("Activation Tag", selectedTag);
        if (selectedTag != tag)
        {
            actionProperty.enumValueFlag = (int)tag;
            bIsDirty = true;
        }
    }
    protected void drawGrantTags(Action action, ref bool bIsDirty)
    {
        var selectedTags = (EGameplayTags)grantsProperty.enumValueFlag;
        var newTags = (EGameplayTags)EditorGUILayout.EnumFlagsField("Garnts Tags", selectedTags);
        if (selectedTags != newTags)
        {
            grantsProperty.enumValueFlag = (int)newTags;
            bIsDirty = true;
        }
    }
    private void drawCancelTags(Action action, ref bool bIsDirty)
    {
        var selectedTags = (EGameplayTags)cancelProperty.enumValueFlag;
        var newTags = (EGameplayTags)EditorGUILayout.EnumFlagsField("Cancel Tags", selectedTags);
        if (selectedTags != newTags)
        {
            cancelProperty.enumValueFlag = (int)newTags;
            bIsDirty = true;
        }
    }
    protected void drawBlokcedTags(Action action, ref bool bIsDirty)
    {
        var selectedTags = (EGameplayTags)blockedProperty.enumValueFlag;
        var newTags = (EGameplayTags)EditorGUILayout.EnumFlagsField("Blocked Tags", selectedTags);
        if (selectedTags != newTags)
        {
            blockedProperty.enumValueFlag = (int)newTags;
            bIsDirty = true;
        }
    }
}
