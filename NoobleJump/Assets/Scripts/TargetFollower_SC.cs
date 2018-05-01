using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Следит за целью
/// </summary>
public class TargetFollower_SC : MonoBehaviour
{
    public Transform targetTransform;
    public bool followXAxis = true;
    public bool followYAxis = true;
    public bool followZAxis = false;

    private Vector3 deltaPos;

    void Start()
    {
        // Запоминает начальный отступ по всем осям, по которым будет синхронизация
        deltaPos = Vector3.zero;
        if (followXAxis)
            deltaPos += Vector3.right * (transform.position.x - targetTransform.position.x);
        if (followYAxis)
            deltaPos += Vector3.up * (transform.position.y - targetTransform.position.y);
        if (followZAxis)
            deltaPos += Vector3.forward * (transform.position.z - targetTransform.position.z);
    }

    void LateUpdate()
    {
        float _x = followXAxis ? targetTransform.position.x + deltaPos.x : transform.position.x;
        float _y = followYAxis ? targetTransform.position.y + deltaPos.y : transform.position.y;
        float _z = followZAxis ? targetTransform.position.z + deltaPos.z : transform.position.z;
        transform.position = new Vector3(_x, _y, _z);
    }
}
