using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState_Idle : IState
{
    private Fire _fire;

    public FireState_Idle(Fire fire)
    {
        _fire = fire;
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

public class FireState_Extinguish : IState
{
    private Fire _fire;

    private float _powerDown;

    public FireState_Extinguish(Fire fire)
    {
        _fire = fire;
        _powerDown = _fire.sO_Fire?.power_Decrease ?? 0;
    }

    public void Enter()
    {
        _fire.isExtinguish = true;
        if (_fire.isBackdraft)
            _fire.sController_Fire.ChangeState(_fire.sController_Fire.state_Dead);
    }

    public void Update()
    {
        _fire.Set_Power(_fire.power - _powerDown * Time.deltaTime);

        if (_fire.power <= 0.1f)
        {
            _fire.sController_Fire.ChangeState(_fire.sController_Fire.state_Dead);
            return;
        }

        _fire.transform.localScale = Vector3.one * _fire.power;
    }

    public void Exit()
    {
        _fire.isExtinguish = false;
    }
}

public class FireState_Restore : IState
{
    private Fire _fire;

    private float _powerUp;

    public FireState_Restore(Fire fire)
    {
        _fire = fire;
        _powerUp = _fire.sO_Fire?.power_Increase ?? 0;
    }

    public void Enter()
    {
        
    }

    public void Update()
    {
        _fire.Set_Power(_fire.power + _powerUp * Time.deltaTime);

        if (_fire.power >= _fire.maxPower)
        {
            _fire.Set_Power(_fire.maxPower);
            _fire.sController_Fire.ChangeState(_fire.sController_Fire.state_Idle);
            return;
        }

        _fire.transform.localScale = Vector3.one * _fire.power;

    }

    public void Exit()
    {
        _fire.transform.localScale = Vector3.one * _fire.maxPower;
    }
}

public class FireState_Dead : IState
{
    Fire _fire;

    public FireState_Dead(Fire fire)
    {
        _fire = fire;
    }

    public void Enter()
    {
        if (!_fire.isBackdraft && !_fire.debug)
            EventManager.instance.TriggerEvent(PlayerEventType.f_Dead, null);

        _fire.EventRemove();
        _fire.gameObject.SetActive(false);

        _fire.sController_Fire.TurnOffState();
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}