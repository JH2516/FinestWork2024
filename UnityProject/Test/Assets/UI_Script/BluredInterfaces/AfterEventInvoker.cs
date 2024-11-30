using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class AfterEventInvoker : MonoBehaviour
{
    public string invokerName; // 단순 구분용
    [SerializeField]
    private int id;
    public int Id { get => id; set => id = value; }
    public UnityEvent action;

    public void AfterBackCapture()
    {
        action.Invoke();
    }
}

[CustomEditor(typeof(AfterEventInvoker))]
public class AfterEventInvokerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AfterEventInvoker myTarget = (AfterEventInvoker)target;
        myTarget.invokerName = EditorGUILayout.TextField("Invoker Name", myTarget.invokerName);
        myTarget.Id = EditorGUILayout.IntField("ID", myTarget.Id);

        if (myTarget.action != null)
        {
            SerializedProperty actionProperty = serializedObject.FindProperty("action");
            EditorGUILayout.PropertyField(actionProperty, true);
        }
        else
        {
            GUILayout.Label("List is empty.", EditorStyles.helpBox);
        }
        serializedObject.ApplyModifiedProperties();
    }
}