using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Item : MonoBehaviour, IAfterBlurBack
{
    public GameObject item;
    
    public void AfterBackCapture()
    {
        item.SetActive(true);
        Debug.Log("이후 액션 이벤트 작동됨");
    }
}
