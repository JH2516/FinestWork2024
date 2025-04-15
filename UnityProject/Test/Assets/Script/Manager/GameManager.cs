using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider slider_MasterVolume;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    private void Awake()
    {
        Application.targetFrameRate = 60;
        slider_MasterVolume.onValueChanged.AddListener(Set_MasterVolume);
        slider_MasterVolume.value = PlayerPrefs.GetFloat("Volume");
    }





    //-----------------< Setting. 속성 설정 >-----------------//

    public void Set_MasterVolume(float volume)
    {
        Debug.Log(Mathf.Log10(volume) * 20);
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
