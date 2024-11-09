using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_PlayerTransform : MonoBehaviour
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

    /// <summary> 회전 모음 불러오기 </summary>
    public static Quaternion[] Get_Rotation()
    {
        Quaternion[] rotate = new Quaternion[9];

        rotate[0] = Quaternion.Euler(0, 0, 45 * 3);
        rotate[1] = Quaternion.Euler(0, 0, 45 * 2);
        rotate[2] = Quaternion.Euler(0, 0, 45 * 1);
        rotate[3] = Quaternion.Euler(0, 0, 45 * 4);
        rotate[4] = Quaternion.Euler(0, 0, 45 * 0);
        rotate[5] = Quaternion.Euler(0, 0, 45 * 0);
        rotate[6] = Quaternion.Euler(0, 0, 45 * 5);
        rotate[7] = Quaternion.Euler(0, 0, 45 * 6);
        rotate[8] = Quaternion.Euler(0, 0, 45 * 7);

        return rotate;
    }
}
