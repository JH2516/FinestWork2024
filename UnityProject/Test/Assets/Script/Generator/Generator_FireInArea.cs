using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[DefaultExecutionOrder(5)]
[CustomEditor(typeof(Generator_FireInArea))]
public class ObjectSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Generator_FireInArea selector = (Generator_FireInArea)target;

        // 기본 인스펙터 표시
        DrawDefaultInspector();

        if (selector.Fires == null || selector.Fires.Length == 0)
        {
            EditorGUILayout.HelpBox("Fire Object를 ", MessageType.Warning);
            return;
        }

        // 배열의 이름을 표시하기 위한 목록 생성
        string[] objectNames = new string[selector.Fires.Length];
        for (int i = 0; i < selector.Fires.Length; i++)
        {
            objectNames[i] = selector.Fires[i] ? selector.Fires[i].name : "None";
        }

        // 콤보박스 (Popup) 표시 및 선택
        selector.selectedIndex = EditorGUILayout.Popup("Select Object", selector.selectedIndex, objectNames);

        // 선택된 오브젝트를 업데이트
        if (selector.selectedIndex >= 0 && selector.selectedIndex < selector.Fires.Length)
        {
            selector.selectedFire = selector.Fires[selector.selectedIndex];
        }
    }
}
#endif


public class Generator_FireInArea : MonoBehaviour, IEventTrigger
{
    public static Generator_FireInArea instance;

    [Header("Set Fires")]
    public  GameObject[]    Fires;
    public  int             SetCountGen = 0;

    [Header("Select Fire")]
    public  GameObject      selectedFire;
    [HideInInspector]
    public  int             selectedIndex;


    private Vector2 areaPosMin;
    private Vector2 areaPosMax;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    private void Awake()
    {
        if (instance = null) instance = this;

        Vector2 areaScale = GetComponent<BoxCollider2D>().size / 2;
        areaPosMin = (Vector2)transform.position - areaScale;
        areaPosMax = (Vector2)transform.position + areaScale;

        Generate_Fire();
    }





    //-----------------< Generate. 생성 작업 모음 >-----------------//

    /// <summary>
    /// 영역 내 불 생성
    /// </summary>
    public void Generate_Fire()
    {
        for (int i = 0; i < SetCountGen; i++)
        {
            Fire fire = Instantiate(selectedFire, transform).GetComponent<Fire>();
            //fire.Init_Fire();

            Vector2 colliderSize    = fire.GetComponent<CapsuleCollider2D>().size / 2;
            Vector2 colliderOffset  = fire.GetComponent<CapsuleCollider2D>().offset;

            float sizeX = colliderSize.x * fire.transform.localScale.x;
            float sizeY = colliderSize.y * fire.transform.localScale.y;
            float offsetX = colliderOffset.x * fire.transform.localScale.x;
            float offsetY = colliderOffset.y * fire.transform.localScale.y;

            Vector2 firePos = new Vector2(
            Random.Range(areaPosMin.x + sizeX - offsetX, areaPosMax.x - sizeX - offsetX),
            Random.Range(areaPosMin.y + sizeY - offsetY, areaPosMax.y - sizeY - offsetY));


            fire.Set_OrderInLayer(i);
            fire.transform.position = firePos;

            TriggerEvent(PlayerEventType.f_Summon, this);
        }
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public void TriggerEvent(PlayerEventType e_Type, Component sender, object args = null)
    => EventManager.instance.TriggerEvent(e_Type, sender, args);

    public void TriggerEventOneListener(PlayerEventType e_Type, Component sender, object args = null)
    => EventManager.instance.TriggerEventForOneListener(e_Type, sender, args);
}
