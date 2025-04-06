using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class RangeSliderAttribute : PropertyAttribute
{
    public float Min;
    public float Max;

    public RangeSliderAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}

[CustomPropertyDrawer(typeof(RangeSliderAttribute))]
public class RangeSliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 어트리뷰트를 가져옵니다.
        RangeSliderAttribute range = (RangeSliderAttribute)attribute;

        // Vector2 타입의 값이 필요합니다.
        if (property.propertyType == SerializedPropertyType.Vector2)
        {
            EditorGUI.BeginProperty(position, label, property);

            // 현재 값을 가져옵니다.
            Vector2 rangeValues = property.vector2Value;
            float min = rangeValues.x;
            float max = rangeValues.y;

            // 레이블을 위한 영역
            Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PrefixLabel(labelRect, label);

            // 슬라이더 영역 설정
            float fieldWidth = 50f;
            float sliderWidth = position.width - fieldWidth * 2 - 10f;

            // 최솟값 입력 필드
            Rect minFieldRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, fieldWidth, EditorGUIUtility.singleLineHeight);
            min = EditorGUI.FloatField(minFieldRect, Mathf.Clamp(min, range.Min, max));

            // 최솟값 반올림 (소수점 2자리)
            min = Mathf.Round(min * 100f) / 100f;

            // 슬라이더
            Rect sliderRect = new Rect(position.x + fieldWidth + 5f, position.y + EditorGUIUtility.singleLineHeight + 2, sliderWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, range.Min, range.Max);

            // 최댓값 반올림 (소수점 2자리)
            max = Mathf.Round(max * 100f) / 100f;

            // 최댓값 입력 필드
            Rect maxFieldRect = new Rect(position.x + fieldWidth + sliderWidth + 10f, position.y + EditorGUIUtility.singleLineHeight + 2, fieldWidth, EditorGUIUtility.singleLineHeight);
            max = EditorGUI.FloatField(maxFieldRect, Mathf.Clamp(max, min, range.Max));

            // 최솟값과 최댓값을 다시 반올림하여 벡터로 저장
            property.vector2Value = new Vector2(min, max);

            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use only with Vector2");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + 4;
    }
}
#endif