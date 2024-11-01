using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInteract : MonoBehaviour
{
    protected GameObject obj_Interactor;
    protected Vector2 pos_Interact;
    protected RectTransform rectTransform;

    [SerializeField]
    protected Image guage;
    [SerializeField]
    private TextMeshProUGUI text;
    protected bool getRequested;
    protected float amount_Up;

    public float time_Interact;

    public void Set_Interactor(GameObject obj) => obj_Interactor = obj;
    public void Set_Position(Vector2 pos) => pos_Interact = pos;


    public void Init()
    {
        getRequested = false;
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

    private void LateUpdate()
    {
        Set_Position();

        Gauge_Up();
        Gauge_Check();
    }

    void Set_Position()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos_Interact);
        rectTransform.position = screenPosition;
    }

    private void Gauge_Up()
    {
        if (!getRequested) return;

        guage.fillAmount += Time.deltaTime * amount_Up;
    }

    protected virtual void Gauge_Check()
    {
        if (guage.fillAmount >= 1f)
        obj_Interactor.GetComponent<Interactor>().Done_Interact();
    }

    public virtual void Request_Start()
    {
        getRequested = true;
        guage.gameObject.SetActive(true);
        text.gameObject.SetActive(false);
    }
}
