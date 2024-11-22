using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager soundManager;

    [Header("UI/UX Audio")]
    public AudioSource audio_ButtonClick;

    [Header("Player Audio")]
    public AudioSource audio_PlayerWalk;
    public AudioSource audio_Extinguising;
    public AudioSource audio_RemoveCollapse;

    [Header("Item Audio")]
    public AudioSource audio_AlertCollapseAlarm;
    public AudioSource audio_UsePortableLift;

    [Header("Around Audio")]
    public AudioSource audio_TriggerCollapse;
    public AudioSource audio_BurningAround;
   
    [Header("GameOver Audio")]
    public AudioSource audio_GameoverByCollapse;
    public AudioSource audio_GameoverByBackDraft;
    public AudioSource audio_GameoverByLowerOxygen;

    private void Awake()
    {
        if (soundManager != null) soundManager = this;
    }

    private void RemoteAudio(AudioSource source, bool isPlay)
    {
        if (isPlay && !source.isPlaying)    source.Play();
        if (!isPlay)                        source.Stop();
    }

    public void ButtonClick             (bool isPlay) => RemoteAudio(audio_ButtonClick, isPlay);

    public void PlayerWalk              (bool isPlay) => RemoteAudio(audio_PlayerWalk, isPlay);
    public void Extinguising            (bool isPlay) => RemoteAudio(audio_Extinguising, isPlay);
    public void RemoveCollapse          (bool isPlay) => RemoteAudio(audio_RemoveCollapse, isPlay);

    public void AlertCollapseAlarm      (bool isPlay) => RemoteAudio(audio_AlertCollapseAlarm, isPlay);
    public void UsePortableLift         (bool isPlay) => RemoteAudio(audio_UsePortableLift, isPlay);

    public void TriggerCollapse         (bool isPlay) => RemoteAudio(audio_TriggerCollapse, isPlay);
    public void BurningAround           (bool isPlay) => RemoteAudio(audio_BurningAround, isPlay);

    public void GameoverByCollapse      (bool isPlay) => RemoteAudio(audio_GameoverByCollapse, isPlay);
    public void GameoverByBackDraft     (bool isPlay) => RemoteAudio(audio_GameoverByBackDraft, isPlay);
    public void GameoverByLowerOxygen   (bool isPlay) => RemoteAudio(audio_GameoverByLowerOxygen, isPlay);
}
