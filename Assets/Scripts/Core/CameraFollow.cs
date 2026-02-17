using UnityEngine;

/// <summary>
/// Isometric camera that smoothly follows the player.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float SmoothSpeed = 8f;
    public Vector3 Offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (Target == null) return;
        Vector3 desired = Target.position + Offset;
        transform.position = Vector3.Lerp(transform.position, desired, SmoothSpeed * Time.deltaTime);
    }
}
