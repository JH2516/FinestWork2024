using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider slider_MasterVolume;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        slider_MasterVolume.onValueChanged.AddListener(SetMasterVolume);
        slider_MasterVolume.value = PlayerPrefs.GetFloat("Volume");
    }

    public void SetMasterVolume(float volume)
    {
        Debug.Log(Mathf.Log10(volume) * 20);
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
