using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Generator_Map_AreaPos : MonoBehaviour
{
    public Generator_Map        tool_Map;
    public Vector3              currenPos;



    // 에디터에서 오브젝트 위치가 변경되면 호출되는 메서드
    private void OnValidate()
    {
        currenPos = transform.position;
    }

    private void Update()
    {
        currenPos = transform.position;
        tool_Map.SetTransform_WallAreaPos(transform);


        Debug.Log(Application.isPlaying);
        Debug.Log(currenPos);
        Debug.Log($"{tool_Map.areaCoordX}, {tool_Map.areaCoordY}");
    }
}
#endif
