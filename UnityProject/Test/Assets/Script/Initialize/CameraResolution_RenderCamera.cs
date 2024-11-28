using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraResolution_RenderCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        UpdateCameraViewport();
    }

    private void UpdateCameraViewport()
    {
        // 원하는 비율: 16:9
        float targetAspect = 16.0f / 9.0f;

        // 현재 화면의 비율 계산
        float windowAspect = (float)Screen.width / Screen.height;

        // 비율 비교
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // 화면이 타겟 비율보다 더 넓음 (레터박스 필요)
            Rect rect = mainCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            mainCamera.rect = rect;
        }
        else
        {
            // 화면이 타겟 비율보다 더 좁음 (필러박스 필요)
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = mainCamera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            mainCamera.rect = rect;
        }
    }

    private void OnValidate()
    {
        if (!mainCamera)
            mainCamera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        // 화면 크기 변경 시 자동 업데이트
        UpdateCameraViewport();
    }

    private void OnDisable()
    {
        // 원래대로 복구
        mainCamera.rect = new Rect(0, 0, 1, 1);
    }

    private void Update()
    {
        // 실시간으로 해상도 변경 반영 (선택 사항)
        if (Screen.width != Screen.currentResolution.width || Screen.height != Screen.currentResolution.height)
        {
            UpdateCameraViewport();
        }
    }
}
