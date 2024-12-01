using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

public class AfterEventInvoker : MonoBehaviour
{
    public string invokerName; // �ܼ� ���п�
    [SerializeField]
    private int id;
    public int Id { get => id; set => id = value; }
    public UnityEvent action;

    public void AfterBackCapture()
    {
        action.Invoke();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AfterEventInvoker))]
public class AfterEventInvokerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Script �ʵ带 ���� �Ұ����ϰ� ǥ��
        GUI.enabled = false; // GUI ��Ȱ��ȭ
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);
        GUI.enabled = true; // GUI �ٽ� Ȱ��ȭ

        // �⺻������ ����ȭ�� �ʵ带 �׸���, "action"�� ����
        SerializedProperty actionProperty = serializedObject.FindProperty("action");

        // �⺻ �ʵ� �׸��� (action ����)
        DrawPropertiesExcluding(serializedObject, "action", "m_Script");

        // action �ʵ带 �������� �׸���
        if (actionProperty != null)
        {
            EditorGUILayout.PropertyField(actionProperty, true);
        }
        else
        {
            GUILayout.Label("List is empty.", EditorStyles.helpBox);
        }

        // ����ȭ�� ��ü�� ���� ������ �����մϴ�.
        serializedObject.ApplyModifiedProperties();
    }
}
#endif