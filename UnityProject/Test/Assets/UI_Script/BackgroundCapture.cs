using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCapture : MonoBehaviour
{
    Camera gameCamera;
    public RenderTexture renderTexture; // RenderTexture
    public Image targetImage;           // ��������Ʈ�� ������ Image ������Ʈ
    float nowTime = 0;
    public float repeatTime;
    bool isRepeat = false;
    IAfterBlurBack afterAction;

    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
        afterAction = GetComponent<IAfterBlurBack>();
    }

    void CaptureBackground()
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

    public void ButtonCapture()
    {
        isRepeat = true;
        nowTime = 0;
    }

    private void Update()
    {
        if (isRepeat)
        {
            CaptureBackground();
            nowTime += Time.deltaTime;
            if (nowTime >= repeatTime)
            {
                isRepeat = false;
                afterAction.AfterBackCapture();
            }
        }
    }
}