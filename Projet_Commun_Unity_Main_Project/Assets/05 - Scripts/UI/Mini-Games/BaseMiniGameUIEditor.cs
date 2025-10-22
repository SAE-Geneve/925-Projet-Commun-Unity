/*using System;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseMinigameUI))]
public abstract class BaseMiniGameUIEditor : TMP_BaseEditorPanel
{
    override public void OnInspectorGUI()
    {
        var myScript = target as BaseMinigameUI;

        myScript.ExtraScores = EditorGUILayout.Toggle("Add extra scores", myScript.ExtraScores);

        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.ExtraScores)))
        {
            if (group.visible == false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PrefixLabel("Subscore 3");
                myScript.totalSubScore3 = EditorGUILayout.TextField("New name", myScript.totalSubScore3.text);
                EditorGUILayout.PrefixLabel("Subscore 4");
                myScript.totalSubScore4 = EditorGUILayout.TextField(myScript.totalSubScore4);
                EditorGUI.indentLevel--;
            }
        }

        myScript.ExtraScores = GUILayout.Toggle(myScript.ExtraScores, "Disable Fields");

        using (new EditorGUI.DisabledScope(myScript.ExtraScores))
        {
            //myScript.someColor = EditorGUILayout.ColorField("Color", myScript.someColor);
            //myScript.someString = EditorGUILayout.Field("Text", myScript.someString);
            //myScript.someNumber = EditorGUILayout.IntField("Number", myScript.someNumber);
        }
    }
}*/

//YEAH SO TMP_TEXT IS ANNOYING AND DOES NOT WANT TO WORK WITH EDITOR
