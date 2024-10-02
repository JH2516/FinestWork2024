using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

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
    public GameObject pref_Wall;

    Color areaColor = new Color(255, 0, 0, 0.2f);
    Matrix4x4 originalMatrix = Gizmos.matrix;
    Wall[] walls = new Wall[4];

    public bool isActive = false;

    public float areaScaleX = 0, areaScaleY = 0;
    public float areaCoordX = 0, areaCoordY = 0;
    public float areaRotateZ = 0;

    public float wallScaleX = 0, wallScaleY = 0;


    public void MakeWall()
    {
        isActive = false;

        for (int i = 0; i < 4; i++)
        {
            GameObject wall = Instantiate(pref_Wall, transform);
            wall.transform.position = new Vector2(areaCoordX, areaCoordY);
            wall.transform.rotation = Quaternion.Euler(0, 0, areaRotateZ);
            wall.transform.localScale = walls[i].wallScale;
            wall.transform.Translate(walls[i].wallCoord);
            Debug.Log($"After_{i} : {walls[i].wallCoord}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!isActive) return;

        walls = new Wall[4];

        Vector2 areaCoord = new Vector2(areaCoordX, areaCoordY);
        Vector2 areaScale = new Vector2(areaScaleX, areaScaleY);
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
            Debug.Log($"Before_{i} : {walls[i].wallCoord}");
        }

        Gizmos.matrix = originalMatrix;
    }
}
