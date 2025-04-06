using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DebugManager
{
    public void OnDebug(InputAction.CallbackContext context)
    {



        Debug.Log(context.control.path);
        return;
    }
}
