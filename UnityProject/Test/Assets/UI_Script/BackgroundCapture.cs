using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCapture : MonoBehaviour
{
    public Camera gameCamera;          // 캡처할 카메라
    public RenderTexture renderTexture; // RenderTexture
    public Image targetImage;           // 스프라이트를 설정할 Image 컴포넌트

    void CaptureToSprite()
    {
        // 카메라가 RenderTexture에 렌더링하도록 설정
        gameCamera.targetTexture = renderTexture;

        // RenderTexture에서 Texture2D로 복사
        RenderTexture.active = renderTexture;
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        // Texture2D를 스프라이트로 변환
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));

        // Image 컴포넌트에 스프라이트 설정
        targetImage.sprite = sprite;
    }

    private void Update()
    {
        CaptureToSprite();
    }
}