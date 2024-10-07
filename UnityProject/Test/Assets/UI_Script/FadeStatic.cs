using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeStatic : MonoBehaviour
{
    static bool isFade = true;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(isFade);
    }

    private void OnDisable()
    {
        isFade = false;
    }

}
