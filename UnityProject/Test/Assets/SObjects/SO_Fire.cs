using System;
using UnityEngine;

[CreateAssetMenu]
public class SO_Fire : ScriptableObject
{
    [Header("Random Fire Value")]
#if UNITY_EDITOR
    [RangeSlider(0.1f, 2f), SerializeField]
#endif
    private Vector2 _randomSpeedRange;

#if UNITY_EDITOR
    [RangeSlider(0.5f, 2f), SerializeField]
#endif
    private Vector2 _randomPowerRange;

    [Header("Fire Power")]
    [Range(0.1f, 1f), SerializeField]
    private float   _power_Increase;
    [Range(0.1f, 1f), SerializeField]
    private float   _power_Decrease;

    /// <summary>
    /// SO_Fire에서 지정한 범위 내 무작위 속도를 가져옵니다. 
    /// </summary>
    public  float   randomSpeed
    {
        get         { return UnityEngine.Random.Range(_randomSpeedRange.x, _randomSpeedRange.y); }
        private set { }
    }

    /// <summary>
    /// SO_Fire에서 지정한 범위 내 무작위 크기를 가져옵니다. 
    /// </summary>
    public float   randomPower
    {
        get         { return UnityEngine.Random.Range(_randomPowerRange.x, _randomPowerRange.y); }
        private set { }
    }

    /// <summary> Fire의 초당 Power 증가량을 가져옵니다. </summary>
    public  float   power_Increase  =>  _power_Increase;
    /// <summary> Fire의 초당 Power 감소량을 가져옵니다. </summary>
    public  float   power_Decrease  =>  _power_Decrease;

    private void Reset()
    {
        _randomSpeedRange   = new Vector2(0.3f, 0.5f);
        _randomPowerRange   = new Vector2(0.8f, 1.5f);
        _power_Increase = 0.2f;
        _power_Decrease = 0.4f;
    }
}
