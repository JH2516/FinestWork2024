using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInteract : MonoBehaviour
{
    protected   Interactor      obj_Interactor;
    protected   Vector2         pos_Interact;
    [SerializeField]
    protected   RectTransform   rect;
    protected   RectTransform   rect_canvas;
    protected   Camera          uiCamera;       // 캔버스에 설정된 카메라
    protected   Canvas          canvas;         // 참조하고 있는 Canvas

    public      Image           guage;
    [SerializeField]
    protected   TextMeshProUGUI text;
    protected   bool            getRequested;
    public      float           amount_Up { get; protected set; }

    public float time_Interact;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    protected virtual void OnEnable()
    {
        Init();
    }

    void Start()
    {
        canvas      = GetComponentInParent<Canvas>();
        rect_canvas = canvas.GetComponent<RectTransform>();
        uiCamera    = canvas.worldCamera; // Screen Space - Camera에서 사용하는 카메라

        Set_Position();
    }

    private void FixedUpdate()
    {
        Set_Position();
    }





    //-----------------< Initialize. 초기화 모음 >-----------------//

    /// <summary>
    /// UIInteract 초기화
    /// </summary>
    public void Init()
    {
        getRequested = false;
        guage.fillAmount = 0;
        amount_Up = 1 / time_Interact;

        guage.gameObject.SetActive(false);
        text.gameObject.SetActive(true);
    }





    //-----------------< Request. Interact UI 작업 모음 >-----------------//

    /// <summary>
    /// Interactor 요청 실행
    /// </summary>
    /// <param name="start_Guage"> 활동 게이지 시작 값(0 ~ 1) </param>
    public virtual void Request_Start(float start_Guage = 0)
    {
        getRequested = true;

        guage.fillAmount = start_Guage / 100;

        guage.gameObject.SetActive(true);
        text.gameObject.SetActive(false);

        StartCoroutine(GaugeUp());
    }

    /// <summary>
    /// Interactor 요청 중단
    /// </summary>
    public virtual void Request_Stop()
    {
        getRequested = false;

        guage.gameObject.SetActive(false);
        text.gameObject.SetActive(true);

        StopCoroutine(GaugeUp());
    }





    //-----------------< Activity. 활동 모음 >-----------------//

    /// <summary>
    /// 활동 게이지 증가
    /// </summary>
    private IEnumerator GaugeUp()
    {
        WaitForFixedUpdate wf = new WaitForFixedUpdate();

        while (true)
        {
            guage.fillAmount += Time.deltaTime * amount_Up;

            if (guage.fillAmount >= 1f)
            {
                obj_Interactor.Done_Interact();
                yield break;
            }

            yield return wf;
        }
    }





    //-----------------< Setting. 속성 설정 >-----------------//

    /// <summary>
    /// 연결된 Interactor 설정
    /// </summary>
    /// <param name="obj"> 연결된 Interactor </param>
    public void Set_Interactor(Interactor obj)
        => obj_Interactor = obj;

    /// <summary>
    /// UI 위치 설정
    /// </summary>
    /// <param name="pos"> UI 위치 벡터 값 </param>
    public void Set_Position(Vector2 pos)
        => pos_Interact = pos;

    /// <summary>
    /// UI 위치 설정
    /// </summary>
    public void Set_Position()
    {
        // 월드 좌표를 스크린 좌표로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos_Interact);

        // 스크린 좌표를 캔버스 좌표로 변환
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect_canvas,
            screenPosition,
            uiCamera, // Canvas에 설정된 카메라를 사용
            out canvasPosition
        );

        // RectTransform 위치 갱신
        rect.anchoredPosition = canvasPosition;
    }

    /// <summary>
    /// 초당 활동 게이지 증가량 수정
    /// </summary>
    /// <param name="amountValue"> 활동 실행 시간(초) </param>
    public void Set_GuageAmountUpPerSecond(float amountValue)
    {
        amount_Up = 1 / amountValue;
    }
}
