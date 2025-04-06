/* Generator_Map [2024.10.07]
 * 
 * <Description>
 * 기능 : 벽 생성 도구
 * 
 * <Required>
 * 1. Hierarchy 내 "All_Walls" GameObject 존재
 * 2. Resources 폴더 내 "Wall" prefab 존재
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(Generator_Map))]
public class Editor_Generator_Map : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Generator_Map generator = (Generator_Map)target;
        if (GUILayout.Button("Generate Walls"))
        {
            generator.MakeWall();
        }
    }

    private void OnSceneGUI()
    {
        Generator_Map generator = (Generator_Map)target;

        Debug.Log(generator.set_WallAreaPos.transform.position);
    }
}
#endif

struct Wall
{
    public Vector2 wallCoord;
    public Vector2 wallScale;

    public Wall(float coordX, float coordY, float scaleX, float scaleY)
    {
        wallCoord = new Vector2(coordX, coordY);
        wallScale = new Vector2(scaleX, scaleY);
    }
}

public class Generator_Map : MonoBehaviour
{
    [Header("Wall")]
    public Transform all_walls;
    public Transform set_WallAreaPos;
    public GameObject pref_Wall;

    [Header("Area")]
    public Vector2 areaPos;
    Color areaColor = new Color(255, 0, 0, 0.2f);
    Matrix4x4 originalMatrix = Gizmos.matrix;
    Wall[] walls = new Wall[4];

    [Header("Area Transform")]
    public float areaScaleX = 0;
    public float areaScaleY = 0;
    public float areaCoordX = 0, areaCoordY = 0;
    public float areaRotateZ = 0;

    [Header("Wall Scale")]
    public float wallScaleX = 0;
    public float wallScaleY = 0;

    private Vector2 AREA_COORD;

    private Vector2 areaCoord;
    private Vector2 areaScale;

    [Header("Tool Active")]
    public bool isActive = true;

    private void Reset()
    {
        all_walls = GameObject.Find("All_Walls").transform;
        set_WallAreaPos = transform.Find("Set_WallAreaPos");
        pref_Wall = Resources.Load<GameObject>("Wall");

        all_walls.transform.position = Vector3.zero;
        areaPos = (Vector2)set_WallAreaPos.transform.position;

        areaScaleX = 3; areaScaleY = 2;
        areaCoordX = 0; areaCoordY = 0;
        areaRotateZ = 0;

        wallScaleX = 0.4f; wallScaleY = 0.4f;

        isActive = true;
    }

    public void MakeWall()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject wall = Instantiate(pref_Wall, all_walls.transform);
            wall.transform.position = new Vector2(areaCoordX, areaCoordY);
            wall.transform.rotation = Quaternion.Euler(0, 0, areaRotateZ);
            wall.transform.localScale = walls[i].wallScale;
            wall.transform.Translate(walls[i].wallCoord);
            //Debug.Log($"After_{i} : {walls[i].wallCoord}");
        }
    }

    public void SetTransform_WallAreaPos(Transform transform)
    {
        set_WallAreaPos = transform;
        areaCoordX = set_WallAreaPos.position.x;
        areaCoordY = set_WallAreaPos.position.y;
    }



    private void OnDrawGizmos()
    {
        if (!isActive) return;

        walls = new Wall[4];

        areaCoord = new Vector2(areaCoordX, areaCoordY);
        areaScale = new Vector2(areaScaleX, areaScaleY);
        Quaternion areaRotate = Quaternion.Euler(0, 0, areaRotateZ);

        float wallCoordX = (areaScaleX - wallScaleX) / 2;
        float wallCoordY = (areaScaleY - wallScaleY) / 2;

        Gizmos.matrix = Matrix4x4.TRS(areaCoord, areaRotate, Vector3.one);

        Gizmos.color = areaColor;
        Gizmos.DrawCube(Vector2.zero, areaScale);

        walls[0] = new Wall(0, wallCoordY, areaScaleX, wallScaleY);
        walls[1] = new Wall(0, -wallCoordY, areaScaleX, wallScaleY);
        walls[2] = new Wall(-wallCoordX, 0, wallScaleX, areaScaleY);
        walls[3] = new Wall(wallCoordX, 0, wallScaleX, areaScaleY);

        Gizmos.color = Color.yellow;

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawWireCube(walls[i].wallCoord, walls[i].wallScale);
            //Debug.Log($"Before_{i} : {walls[i].wallCoord}");
        }

        Gizmos.matrix = originalMatrix;
    }

    private void OnValidate()
    {
        
    }
}
