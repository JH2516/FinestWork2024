using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.Rendering;

public class BackgroundCapture : MonoBehaviour
{
    Camera gameCamera;
    RenderTexture renderTexture; // RenderTexture
    public Canvas captureCanvas;
    public Image[] targetImage;           // ��������Ʈ�� ������ Image ������Ʈ
    RectTransform[] targetRect;
    float originWidth = 16.0f / 9;
    float originHeight = 9.0f / 16;
    int texWidth;
    int texHeight;
    bool isRepeat = false;
    bool allowOutput = false;
    int actionID = 0;
    AfterEventInvoker[] afterAction;
    MaterialManager materialManager;

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
        materialManager = GetComponent<MaterialManager>();
    }

    void CaptureBackground()
    {
        RenderTexture.active = renderTexture;
        Texture2D texture2D = new Texture2D(texWidth, texHeight, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, texWidth, texHeight), 0, 0);
        texture2D.Apply();
        texture2D.wrapMode = TextureWrapMode.Clamp;

        Color centerColor = texture2D.GetPixel(texWidth / 2, texHeight / 2);

        Debug.Log("Center Color: " + centerColor);

        if (centerColor.r != centerColor.g || centerColor.g != centerColor.b || centerColor.b != centerColor.r)
        {
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            allowOutput = true;
            targetImage[actionID].sprite = sprite;
        }
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
        float screenWidth = (float)Screen.width / Screen.height;
        if (screenWidth >= originWidth)
        {
            renderTexture = new RenderTexture(Mathf.FloorToInt(640 * screenWidth / originWidth), 360, 24);
            renderTexture.Create();
            targetRect[actionID].sizeDelta = new Vector2(1920 * screenWidth / originWidth, 1080);
        }
        else
        {
            float screenHeight = (float)Screen.height / Screen.width;
            renderTexture = new RenderTexture(640, Mathf.FloorToInt(360 * screenHeight / originHeight), 24);
            renderTexture.Create();
            targetRect[actionID].sizeDelta = new Vector2(1920, 1080 * screenHeight / originHeight);
        }
        allowOutput = false;
        materialManager.OnMaterialWithTag("Copy");
        gameCamera.targetTexture = renderTexture;
        
        texWidth = renderTexture.width;
        texHeight = renderTexture.height;
        isRepeat = true;
    }

    private void Update()
    {
        if (isRepeat)
        {
            CaptureBackground();
            if (allowOutput)
            {
                materialManager.OffMaterialWithTag("Copy");
                isRepeat = false;
                gameCamera.targetTexture = null;
                renderTexture = null;
                afterAction[actionID].AfterBackCapture();
            }
        }
    }
}