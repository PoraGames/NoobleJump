using UnityEngine;
using System.Collections;

public class Platform_SC : MonoBehaviour
{
    public Transform leftEnd;
    public Transform rightEnd;

    public virtual void OnPlayerJumpOnPlatform()
    {
        // Debug.Log("OnPlayerJumpOnPlatform" + name);
    }
}
