using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListScroll : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public float yUpLimit;
    public float yDownLimit;
    float preY;
    float accel;
    public float accelSlow;
    public float dragSpeed;
    bool naturalMove;
    bool accelPositive;

    RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (naturalMove)
        {
            float direction = accelPositive ? -1 : 1;
            accel += accelSlow * direction;
            if ((accelPositive && accel > 0) || (!accelPositive && accel < 0))
            {
                rect.localPosition = new Vector2(rect.localPosition.x, Mathf.Clamp(rect.localPosition.y + accel * dragSpeed, yUpLimit, yDownLimit));
            }
            else
            {
                naturalMove = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        naturalMove = false;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out Vector2 localPoint))
        {
            preY = localPoint.y;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out Vector2 localPoint))
        {
            accel = localPoint.y - preY;
            Debug.Log(accel);
            rect.localPosition = new Vector2(rect.localPosition.x, Mathf.Clamp(rect.localPosition.y + accel * dragSpeed, yUpLimit, yDownLimit));
            preY = localPoint.y;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        accelPositive = accel > 0;
        naturalMove = true;
    }    
}
