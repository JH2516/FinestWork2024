using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameOverType
{
    BackDraft, CollaspeRoom, LowerOxygen, FailedSaveSurvivor, Debug
}

[CreateAssetMenu]
public class SO_Stage : ScriptableObject
{
    public float[] stageClearTime = { 120, 180, 180, 180, 240 };

    public Dictionary<GameOverType, string> gameOverComments
        = new Dictionary<GameOverType, string>
        {
            { GameOverType.BackDraft,           "순간적인 화염에 휩싸이고 말았습니다." },
            { GameOverType.CollaspeRoom,        "무너지는 방 속에서 붕괴되고 말았습니다." },
            { GameOverType.LowerOxygen,         "산소통이 더 이상 버텨낼 수 없었습니다." },
            { GameOverType.FailedSaveSurvivor,  "누군가 붕괴 속에서 피해를 입게 되었습니다." },
            { GameOverType.Debug,               "ZWxxanJtIDogYWt0ZGpxdHNtc2RrZGx0bXptZmxh" },
        };
}
