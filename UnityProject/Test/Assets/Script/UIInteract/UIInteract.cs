using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIInteract : MonoBehaviour
{
    public      StageManager    stageManager;

    protected   GameObject      obj_Interactor;
    protected   Vector2         pos_Interact;
    protected   RectTransform   rectTransform;
    protected   Camera          uiCamera;       // 캔버스에 설정된 카메라
    protected   Canvas          canvas;         // 참조하고 있는 Canvas

    public      Image           guage;
    [SerializeField]
    protected   TextMeshProUGUI text;
    protected   bool            getRequested;
    public      float           amount_Up { get; protected set; }
    public      bool            isRecoveryHP;

    public float time_Interact;

    public void Set_Interactor(GameObject obj) => obj_Interactor = obj;
    public void Set_Position(Vector2 pos) => pos_Interact = pos;


    public void Init()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        getRequested = false;
        isRecoveryHP = false;
        guage.fillAmount = 0;
        amount_Up = 1 / time_Interact;

        guage.gameObject.SetActive(false);
        text.gameObject.SetActive(true);
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void OnEnable()
    {
        Init();
    }

    void Start()
    {
        // RectTransform 초기화
        rectTransform = GetComponent<RectTransform>();

        // Canvas 및 카메라 초기화
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera; // Screen Space - Camera에서 사용하는 카메라
    }

    private void LateUpdate()
    {
        Set_Position();

        Gauge_Up();
        Gauge_Check();
    }

    public void Set_Position()
    {
        ////Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos_Interact);
        //Vector2 screenPos = Camera.main.WorldToViewportPoint(pos_Interact);
        //rectTransform.position = new Vector3(screenPos.x, screenPos.y, 90);

        // 월드 좌표를 스크린 좌표로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos_Interact);

        // 스크린 좌표를 캔버스 좌표로 변환
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPosition,
            uiCamera, // Canvas에 설정된 카메라를 사용
            out canvasPosition
        );

        // RectTransform 위치 갱신
        rectTransform.anchoredPosition = canvasPosition;
    }

    private void Gauge_Up()
    {
        if (!getRequested) return;

        guage.fillAmount += Time.deltaTime * amount_Up;
        if (isRecoveryHP) stageManager.Player_RemoteHP(guage.fillAmount);

    }

    protected virtual void Gauge_Check()
    {
        if (guage.fillAmount >= 1f)
        obj_Interactor.GetComponent<Interactor>().Done_Interact();

        stageManager.Set_RecoveryHP(false);
    }

    public void Modify_GuageAmountUpPerSecond(float amountValue)
    {
        amount_Up = 1 / amountValue;
    }

    public void Request_Start(bool isRecoveryHP = false, float start_Guage = 0)
    {
        getRequested = true;

        this.isRecoveryHP = isRecoveryHP;
        guage.fillAmount = start_Guage / 100;

        stageManager.Set_RecoveryHP(true);

        guage.gameObject.SetActive(true);
        text.gameObject.SetActive(false);
    }
}
