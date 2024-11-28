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
    /// <summary>
    /// �������� �ҷ��� �� �����ϴ� �����۰�
    /// </summary>
    public static int Item { get; private set; }

    public void SetStageNumber(int stage)
    {
        Stage = stage;
    }

    public void SetItemID(int itemID)
    {
        Item = itemID;
    }

    public void LoadStage()
    {
        Debug.Log("ㅗㅗㅗ");
        SceneManager.LoadScene("Stage_Test");
    }
}
