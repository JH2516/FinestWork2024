using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractState_Idle : IState
{
    private UIInteract  _ui;

    public UIInteractState_Idle(UIInteract ui)
    {
        _ui = ui;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        _ui.Set_Position();
    }

    public void Exit()
    {

    }
}

public class UIInteractState_Working : IState
{
    private UIInteract  _ui;

    public UIInteractState_Working(UIInteract ui)
    {
        _ui = ui;
    }
    public void Enter()
    {

    }

    public void Update()
    {
        _ui.Set_Position();
        _ui.guage.fillAmount += Time.deltaTime * _ui.amount_Up;
        if (_ui.isRecoveryHP) _ui.stageManager.Player_RemoteHP(_ui.guage.fillAmount);
    }

    public void Exit()
    {

    }
}

public class UIInteractState_Done : IState
{
    private UIInteract _ui;

    public UIInteractState_Done(UIInteract ui)
    {
        _ui = ui;
    }
    public void Enter()
    {

    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}
