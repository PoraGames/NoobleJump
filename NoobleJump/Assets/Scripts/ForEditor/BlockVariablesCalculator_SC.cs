#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BlockVariablesCalculator_SC : MonoBehaviour
{
    [Header("Поставить галку для выполнения")]
    public bool needCheck = false;

    public Block_SC[] blocksForCalculate;

    void Update()
    {
        if (!needCheck)
            return;

        foreach (Block_SC blockForCalculate in blocksForCalculate)
        {
            Calulate(blockForCalculate);
        }

        needCheck = false;
    }

    void Calulate(Block_SC block)
    {
        float _leftEnd = block.transform.position.x + 9999;// Разброс позиций в блоке не будет превышать и 100, так что эти границы не вызовут ошибок
        float _rightEnd = block.transform.position.x - 9999;// Вычисления буду проводится в глобальных координатах, результат переведем в локальные

        Transform[] transforms = block.GetComponentsInChildren<Transform>();

        foreach (Transform tr in transforms)
        {
            // Объект самого блока не учитывать
            if (tr == block.transform)
                continue;
        }
    }
}
#endif
