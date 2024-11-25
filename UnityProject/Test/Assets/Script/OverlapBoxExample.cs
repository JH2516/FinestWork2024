using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapBoxExample : MonoBehaviour
{
    public Vector2 boxSize = new Vector2(6, 3); // 가로 6, 세로 3
    public float offsetDistance = 3f; // 플레이어 중심에서의 거리
    public LayerMask targetLayer; // 감지할 대상 레이어

    void Update()
    {
        // 플레이어 위치
        Vector2 playerPosition = transform.position;

        // 네 방향으로 OverlapBox 생성
        CreateOverlapBox(playerPosition, Vector2.right); // 오른쪽
        CreateOverlapBox(playerPosition, Vector2.up); // 위쪽
        CreateOverlapBox(playerPosition, Vector2.left); // 왼쪽
        CreateOverlapBox(playerPosition, Vector2.down); // 아래쪽
    }

    void CreateOverlapBox(Vector2 origin, Vector2 direction)
    {
        // 방향에 따른 박스 중심 계산
        Vector2 boxCenter = origin + direction * offsetDistance;

        // 박스의 회전 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // OverlapBox 호출
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, targetLayer);

        // 감지된 오브젝트 처리
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log($"Detected {hitCollider.name} in direction {direction}");
        }

        // 디버그용 박스 그리기
        DebugDrawBox(boxCenter, boxSize, angle, Color.red);
    }

    void DebugDrawBox(Vector2 center, Vector2 size, float angle, Color color)
    {
        // 박스의 네 꼭짓점 계산
        Vector2 halfSize = size * 0.5f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Vector3로 변환 후 Quaternion 회전 적용
        Vector2 topRight = (Vector2)(rotation * new Vector3(halfSize.x, halfSize.y, 0)) + center;
        Vector2 topLeft = (Vector2)(rotation * new Vector3(-halfSize.x, halfSize.y, 0)) + center;
        Vector2 bottomRight = (Vector2)(rotation * new Vector3(halfSize.x, -halfSize.y, 0)) + center;
        Vector2 bottomLeft = (Vector2)(rotation * new Vector3(-halfSize.x, -halfSize.y, 0)) + center;

        // 박스 외곽선 그리기
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
    }

}
