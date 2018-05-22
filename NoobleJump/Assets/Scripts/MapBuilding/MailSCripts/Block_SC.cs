using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
///     То из чего строится карта
/// </summary>
#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class Block_SC : MonoBehaviour
{
    private const float horisontalMapSize = 35.5f;

    public Transform horShifter;

    [Header("Для следующего блока")]
    public Transform positionForCreatePoint;

    [Header("Края блока (влияют на возможность горизонтального сдвига)")]
    public Transform leftEnd;
    public Transform rightEnd;

    [Header("Связующие объекты")]
    public GameObject inObject;
    public GameObject outObject;

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

        // Если выходная платформа односторонняя, то корректировка leftGap и rightGap
        if (outPlatform)
            if (outPlatform.accessibility != PlatformAccessibility.free)
            {
                if (outPlatform.accessibility == PlatformAccessibility.rightOnly)
                {
                    float needForCorrectRightGeneration = 
                        horisontalMapSize - 
                        (transform.InverseTransformPoint(outPlatform.rightEnd.position).x + 
                         MapBuilder_SC.MIN_SHIFT_FOR_BLOCK_WITH_SINGLE_SIDE_PLATFORM);
                    rightGap = Mathf.Min(rightGap, needForCorrectRightGeneration);
                }
                else if (outPlatform.accessibility == PlatformAccessibility.leftOnly)
                {
                    float needForCorrectLeftGeneration =
                        (transform.InverseTransformPoint(outPlatform.rightEnd.position).x
                        - 0 // Левый край карты всегда в нуле
                        - MapBuilder_SC.MIN_SHIFT_FOR_BLOCK_WITH_SINGLE_SIDE_PLATFORM);
                    leftGap = Mathf.Min(leftGap, needForCorrectLeftGeneration);
                }
            }

        // Если входная платформа односторонняя,
        // то это не накладывает дополнительных ограничений при генерации
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
