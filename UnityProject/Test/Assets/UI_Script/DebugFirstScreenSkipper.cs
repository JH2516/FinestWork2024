using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFirstScreenSkipper : MonoBehaviour
{
    private void Awake()
    {
        FirstScreenSkipper.FirstScreenNoSkip = false;
    }
}
