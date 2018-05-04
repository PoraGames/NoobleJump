using UnityEngine;
using System.Collections;
using Boo.Lang;

public class MapBuilder_SC : MonoBehaviour
{
    public Transform reactPoint;
    public Transform createPoint;
    public Transform playerPoint;

    public Block_SC[] blocksForBuild;

    public Transform rootForAllGeneratedMap;

    /// <summary> Где возродится ели умрет прямо сейчас </summary>
    public Transform currentRespawnPoint;
    /// <summary> Следующие точки респа ([0] всегда ближайшая)</summary>
    public List<Transform> nextRespawnPoints = new List<Transform>();

    private void Update()
    {
        if (CheckTheNeedToCreate())
            CreateNewBlock();

        CheckNextRespawnPoint();
    }

    bool CheckTheNeedToCreate()
    {
        return playerPoint.position.y >= reactPoint.position.y;
    }

    /// <summary>
    /// Проверить дошли ли до следующей точки респауна,
    /// если да -> обновить точку респауна
    /// </summary>
    void CheckNextRespawnPoint()
    {
        if (nextRespawnPoints.Count == 0)
            return;

        if (playerPoint.position.y >= nextRespawnPoints[0].position.y)
        {
            currentRespawnPoint.position = nextRespawnPoints[0].position;// Переместить главную точку респауна
            nextRespawnPoints.RemoveAt(0);// Убрать точку из массива (теперь новая след точка в позиции [0])
        }
    }

    void CreateNewBlock()
    {
        // Выбор блока для создания
        GameObject _generatedObj = GenerateBlockForCreating().gameObject;

        // Создание (дочерним объектом для держателя карты)
        GameObject _createdObj = Instantiate(_generatedObj, createPoint.position, Quaternion.identity, rootForAllGeneratedMap);
        Block_SC _createdBlock = _createdObj.GetComponent<Block_SC>();

        // Перемещение точек создания и ожидания
        float deltaY = _createdBlock.positionForCreatePoint.position.y - createPoint.position.y;
        reactPoint.position += Vector3.up * deltaY;
        createPoint.position += Vector3.up * deltaY;

        // Новая точка респауна, если нужно
        if (_createdBlock.respawnPoints.Length > 0)
        {
            // TODO: Пока берем только одну, потом можно пересмотреть
            nextRespawnPoints.Add(_createdBlock.respawnPoints[0]);
        }
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

    public void RespawnPlayer()
    {
        playerPoint.position = currentRespawnPoint.position;// Перемещение игрока
    }
}
