using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowingType
{
    standart = 0,
    catching = 1,
}

/// <summary>
/// Следит за целью
/// </summary>
public class TargetFollower_SC : MonoBehaviour
{
    public Transform targetTransform;
    public bool followXAxis = true;
    public bool followYAxis = true;
    public bool followZAxis = false;
    public FollowingType followingType = FollowingType.standart;

    public Vector3 deltaPos;
    [Header("Дистанция перемещения расцениваемая как телепортация")]
    public float distanseForTeleportReact = 0.2f;

    private Vector3 lastCheckedTargetPosition;

    private void Start()
    {
        lastCheckedTargetPosition = targetTransform.position;// Запоминаем позицию цели
    }

    //void Awake()
    //{
    //    // Запоминает начальный отступ по всем осям, по которым будет синхронизация
    //    deltaPos = Vector3.zero;
    //    if (followXAxis)
    //        deltaPos += Vector3.right * (transform.position.x - targetTransform.position.x);
    //    if (followYAxis)
    //        deltaPos += Vector3.up * (transform.position.y - targetTransform.position.y);
    //    if (followZAxis)
    //        deltaPos += Vector3.forward * (transform.position.z - targetTransform.position.z);
    //}



    void FixedUpdate()
    {
        float _curX = followXAxis ? targetTransform.position.x + deltaPos.x : transform.position.x;
        float _curY = followYAxis ? targetTransform.position.y + deltaPos.y : transform.position.y;
        float _curZ = followZAxis ? targetTransform.position.z + deltaPos.z : transform.position.z;

        if (followingType == FollowingType.catching && !IsTargetTeleported())// Если цель телепортировалась, то в любом моде провести синхронизацию
        {
            if (_curX < transform.position.x)
                _curX = transform.position.x;
            if (_curY < transform.position.y)
                _curY = transform.position.y;
            if (_curZ < transform.position.z)
                _curZ = transform.position.z;
        }

        transform.position = new Vector3(_curX, _curY, _curZ);
    }

    /// <summary>
    /// Проверка на внезапное резкое перемещение цели и корректировка последней позиции
    /// </summary>
    /// <returns></returns>
    bool IsTargetTeleported()
    {
        if (Vector3.Distance(lastCheckedTargetPosition, targetTransform.position) >= distanseForTeleportReact)
        {
            // Обнаружена телепортация
            lastCheckedTargetPosition = targetTransform.position;
            return true;
        }

        // Все ОК, телепортация не обнаружена
        lastCheckedTargetPosition = targetTransform.position;
        return false;
    }
}
