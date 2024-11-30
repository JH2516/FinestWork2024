using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCapture : MonoBehaviour
{
    Camera gameCamera;
    RenderTexture renderTexture; // RenderTexture
    public Canvas captureCanvas;
    public Image[] targetImage;           // ��������Ʈ�� ������ Image ������Ʈ
    public float rangeOfCheck = 1.0f;
    public float checkDevitation = 0.4625f;
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
        afterAction = afterAction.OrderBy(x => x.Id).ToArray();
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

        if (CheckTexture(texture2D, CheckRangeCarculate()))
        {
            return;
        }

        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        allowOutput = true;
        targetImage[actionID].sprite = sprite;
    }

    Vector2 CheckRangeCarculate()
    {
        float startX = 0;
        float startY = 0;
        float screenRangeX = 1;
        float screenRangeY = 1;

        float screenWidth = (float)Screen.width / Screen.height;
        if (screenWidth > originWidth)
        {
            float removeWidth = (screenWidth - originWidth) / screenWidth;
            screenRangeX -= removeWidth;
            startX += removeWidth / 2;
        }
        float screenHeight = (float)Screen.height / Screen.width;
        if (screenHeight > originHeight)
        {
            float removeHeight = (screenHeight - originHeight) / screenHeight;
            screenRangeY -= removeHeight;
            startY += removeHeight / 2;
        }

        startX += screenRangeX * (1 - rangeOfCheck) / 2;
        startY += screenRangeY * (1 - rangeOfCheck) / 2;

        return new Vector2 (startX, startY);
    }

    bool CheckTexture(Texture2D texture, Vector2 range)
    {
        Color[] mainPixels = new Color[9];
        float termX = (0.5f - range.x) / 2;
        float termY = (0.5f - range.y) / 2;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                mainPixels[i * 3 + j] = texture.GetPixel(Mathf.FloorToInt(texWidth * (range.x + termX * (i + 1))), Mathf.FloorToInt(texHeight * (range.y + termY * (j + 1))));
            }
        }

        for (int i = 0; i < mainPixels.Length; i++)
        {
            if (ColorCheck(mainPixels[i]))
            {
                return true;
            }
        }
        if (mainPixels[4] == Color.black)
        {
            return true;
        }
        int corrputed = 0;
        for (int i = 0; i < 100; i++)
        {
            int x = Random.Range(Mathf.FloorToInt(texWidth * range.x), Mathf.FloorToInt(texWidth * (1 - range.x)));
            int y = Random.Range(Mathf.FloorToInt(texHeight * range.y), Mathf.FloorToInt(texHeight * (1 - range.y)));
            Color searchPixel = texture.GetPixel(x, y);

            if (ColorCheck(searchPixel))
            {
                corrputed++;
            }
        }
        return corrputed >= 50;
    }

    bool ColorCheck(Color pixel)
    {
        float[] colors = new float[] { pixel.r, pixel.g, pixel.b };
        float avar = colors.Average();
        float variandce = colors.Select(val => (val - avar) * (val - avar)).Average();
        bool condit = Mathf.Sqrt(variandce) > checkDevitation;
        if (condit)
        {
            Debug.Log(pixel.ToString());
        }
        return condit;
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