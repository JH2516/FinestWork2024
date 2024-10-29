using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListScroll : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool isX;
    public float startLimit;
    public float endLimit;
    float preCoordValue;
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
                UpdateNewPosition();
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
            preCoordValue = isX ? localPoint.x : localPoint.y;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out Vector2 localPoint))
        {
            accel = isX ? localPoint.x - preCoordValue : localPoint.y - preCoordValue;
            UpdateNewPosition();
            preCoordValue = isX ? localPoint.x : localPoint.y;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        accelPositive = accel > 0;
        naturalMove = true;
    }

    private void UpdateNewPosition()
    {
        Vector2 newPosition = rect.localPosition;
        float delta = accel * dragSpeed;
        if (isX)
        {
            newPosition.x = Mathf.Clamp(newPosition.x + delta, startLimit, endLimit);
        }
        else
        {
            newPosition.y = Mathf.Clamp(newPosition.y + delta, startLimit, endLimit);
        }

        rect.localPosition = newPosition;
    }
}
