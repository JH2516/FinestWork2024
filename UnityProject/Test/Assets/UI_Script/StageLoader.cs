using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    /// <summary>
    /// �������� �ҷ��� �� �����ϴ� ����, ������ ���� ���� ��
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
