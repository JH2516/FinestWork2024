using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu]
public class SO_Player : ScriptableObject
{
    public readonly Vector2[] moveVecs =
    {
        new Vector2(-1, 1),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(-1, 0),
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(-1, -1),
        new Vector2(0, -1),
        new Vector2(1, -1)
    };

    public readonly Quaternion[] rotations =
    {
        Quaternion.Euler(0, 0, 45 * 3),
        Quaternion.Euler(0, 0, 45 * 2),
        Quaternion.Euler(0, 0, 45 * 1),
        Quaternion.Euler(0, 0, 45 * 4),
        Quaternion.Euler(0, 0, 45 * 0),
        Quaternion.Euler(0, 0, 45 * 0),
        Quaternion.Euler(0, 0, 45 * 5),
        Quaternion.Euler(0, 0, 45 * 6),
        Quaternion.Euler(0, 0, 45 * 7)
    };

    [Header("Light")]
    [Range(0.1f, 1f), SerializeField]
    private float _intensity_Around = 0.5f;

    [Range(0f, 10f), SerializeField]
    private float _radius_InnerAround;
    [Range(0f, 10f), SerializeField]
    private float _radius_OuterAround;

    [Range(0f, 10f), SerializeField]
    private float _radius_InnerFOV;
    [Range(0f, 10f), SerializeField]
    private float _radius_OuterFOV;
    [Range(0f, 360f), SerializeField]
    private float _Angle_InnerFOV;
    [Range(0f, 360f), SerializeField]
    private float _Angle_OuterFOV;

    [Range(1f, 4f), SerializeField]
    private float _changePower;


    [Header("Light - Boost")]

    [Range(0f, 10f), SerializeField]
    private float _boost_radius_InnerAround;
    [Range(0f, 10f), SerializeField]
    private float _boost_radius_OuterAround;

    [Range(0f, 10f), SerializeField]
    private float _boost_radius_InnerFOV;
    [Range(0f, 10f), SerializeField]
    private float _boost_radius_OuterFOV;
    [Range(0f, 360f), SerializeField]
    private float _boost_Angle_InnerFOV;
    [Range(0f, 360f), SerializeField]
    private float _boost_Angle_OuterFOV;

    [Range(1f, 4f), SerializeField]
    private float _boost_changePower;

    private void Reset()
    {
        _intensity_Around = 0.5f;

        _radius_InnerAround = 2f;
        _radius_OuterAround = 7f;
        _radius_InnerFOV = 0f;
        _radius_OuterFOV = 5f;
        _Angle_InnerFOV = 30f;
        _Angle_OuterFOV = 45f;
        _changePower = 2f;

        _boost_radius_InnerAround = 2f;
        _boost_radius_OuterAround = 8f;
        _boost_radius_InnerFOV = 2f;
        _boost_radius_OuterFOV = 8f;
        _boost_Angle_InnerFOV = 60f;
        _boost_Angle_OuterFOV = 60f;
        _boost_changePower = 1.5f;
    }


    /// <summary>
    /// 어두운 방 내 플레이어 여부에 따른 전등 변화
    /// </summary>
    /// <param name="lightAround"> Light2D : 플레이어 주변 </param>
    /// <param name="lightFOV"> Light2D : 플레이어 전방 </param>
    /// <param name="isPlayerInRoom"> 플레이어 방 입장 여부 </param>
    /// <param name="isBoost"> 대상 부스트 아이템 사용 여부 </param>
    public void SetLight_DarkedRoom(Light2D lightAround, Light2D lightFOV, bool isPlayerInRoom, bool isBoost = false)
    {
        if (isBoost)
        {
            lightAround.intensity
                = isPlayerInRoom ? _intensity_Around / _boost_changePower : _intensity_Around;
            lightAround.pointLightInnerRadius
                = isPlayerInRoom ? _boost_radius_InnerAround / _boost_changePower : _boost_radius_InnerAround;
            lightAround.pointLightOuterRadius
                = isPlayerInRoom ? _boost_radius_OuterAround / _boost_changePower : _boost_radius_OuterAround;

            lightFOV.pointLightInnerRadius =
                isPlayerInRoom ? _boost_radius_InnerFOV / _boost_changePower : _boost_radius_InnerFOV;
            lightFOV.pointLightOuterRadius =
                isPlayerInRoom ? _boost_radius_OuterFOV / _boost_changePower : _boost_radius_OuterFOV;
            lightFOV.pointLightInnerAngle =
                isPlayerInRoom ? _boost_Angle_InnerFOV / _boost_changePower : _boost_Angle_InnerFOV;
            lightFOV.pointLightInnerAngle =
                isPlayerInRoom ? _boost_Angle_OuterFOV / _boost_changePower : _boost_Angle_OuterFOV;
        }
        else
        {
            lightAround.intensity
                = isPlayerInRoom ? _intensity_Around / _changePower : _intensity_Around;
            lightAround.pointLightInnerRadius
                = isPlayerInRoom ? _radius_InnerAround / _changePower : _boost_radius_InnerAround;
            lightAround.pointLightOuterRadius
                = isPlayerInRoom ? _radius_OuterAround / _changePower : _radius_OuterAround;

            lightFOV.pointLightInnerRadius =
                isPlayerInRoom ? _radius_InnerFOV / _changePower : _boost_radius_InnerFOV;
            lightFOV.pointLightOuterRadius =
                isPlayerInRoom ? _radius_OuterFOV / _changePower : _radius_OuterFOV;
            lightFOV.pointLightInnerAngle =
                isPlayerInRoom ? _Angle_InnerFOV / _changePower : _Angle_InnerFOV;
            lightFOV.pointLightInnerAngle =
                isPlayerInRoom ? _Angle_OuterFOV / _changePower : _Angle_OuterFOV;
        }
    }
}
