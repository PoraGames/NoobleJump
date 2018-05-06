using UnityEngine;
using System.Collections;
using Boo.Lang;

public class MapBuilder_SC : MonoBehaviour
{
    [Header("Параметры генерации блоков")]
    /// <summary> Максимальное отдаление краев связывающих блоки платформ </summary>
    public float maxDeltaXGenerate = 4f;
    /// <summary> Стандартный отступ при генерации нового блока относительно out платформы </summary>
    public float deltaYGenerate = 3f;
    [Space(20)]

    public Transform reactPoint;
    public Transform createPoint;
    public Transform playerPoint;

    public Block_SC[] blocksForBuild;

    public Transform rootForAllGeneratedMap;

    /// <summary> Где возродится ели умрет прямо сейчас </summary>
    public Transform currentRespawnPoint;
    /// <summary> Следующие точки респа ([0] всегда ближайшая)</summary>
    public List<Transform> nextRespawnPoints = new List<Transform>();

    public Block_SC currentLastBlock;

    private void Update()
    {
        // За один Update создается только один блок
        // TODO: Можно разделить генерацию + создание + активация блока на несколько частей и вызывать в разных Update
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
        float horShift = 0;

        // Выбор блока для создания
        GameObject _generatedObj = GenerateBlockForCreating(out horShift).gameObject;

        // Создание (дочерним объектом для держателя карты)
        GameObject _createdObj = Instantiate(_generatedObj, createPoint.position, Quaternion.identity, rootForAllGeneratedMap);
        Block_SC _createdBlock = _createdObj.GetComponent<Block_SC>();

        // Сдвиг по горизонтали
        _createdBlock.horShifter.position += Vector3.right * horShift;

        // Запоминаем новый блок как последний
        currentLastBlock = _createdBlock;

        // Перемещение точек создания и ожидания ТОЛЬКО ПО ВЕРТИКАЛИ

        // Если есть своя точка для отступа, то отталкиваемся от нее
        if (_createdBlock.positionForCreatePoint)
        {
            float deltaY = _createdBlock.positionForCreatePoint.position.y - createPoint.position.y;
            reactPoint.position += Vector3.up * deltaY;
            createPoint.position += Vector3.up * deltaY;
        }
        // Если нет, то берем дефолтное значение из настроек
        else
        {
            reactPoint.position += Vector3.up * deltaYGenerate;
            createPoint.position += Vector3.up * deltaYGenerate;
        }

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
    /// <param name="horisontalShift"> сгенерированный сдвиг блока по горизонтали, который нужно будет сделать после создания </param>
    /// <returns> Уже готовый к созданию блок </returns>
    Block_SC GenerateBlockForCreating(out float horisontalShift)
    {
        int changeTryCounter = 0;
        Block_SC _block;
        bool isBlockCorrect = false;

        float outPlatformRightEndLocalPosX;
        float inPlatformLeftEndPosX;
        bool isRightSideCorrect;
        float outPlatformLeftEndLocalPosX;
        float inPlatformRightEndPosX;
        bool isLeftSideCorrect;

        do
        {
            // Достаем случайный блок
            _block = blocksForBuild[Random.Range(0, blocksForBuild.Length)];

            // TODO: другие проверки

            #region Проверка на совместимость связующих блоков погоризонтальным позициям блоков
            // Вычисление локальной позиции по X
            // правого края выходной платформы последнего созданного блока,
            // относительно трансформа последнего созданного блока
            outPlatformRightEndLocalPosX = currentLastBlock.transform
                .InverseTransformPoint(currentLastBlock.outPlatform.rightEnd.transform.position).x;

            // Вычисление локальной позиции по X
            // левого края входной платформы сгенерированного блока-претендента,
            // относительно трансформа блока-претендента 
            inPlatformLeftEndPosX = _block.transform.InverseTransformPoint(_block.inPlatform.leftEnd.position).x;

            // Можно ли вставить справа
            isRightSideCorrect =
                (inPlatformLeftEndPosX + _block.rightGap > outPlatformRightEndLocalPosX &&
                 inPlatformLeftEndPosX - _block.leftGap <= outPlatformRightEndLocalPosX);

            // То же самое (только зеркально) для проверки возможности вставки слева
            outPlatformLeftEndLocalPosX = currentLastBlock.transform
                .InverseTransformPoint(currentLastBlock.outPlatform.leftEnd.transform.position).x;
            inPlatformRightEndPosX = _block.transform.InverseTransformPoint(_block.inPlatform.rightEnd.position).x;

            // Можно ли вставить справа
            isLeftSideCorrect =
                (inPlatformRightEndPosX - _block.leftGap < outPlatformLeftEndLocalPosX &&
                 inPlatformRightEndPosX + _block.rightGap >= outPlatformLeftEndLocalPosX);
            #endregion

            // Во избежании зацикливания
            // TODO: сделать заглушку по умолчанию
            if (++changeTryCounter > 25)
            {
                Debug.LogError("Correct block do not found");
                isLeftSideCorrect = true;
            }
        } while (!(isLeftSideCorrect || isRightSideCorrect));

        //  Далее: Если блок подходит

        #region Выбор стороны генерации и точного сдвига
        // Выбор стороны
        // TODO: Можно добавить процентную вероятность, основанную на отношении размеров диапозона с каждой стороны (сейчас 50 \ 50)
        if (isLeftSideCorrect && isRightSideCorrect)
        {
            isLeftSideCorrect = Random.Range(0, 2) == 1;
            isRightSideCorrect = !isLeftSideCorrect;
        }

        // Выбор сдвига
        if (isLeftSideCorrect)
        {
            float maxLeftShift = (inPlatformRightEndPosX - _block.leftGap) - outPlatformLeftEndLocalPosX;   // Вычисление возможного отступа
            maxLeftShift = maxLeftShift < -maxDeltaXGenerate ? -maxDeltaXGenerate : maxLeftShift;           // Соблюдение боковогого интервала
            float generatedShift = Random.Range(maxLeftShift, 0f);                                          // Генерация отступа

            horisontalShift = outPlatformLeftEndLocalPosX + generatedShift - inPlatformRightEndPosX;        // Сообщить о выбранном отступе
        }
        else
        {
            float maxRightShift = (inPlatformLeftEndPosX + _block.rightGap) - outPlatformRightEndLocalPosX; // Вычисление возможного отступа
            maxRightShift = maxRightShift > maxDeltaXGenerate ? maxDeltaXGenerate : maxRightShift;          // Соблюдение боковогого интервала
            float generatedShift = Random.Range(0f, maxRightShift);                                         // Генерация отступа

            horisontalShift = outPlatformRightEndLocalPosX + generatedShift - inPlatformLeftEndPosX;        // Применение отступа
        }
        #endregion

        return _block;
    }

    public void RespawnPlayer()
    {
        playerPoint.position = currentRespawnPoint.position;// Перемещение игрока
    }
}
