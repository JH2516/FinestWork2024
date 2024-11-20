using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

[CustomEditor(typeof(Generator_FireInArea))]
public class ObjectSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 타겟 스크립트를 가져옵니다.
        Generator_FireInArea objectSelector = (Generator_FireInArea)target;

        // 기본 인스펙터를 표시합니다.
        DrawDefaultInspector();

        // 배열이 비어있다면 경고를 표시합니다.
        if (objectSelector.objects == null || objectSelector.objects.Length == 0)
        {
            EditorGUILayout.HelpBox("Objects 배열이 비어 있습니다!", MessageType.Warning);
            return;
        }

        // 배열의 이름을 표시하기 위한 목록 생성
        string[] objectNames = new string[objectSelector.objects.Length];
        for (int i = 0; i < objectSelector.objects.Length; i++)
        {
            objectNames[i] = objectSelector.objects[i] ? objectSelector.objects[i].name : "None";
        }

        // 콤보박스 (Popup) 표시 및 선택
        objectSelector.selectedIndex = EditorGUILayout.Popup("Select Object", objectSelector.selectedIndex, objectNames);

        // 선택된 오브젝트를 업데이트
        if (objectSelector.selectedIndex >= 0 && objectSelector.selectedIndex < objectSelector.objects.Length)
        {
            objectSelector.selectedObject = objectSelector.objects[objectSelector.selectedIndex];
        }
    }
}

public class Generator_FireInArea : MonoBehaviour
{
    public GameObject[] objects; // 선택할 오브젝트 배열
    public int selectedIndex;   // 현재 선택된 인덱스
    public GameObject selectedObject; // 선택된 오브젝트 참조
    public int          count_Generate = 0;

    public  Vector2 areaPosMin;
    public  Vector2 areaPosMax;

    private void Awake()
    {
        Vector2 areaScale = GetComponent<BoxCollider2D>().size;
        areaPosMin = (Vector2)transform.position - areaScale / 2;
        areaPosMax = (Vector2)transform.position + areaScale / 2;

        Generate_Fire();
    }

    private void Generate_Fire()
    {
        for (int i = 0; i < count_Generate; i++)
        {
            Vector2 firePos = new Vector2(
            Random.Range(areaPosMin.x, areaPosMax.x),
            Random.Range(areaPosMin.y, areaPosMax.y));

            Fire fire = Instantiate(selectedObject, transform).GetComponent<Fire>();

            fire.Set_OrderInLayer(i);
            fire.transform.position = firePos;
        }
    }
}
