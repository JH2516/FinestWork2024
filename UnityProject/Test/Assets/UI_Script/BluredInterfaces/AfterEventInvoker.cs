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
        // Script 필드를 수정 불가능하게 표시
        GUI.enabled = false; // GUI 비활성화
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);
        GUI.enabled = true; // GUI 다시 활성화

        // 기본적으로 직렬화된 필드를 그리되, "action"은 제외
        SerializedProperty actionProperty = serializedObject.FindProperty("action");

        // 기본 필드 그리기 (action 제외)
        DrawPropertiesExcluding(serializedObject, "action", "m_Script");

        // action 필드를 수동으로 그리기
        if (actionProperty != null)
        {
            EditorGUILayout.PropertyField(actionProperty, true);
        }
        else
        {
            GUILayout.Label("List is empty.", EditorStyles.helpBox);
        }

        // 직렬화된 객체의 변경 사항을 적용합니다.
        serializedObject.ApplyModifiedProperties();
    }
}