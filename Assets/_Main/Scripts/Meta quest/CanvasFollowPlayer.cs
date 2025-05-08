using UnityEngine;

public class CanvasFollowPlayer : MonoBehaviour
{
    public Transform player;       // Gán player (camera hoặc nhân vật)
    public float distance = 2f;    // Khoảng cách phía trước player
    public float heightOffset = 1.5f; // Độ cao so với player

    void LateUpdate()
    {
        if (player == null) return;

        // Lấy hướng forward theo mặt phẳng XZ
        Vector3 forwardDir = player.forward;
        forwardDir.y = 0;
        forwardDir.Normalize();

        // Tính vị trí mới
        Vector3 targetPosition = player.position + forwardDir * distance;
        targetPosition.y = player.position.y + heightOffset;

        // Cập nhật vị trí và hướng nhìn
        transform.position = targetPosition;
        transform.rotation = Quaternion.LookRotation(forwardDir);
    }
}
