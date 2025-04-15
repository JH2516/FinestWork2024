using System;
using System.Collections.Generic;
using UnityEngine;

public enum InteractorType
{
    BackDraft, Collapse, CollaspeRoom, Recovery, Survivor
}

[CreateAssetMenu]
public class SO_Interactor : ScriptableObject
{
    [SerializeField]
    private UIInteract _ui_BackDraft;
    [SerializeField]
    private UIInteract _ui_Collapse;
    [SerializeField]
    private UIInteract _ui_CollapseRoom;
    [SerializeField]
    private UIInteract _ui_Recovery;
    [SerializeField]
    private UIInteract _ui_Survivor;

    public Dictionary<InteractorType, UIInteract>   UIInteractList;
    public Dictionary<InteractorType, string>       UIInteractNameList;





    //-----------------< Initialize. 초기화 모음 >-----------------//

    /// <summary>
    /// Interactor 공용 데이터 초기화
    /// </summary>
    private void Init()
    {
        UIInteractList = new Dictionary<InteractorType, UIInteract>
        {
            { InteractorType.BackDraft,     _ui_BackDraft},
            { InteractorType.Collapse,      _ui_Collapse},
            { InteractorType.CollaspeRoom,  _ui_CollapseRoom},
            { InteractorType.Recovery,      _ui_Recovery },
            { InteractorType.Survivor,      _ui_Survivor},
        };

        UIInteractNameList = new Dictionary<InteractorType, string>
        {
            { InteractorType.BackDraft,     "UIInteract_BackDraft"},
            { InteractorType.Collapse,      "UIInteract_Collapse"},
            { InteractorType.CollaspeRoom,  "UIInteract_CollapseRoom"},
            { InteractorType.Recovery,      "UIInteract_Recovery"},
            { InteractorType.Survivor,      "UIInteract_Survivor"},
        };
    }

    /// <summary>
    /// Interactor 종류 별 대상 UIInteract 가져오기
    /// </summary>
    /// <param name="type"> Interactor 타입 </param>
    /// <returns> 대상 UIInteract 반환 </returns>
    public UIInteract GetUIInteract(InteractorType type)
    {
        if (UIInteractList == null || UIInteractNameList == null) Init();

        UIInteract ui = null;
        if (!UIInteractList.TryGetValue(type, out ui))
            ui = Resources.Load<UIInteract>($"Prefab/{UIInteractNameList[type]}");

        if (ui == null)
            Debug.Log($"{type}에 해당하는 UIInteract를 찾을 수 없습니다.");

        return ui;
    }
}
