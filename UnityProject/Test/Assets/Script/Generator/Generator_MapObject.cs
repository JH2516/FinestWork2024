/* Generator_MapObject [2024.10.09 Ver_1.1]
 * 
 * <Description>
 * 기능 : 맵 내부 오브젝트 배치 및 생성 도구
 * 
 * 이용 방법 :
 * 1. Set_Objects에 오브젝트 등록
 * 2. "오브젝트 불러오기" Button 누르기 (Generator_MapObject 재선택 시 자동 로드)
 * 3. "오브젝트 선택" Box 내부에 오브젝트 선택
 * 4. "오브젝트 생성" Button 누르기
 * 
 * @. 생성된 Object는 "All_MapObjects" 내부에 위치함
 * @. 아래의 단축기로 Object 선택 및 생성 가능
 * 
 * 단축키:
 * A: Object 생성
 * S: 이전 Object 선택
 * D: 다음 Object 선택
 * 
 * <Required>
 * 1. Hierarchy 내 "All_MapObjects" GameObject 존재
 * 2. SpriteRenderer Component 존재
 * 
 * <Exception>
 * - Hierarchy 내 "All_MapObjects" GameObject가 없는 경우
 * - SpriteRenderer Component가 없는 경우
 * 
 * - Set_Objects에 오브젝트 미등록 시 (Null or 일부 누락 시)
 * - 유효하지 않은 Object 선택 시
 * - Object 생성 전 유효하지 않은 Object 선택 시 (예상)
 * - 이 외 (필요 시 보완 예정)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static Get_DateNow;

#if UNITY_EDITOR
using UnityEditor;
#endif



public static class Get_DateNow
{
    public static string DateNow()
    { return DateTime.Now.ToString("HH:mm:ss"); }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Generator_MapObject))]
public class Editor_Generator_MapObject : Editor
{
    private Texture2D[]             icon;
    private int                     type_Icon;

    public void OnEnable()
    {
        Generator_MapObject generator = (Generator_MapObject)target;
        generator.Init();
        icon = generator.GetObjectIcon();
        type_Icon = 0;
    }

    public override void OnInspectorGUI()
    {
        Generator_MapObject generator = (Generator_MapObject)target;
        GUIStyle buttonStyle;

        //////////////////// 오브젝트 선택 Box ////////////////////
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("오브젝트 선택", EditorStyles.boldLabel);

        using (GUILayout.HorizontalScope scope = new GUILayout.HorizontalScope())
        {
            buttonStyle = new GUIStyle(GUI.skin.button)
            { fixedWidth = 50, fixedHeight = 50 };

            if (icon != null)
            {
                for (int i = 0; i < icon.Length; i++)
                    if (GUILayout.Button(icon[i], buttonStyle))
                        type_Icon = generator.Select_MapObject(i);
            }
        }
        GUILayout.EndVertical();

        //////////////////// 오브젝트 불러오기 /////////////////////
        using (GUILayout.HorizontalScope scope = new GUILayout.HorizontalScope())
        {
            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = EditorGUIUtility.currentViewWidth * 0.45f,
                fixedHeight = 30
            };

            if (GUILayout.Button("오브젝트 불러오기", buttonStyle))
            icon = generator.GetObjectIcon();

            if (GUILayout.Button("오브젝트 생성", buttonStyle))
            generator.Make_MapObject(type_Icon);
        }
        

        DrawDefaultInspector(); // 기본 인스펙터 GUI 표시
    }

    public void OnSceneGUI()
    {
        Generator_MapObject generator = (Generator_MapObject)target;
        Event e = Event.current;

        if (e.type == EventType.KeyDown)
        {
            try
            {
                switch (e.keyCode)
                {
                    case KeyCode.A:
                        generator.Make_MapObject(type_Icon); break;
                    case KeyCode.S:
                        type_Icon = (type_Icon == 0) ? icon.Length - 1 : --type_Icon;
                        generator.Select_MapObject(type_Icon); break;
                    case KeyCode.D:
                        type_Icon = (type_Icon == icon.Length - 1) ? 0 : ++type_Icon;
                        generator.Select_MapObject(type_Icon); break;
                }
            }
            catch (NullReferenceException ex)
            {
                Debug.LogError($"오류 : Set Objects를 모두 등록하십시오.  [{DateNow()}]");
            }
            
            e.Use();
        }
    }
}
#endif

public class Generator_MapObject : MonoBehaviour
{
    public GameObject[]         SetObjects;
    private Transform           all_MapObjects;

    private SpriteRenderer      sr;

    public void Reset() => Init();

    public void Init()
    {
        sr = GetComponent<SpriteRenderer>();
        try
        {
            sr.sprite =
            SetObjects[0].GetComponent<SpriteRenderer>().sprite ?? null;
        }
        catch (Exception ex)
        {
            sr.sprite = null;
        }
        
        all_MapObjects = GameObject.Find("All_MapObjects").transform;
    }

    public Texture2D[] GetObjectIcon()
    {
        if (SetObjects.Length == 0) return null;

        Texture2D[] icons = new Texture2D[SetObjects.Length];

        try
        {
            for (int i = 0; i < SetObjects.Length; i++)
            icons[i] = SetObjects[i].GetComponent<SpriteRenderer>().sprite.texture;

            return icons;
        }
        catch (Exception ex)
        {
            Debug.LogError($"오류 : Set Objects를 모두 등록하십시오.  [{DateNow()}]");
        }

        return icons;
    }

    public int Select_MapObject(int type)
    {
        try
        {
            sr.sprite = SetObjects[type].GetComponent<SpriteRenderer>().sprite;
            return type;
        }
        catch (Exception ex)
        {
            Debug.LogError($"오류 : 유효하지 않은 Object입니다. [{DateNow()}]");
        }
        return 0;
    }

    public void Make_MapObject(int type)
    {
        if (SetObjects.Length == 0)
        { Debug.LogError($"오류 : Set Objects를 등록하십시오.  [{DateNow()}]"); return; }

        try
        {
            GameObject obj = Instantiate(SetObjects[type], all_MapObjects);
            obj.transform.position = transform.position;
            obj.transform.localScale = transform.localScale;
            obj.transform.rotation = transform.rotation;
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.LogError($"오류 : 인덱스 오류  [{DateNow()}]");
        }
        catch (Exception ex)
        {
            Debug.LogError($"오류 : 먼저 생성할 Object를 선택하시오.  [{DateNow()}]");
        }
        
    }
}
