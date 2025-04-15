using System.Collections;
using UnityEngine;

public class UIInteract_Recovery : UIInteract
{
    public      StageManager    stageManager;
    public      bool            isRecoveryHP;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    protected override void OnEnable()
    {
        base.OnEnable();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }





    //-----------------< Request. Interact UI 작업 모음 >-----------------//

    public override void Request_Start(float start_Guage = 0)
    {
        getRequested = true;

        guage.fillAmount = start_Guage / 100;

        guage.gameObject.SetActive(true);
        text.gameObject.SetActive(false);

        StartCoroutine(GaugeUpForRecovery());
    }

    public override void Request_Stop()
    {
        getRequested = false;

        guage.gameObject.SetActive(false);
        text.gameObject.SetActive(true);

        StopCoroutine(GaugeUpForRecovery());
    }





    //-----------------< Activity. 활동 모음 >-----------------//

    /// <summary>
    /// 플레이어 체력 회복 게이지 증가
    /// </summary>
    /// <returns></returns>
    private IEnumerator GaugeUpForRecovery()
    {
        WaitForFixedUpdate wf = new WaitForFixedUpdate();

        while (true)
        {
            guage.fillAmount += Time.deltaTime * amount_Up;
            stageManager.SetPlayer_RemoteHP(guage.fillAmount);

            if (guage.fillAmount >= 1f)
            {
                obj_Interactor.GetComponent<Interactor>().Done_Interact();
                yield break;
            }

            yield return wf;
        }
    }
}
