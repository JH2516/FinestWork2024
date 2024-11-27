using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCapture : MonoBehaviour
{
    Camera gameCamera;
    public RenderTexture renderTexture; // RenderTexture
    public Canvas captureCanvas;
    public Image[] targetImage;           // ��������Ʈ�� ������ Image ������Ʈ
    float nowTime = 0;
    public float repeatTime;
    bool isRepeat = false;
    int actionID = 0;
    AfterEventInvoker[] afterAction;

    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
        afterAction = GetComponents<AfterEventInvoker>();
        Array.Sort(afterAction, (x, y) => x.Id.CompareTo(y.Id));
    }

    void CaptureBackground()
    {
        captureCanvas.renderMode = RenderMode.ScreenSpaceCamera;
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
        targetImage[actionID].sprite = sprite;
    }

    public void ButtonCapture()
    {
        actionID = 0;
        nowTime = 0;
        isRepeat = true;
    }

    public void ButtonCaptureID(int id)
    {
        actionID = id;
        nowTime = 0;
        isRepeat = true;
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
                gameCamera.targetTexture = null;
                captureCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                afterAction[actionID].AfterBackCapture();
            }
        }
    }
}