using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : IState
{
    private Player _player;

    public PlayerState_Idle(Player Player)
    {
        _player = Player;
    }

    public void Enter()
    {
        _player.isCanMove = true;
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}

public class PlayerState_Walk : IState
{
    private Player _player;

    public PlayerState_Walk(Player Player)
    {
        _player = Player;
    }

    public void Enter()
    {
        _player.isMove = true;
    }

    public void Update()
    {
        _player.Player_Rotate();
        _player.transform.Translate(_player.setMoveVec * 3f * Time.deltaTime);
    }

    public void Exit()
    {
        _player.isMove = false;
    }
}

public class PlayerState_Attack : IState
{
    private Player _player;

    public PlayerState_Attack(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.isFire = true;
        _player.isCanMove = false;

        _player.obj_Attack.SetActive(true);
        _player.audio.Extinguising(true);

        _player.StartAttack();
    }

    public void Update()
    {
        _player.time_Fire += Time.deltaTime;

        if (!_player.button_isFireActive && _player.time_Fire > 1f)
            _player.sController_Player.ChangeState
                (_player.isButtonDownMove ? _player.sController_Player.state_Walk : _player.sController_Player.state_Idle);
    }

    public void Exit()
    {
        _player.time_Fire = 0;
        _player.isFire = false;
        _player.isCanMove = true;

        _player.obj_Attack.SetActive(false);
        _player.audio.Extinguising(false);

        _player.EndAttack();
    }
}

public class PlayerState_Dead : IState
{
    private Player _player;

    public PlayerState_Dead(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.isCanMove = false;
        _player.sr.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);

        _player.sController_Player.TurnOffState();
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}