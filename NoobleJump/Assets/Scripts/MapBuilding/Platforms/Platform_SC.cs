using UnityEngine;
using System.Collections;

/// <summary>
/// Доступность для игрока (с какой стороны можно зайти)
/// <remarks> Учитывается при генерации блоков </remarks>
/// </summary>
public enum PlatformAccessibility
{
    /// <summary>
    /// Со всех сторон
    /// </summary>
    free,
    rightOnly,
    leftOnly,
}

public class Platform_SC : MonoBehaviour
{
    public PlatformAccessibility accessibility = PlatformAccessibility.free;
    public Transform leftEnd;
    public Transform rightEnd;

    public float jumpCoeff = 1f;

    public virtual void OnPlayerJumpOnPlatform()
    {
    }

    /// <summary>
    /// Проверить совместимость для генерации справа
    /// </summary>
    public bool CheckRightCompatibility(PlatformAccessibility nextPlatformAccessibility)
    {
        if (accessibility == PlatformAccessibility.free || accessibility == PlatformAccessibility.rightOnly)
            return nextPlatformAccessibility == PlatformAccessibility.free ||
                   nextPlatformAccessibility == PlatformAccessibility.leftOnly;

        return false;
    }

    /// <summary>
    /// Проверить совместимость для генерации слева
    /// </summary>
    public bool CheckLeftCompatibility(PlatformAccessibility nextPlatformAccessibility)
    {
        if (accessibility == PlatformAccessibility.free || accessibility == PlatformAccessibility.leftOnly)
            return nextPlatformAccessibility == PlatformAccessibility.free ||
                   nextPlatformAccessibility == PlatformAccessibility.rightOnly;

        return false;
    }
}
