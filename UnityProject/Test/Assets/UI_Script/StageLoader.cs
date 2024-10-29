using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    /// <summary>
    /// 스테이지 불러올 때 전송하는 변수, 적당히 끌어 쓰면 됨
    /// </summary>
    public static int Stage { get; private set; }

    public void SetStageNumber(int stage)
    {
        Stage = stage;
    }

    public void LoadStage()
    {
        SceneManager.LoadScene("Test");
    }
}
