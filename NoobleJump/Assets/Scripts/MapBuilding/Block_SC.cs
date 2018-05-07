﻿using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// То из чего строится карта
/// </summary>
[ExecuteInEditMode]
public class Block_SC : MonoBehaviour
{
    private const float horisontalMapSize = 35.5f;

    public Transform horShifter;
    [Header("Для следующего блока")]
    public Transform positionForCreatePoint;
    public RespawnPoint_SC respawnPoint;

    [Header("Края блока (влияют на возможность горизонтального сдвига)")]
    public Transform leftEnd;
    public Transform rightEnd;

    [Header("Связующие объекты")]
    public GameObject inObject;
    public GameObject outObject;

    [Space(40)]
    [Header("Вычисляется в коде")]
    [Space(10)]

    [Header("Связующие платформы")]
    public Platform_SC inPlatform;
    public Platform_SC outPlatform;

    [Header("Боковые зазоры")]
    public float leftGap;
    public float rightGap;

#if UNITY_EDITOR
    void Update()
    {
        FindContactPlatforms();
        CalculateGaps();
    }
#endif

    /// <summary>
    /// Вычислить возможные боковые зазоры для блока
    /// </summary>
    void CalculateGaps()
    {
        // Вычисления производятся в локальных координатах относительно головного трансформа блока
        leftGap = transform.InverseTransformPoint(leftEnd.position).x;
        rightGap = horisontalMapSize - transform.InverseTransformPoint(rightEnd.position).x;
    }

    /// <summary>
    /// Найти платформы в объектах для контакта блоков
    /// </summary>
    void FindContactPlatforms()
    {
        if (inObject && outObject)
        {
            inPlatform = inObject.GetComponentInChildren<Platform_SC>();
            outPlatform = outObject.GetComponentInChildren<Platform_SC>();
        }
        else
        {
            inPlatform = null;
            outPlatform = null;
            Debug.Log("Нужно задать контактные объекты для блока :" + name);
        }
    }
}
