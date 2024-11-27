using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AfterEventInvoker : MonoBehaviour
{
    public string invokerName; // 단순 구분용
    [SerializeField]
    private int id;
    public int Id { get => id; set => id = value; }
    public UnityEvent action;

    public void AfterBackCapture()
    {
        action.Invoke();
    }
}
