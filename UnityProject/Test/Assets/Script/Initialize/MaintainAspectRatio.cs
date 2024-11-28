using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MaintainAspectRatio : MonoBehaviour
{
    [SerializeField]
    private float targetAspectRatio = 16f / 9f; // 목표 비율 (16:9)

    void Start()
    {
        SetCameraAspectRatio();
    }

    void SetCameraAspectRatio()
    {
        // 현재 화면의 비율 계산
        float windowAspectRatio = (float)Screen.width / Screen.height;

        // 비율 비교
        float scaleHeight = windowAspectRatio / targetAspectRatio;

        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)
        {
            // 화면이 좁을 경우 (좌우가 잘림)
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            // 화면이 넓을 경우 (위아래가 잘림)
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}

