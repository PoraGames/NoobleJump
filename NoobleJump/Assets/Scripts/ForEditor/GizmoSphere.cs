using UnityEngine;
using System.Collections;

public class GizmoSphere : MonoBehaviour
{
    public float size = 0f;
    public Color color = Color.yellow;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, size);
    }
}
