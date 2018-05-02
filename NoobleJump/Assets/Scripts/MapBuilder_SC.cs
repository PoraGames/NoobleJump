using UnityEngine;
using System.Collections;

public class MapBuilder_SC : MonoBehaviour
{
    public Transform reactPoint;
    public Transform createPoint;
    public Transform playerPoint;

    public Block_SC[] blocksForBuild;

    private void Update()
    {
        if (CheckTheNeedToCreate())
            CreateNewBlock();
    }

    bool CheckTheNeedToCreate()
    {
        return playerPoint.position.y >= reactPoint.position.y;
    }

    void CreateNewBlock()
    {
        // Выбор блока для создания
        GameObject _generatedObj = GenerateBlockForCreating().gameObject;

        // Создание
        GameObject _createdObj = Instantiate(_generatedObj, createPoint.position, Quaternion.identity);
        Block_SC _createdBlock = _createdObj.GetComponent<Block_SC>();

        // Перемещение точек создания и ожидания
        reactPoint.position += Vector3.up * (_createdBlock.positionForCreatePoint.position.y - createPoint.position.y);
        createPoint.position = _createdBlock.positionForCreatePoint.position;
    }

    /// <summary>
    /// Выбор блока для создания
    /// <para> Все проверки идут внутри </para>
    /// </summary>
    /// <returns> Уже готовый к созданию блок </returns>
    Block_SC GenerateBlockForCreating()
    {
        Block_SC _block = blocksForBuild[Random.Range(0, blocksForBuild.Length)];

        // TODO: место для проверок

        return _block;
    }
}
