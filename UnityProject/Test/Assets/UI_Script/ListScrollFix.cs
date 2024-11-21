using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListScrollFix : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool isX;
    public float startLimit;
    public float endLimit;
    float preCoordValue;
    float accel;
    public float fixSpeed;
    public float dragSpeed;
    public float[] fixPoint;
    bool fixingMove;
    float[] fixChangePoint;
    float fixDirectPoint;
    bool changePositive;

    RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        fixChangePoint = new float[fixPoint.Length - 1];
        for (int i = 0; i < fixChangePoint.Length; i++)
        {
            fixChangePoint[i] = (fixPoint[i] + fixPoint[i + 1]) / 2;
        }
    }

    private void Update()
    {
        if (fixingMove)
        {
            Vector2 posSaver = rect.localPosition;
            float coord = isX ? posSaver.x : posSaver.y;
            if (isX)
            {
                posSaver.x = Mathf.Clamp(posSaver.x + fixSpeed * (changePositive ? 1 : -1), startLimit, endLimit);
            }
            else
            {
                posSaver.y = Mathf.Clamp(posSaver.y + fixSpeed * (changePositive ? 1 : -1), startLimit, endLimit);
            }
            if (Mathf.Abs(coord - fixDirectPoint) < fixSpeed)
            {
                if (isX)
                {
                    posSaver.x = fixDirectPoint;
                }
                else
                {
                    posSaver.y = fixDirectPoint;
                }
                fixingMove = false;
            }
            rect.localPosition = posSaver;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        fixingMove = false;
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
        accel = 0;
        int returnPoint = 0;
        Vector2 posSaver = rect.localPosition;
        float coord = isX ? posSaver.x : posSaver.y;
        for (int i = 0; i < fixChangePoint.Length; i++)
        {
            if (coord > fixChangePoint[i])
            {
                returnPoint++;
            }
        }
        fixDirectPoint = fixPoint[returnPoint];
        changePositive = fixDirectPoint - coord >= 0;
        fixingMove = true;
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