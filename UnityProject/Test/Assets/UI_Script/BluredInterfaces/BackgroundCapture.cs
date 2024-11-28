using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCapture : MonoBehaviour
{
    Camera gameCamera;
    RenderTexture renderTexture; // RenderTexture
    public Canvas captureCanvas;
    public Image[] targetImage;           // ��������Ʈ�� ������ Image ������Ʈ
    RectTransform[] targetRect;
    float originWidth = 16.0f / 9;
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
        targetRect = new RectTransform[targetImage.Length];
        for (int i = 0; i < targetImage.Length; i++)
        {
            targetRect[i] = targetImage[i].gameObject.GetComponent<RectTransform>();
        }
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
        targetImage[actionID].sprite = sprite;
    }

    public void ButtonCapture()
    {
        actionID = 0;
        ReadyToCapure();
    }

    public void ButtonCaptureID(int id)
    {
        actionID = id;
        ReadyToCapure();
    }

    private void ReadyToCapure()
    {
        nowTime = 0;
        float screenWidth = (float)Screen.width / Screen.height;
        renderTexture = new RenderTexture(Mathf.FloorToInt(640 * screenWidth / originWidth), 360, 24);
        renderTexture.Create();
        targetRect[actionID].sizeDelta = new Vector2(1920 * screenWidth / originWidth, 1080);
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
                afterAction[actionID].AfterBackCapture();
            }
        }
    }
}