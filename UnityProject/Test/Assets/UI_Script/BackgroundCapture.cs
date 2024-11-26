using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCapture : MonoBehaviour
{
    public Camera gameCamera;          // ĸó�� ī�޶�
    public RenderTexture renderTexture; // RenderTexture
    public Image targetImage;           // ��������Ʈ�� ������ Image ������Ʈ

    void CaptureToSprite()
    {
        // ī�޶� RenderTexture�� �������ϵ��� ����
        gameCamera.targetTexture = renderTexture;

        // RenderTexture���� Texture2D�� ����
        RenderTexture.active = renderTexture;
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        // Texture2D�� ��������Ʈ�� ��ȯ
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));

        // Image ������Ʈ�� ��������Ʈ ����
        targetImage.sprite = sprite;
    }

    private void Update()
    {
        CaptureToSprite();
    }
}