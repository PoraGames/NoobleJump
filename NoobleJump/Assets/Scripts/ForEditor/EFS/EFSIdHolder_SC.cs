#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;

[ExecuteInEditMode]
public class EFSIdHolder_SC : MonoBehaviour
{
    [Header("Руками не трогать !!!")]
    public string[] efsBase;

    void Update()
    {
        // На столько суровая проверка для того чтобы точно не было косяков,
        // ибо они могут вылиться в большие проблемы
        if (SceneManager.GetActiveScene().name != "Forge")
        {
            Debug.LogError("EFSIdHolder_SC не может находиться на этой сцене, EFSIdHolder_SC должен существовать в единственном экземпляре и только на сцене Forge");
            Destroy(gameObject);
        }
    }

    public int GetEFSId(EFS_SC efsObject)
    {
        // Если уже имеем сгенерированное id то вернуть ошибку
        if (efsObject.id >= 0)
        {
            Debug.LogError("Уже иммется id, для принудительной генерации выставить текущий id < 0");
            return efsObject.id;
        }

        string nameForFind = efsObject.transform.gameObject.name;

        // Поиск в базе
        for (int i = 0; i < efsBase.Length; i++)
        {
            if (efsBase[i] == nameForFind)
            {
                Debug.LogError("Уже иммется объект с именем " + nameForFind + ". " +
                               "Переименуйте объект и повторите генерацию если это НЕ тот объект, который уже содержится в базе." +
                               " Или, если почему-то сбился id задайте вручную значение : " + i);
                return -1;
            }
        }

        // Если еще нет такого объекта в базе

        string[] tmp = new string[efsBase.Length + 1];
        for (int i = 0; i < efsBase.Length; i++)
        {
            tmp[i] = efsBase[i];
        }
        tmp[efsBase.Length] = nameForFind;
        int actualId = efsBase.Length;

        efsBase = tmp;

        Debug.Log("элемент " + nameForFind + " добавлен в базу и имеет номер " + actualId);

        return actualId;
    }
}
#endif
