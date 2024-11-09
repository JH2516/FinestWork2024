using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fire : MonoBehaviour
{
    public  bool    isExtinguish;
    public  bool    isBackdraft = false;

    private float   intensity_Increase;
    private float   intensity_Decrease;

    private float   power_Fire;

    [SerializeField]
    private Light2D light;

    private void Awake()
    {
        intensity_Increase = 0.2f;
        intensity_Decrease = 0.4f;

        power_Fire = 1f;
        light.intensity = 1f;

        isExtinguish = false;
    }

    private void OnEnable()
    {
        //light.GetComponent<Light2D>();
    }

    private void Update()
    {
        Check_Extinguished();
    }

    private void FixedUpdate()
    {
        Fire_State();
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

        transform.localScale = Vector3.one * power_Fire;
        light.intensity = power_Fire;

        if (power_Fire >= 1) power_Fire = 1;
    }

    private void Check_Extinguished()
    {
        if (light.intensity <= 0)
        gameObject.SetActive(false);
    }
}
