using UnityEngine;
using System.Collections;

public class Platform_SC : MonoBehaviour
{
    public Transform leftEnd;
    public Transform rightEnd;

    public float jumpCoeff = 1f;

    public virtual void OnPlayerJumpOnPlatform()
    {
    }
}
