using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Init : MonoBehaviour
{
    /// <summary> 이동 벡터 모음 불러오기 </summary>
    public static Vector2[] Get_MoveVecs()
    {
        Vector2[] vecs = new Vector2[9];

        vecs[0] = new Vector2(-1, 1);
        vecs[1] = new Vector2(0, 1);
        vecs[2] = new Vector2(1, 1);
        vecs[3] = new Vector2(-1, 0);
        vecs[4] = new Vector2(0, 0);
        vecs[5] = new Vector2(1, 0);
        vecs[6] = new Vector2(-1, -1);
        vecs[7] = new Vector2(0, -1);
        vecs[8] = new Vector2(1, -1);

        return vecs;
    }
}
