using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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


public class Fire : MonoBehaviour
{
    private StageManager    stageManager;

    [Header("Fire Animation")]
    [SerializeField]
    private Animator        animator;
    [SerializeField]
    private SpriteRenderer  sprite;

#if UNITY_EDITOR
    [RangeSlider(0.1f, 2f)]
#endif
    [SerializeField]
    private Vector2         RandomSpeedRange = new Vector2(0.3f, 0.5f);

    [Header("Fire Sprite Mask")]
    public  SpriteMask      mask;

    [Header("Fire Light")]
    [SerializeField]
    private Light2D light;

    [Header("Fire Light Intensity")]
    [Range(0.1f, 1f), SerializeField]
    private float           intensity_Increase = 0.2f;
    [Range(0.1f, 1f), SerializeField]
    private float           intensity_Decrease = 0.4f;

    [Header("Fire Extinguish")]
    [SerializeField]
    private Type_Extinguish type_Extinguish;
    public  bool            isExtinguish;

    [Header("Fire In BackDraft Room")]
    public  bool            isBackdraft = false;

    private float           power_Fire = 1;
    private float           powerMax_Fire;

    public enum Type_Extinguish { All, Down }

    //private void Awake()
    //{
        
    //}

    public void Init_Fire()
    {
        if (isBackdraft) return;

        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        transform.localScale = Vector2.one * UnityEngine.Random.Range(0.8f, 1.5f);

        powerMax_Fire = transform.localScale.x;
        power_Fire = powerMax_Fire;

        light.intensity = 1f;

        isExtinguish = false;

        animator.speed =
        UnityEngine.Random.Range(RandomSpeedRange.x, RandomSpeedRange.y);

        sprite.sortingOrder = 0;
    }

    public void Set_OrderInLayer(int orderNum = 0)
    {
        sprite.sortingOrder = orderNum;
        mask.frontSortingOrder = orderNum;
        mask.backSortingOrder = orderNum - 1;
    }

    private void Update()
    {
        if (isBackdraft) return;
        Check_Extinguished();
    }

    private void FixedUpdate()
    {
        Fire_State();
        Fire_Extinguished();
    }

    private void Fire_State()
    {
        switch (isExtinguish)
        {
            case true:
                power_Fire -= intensity_Decrease * Time.deltaTime;
                break;

            case false:
                power_Fire += intensity_Increase * Time.deltaTime;
                break;
        }

        if (power_Fire >= powerMax_Fire) power_Fire = powerMax_Fire;
    }

    private void Fire_Extinguished()
    {
        switch (type_Extinguish)
        {
            case Type_Extinguish.All:
                transform.localScale = Vector3.one * power_Fire;
                break;

            case Type_Extinguish.Down:
                transform.localScale = new Vector2(powerMax_Fire, power_Fire);
                break;
        }
        
        light.intensity = power_Fire;
    }

    private void Check_Extinguished()
    {
        if (isBackdraft)        return;
        if (power_Fire > 0.1f)  return;

        stageManager.Discount_Fires(gameObject);
        gameObject.SetActive(false);
    }
}
