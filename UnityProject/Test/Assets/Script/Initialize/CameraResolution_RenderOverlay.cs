using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraResolution_RenderOverlay : MonoBehaviour
{
    private float targetAspect = 16f / 9f; // 16:9 비율

    private void Start()
    {
        AdjustCameraViewport();
    }

    private void AdjustCameraViewport()
    {
        // 현재 화면의 종횡비 (aspect ratio)
        float screenAspect = (float)Screen.width / (float)Screen.height;

        // 화면 비율과 목표 비율 비교
        if (screenAspect > targetAspect)
        {
            // 화면이 더 넓은 경우: 상/하에 검은 영역 추가
            float scaleHeight = targetAspect / screenAspect;
            Camera.main.rect = new Rect(0, (1f - scaleHeight) / 2f, 1f, scaleHeight);
        }
        else if (screenAspect < targetAspect)
        {
            // 화면이 더 높은 경우: 좌/우에 검은 영역 추가
            float scaleWidth = screenAspect / targetAspect;
            Camera.main.rect = new Rect((1f - scaleWidth) / 2f, 0, scaleWidth, 1f);
        }
        else
        {
            // 이미 16:9 비율
            Camera.main.rect = new Rect(0, 0, 1, 1);
        }
    }
}