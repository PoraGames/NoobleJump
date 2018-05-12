using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomReplaceOnStart_SC : MonoBehaviour
{
    public Transform[] generateAreaFrame;

    private void OnDrawGizmos()
    {
        if (generateAreaFrame != null && generateAreaFrame.Length > 1)
        {
            foreach (Transform point1 in generateAreaFrame)
            {
                foreach (Transform point2 in generateAreaFrame)
                {
                    Gizmos.DrawLine(point1.position, point2.position);
                }
            }
        }
    }
}
