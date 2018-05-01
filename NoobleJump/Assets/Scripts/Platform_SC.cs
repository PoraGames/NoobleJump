using UnityEngine;
using System.Collections;

public class Platform_SC : MonoBehaviour
{
    public virtual void OnPlayerJumpOnPlatform()
    {
        Debug.Log("OnPlayerJumpOnPlatform" + name);
    }
}
