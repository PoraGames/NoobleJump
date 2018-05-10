#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Element for synchronize
/// </summary>
public class EFS_SC : MonoBehaviour
{
    public int id = -1;

    public void GenerateId()
    {
        EFSIdHolder_SC idGeneratorSc = FindObjectOfType<EFSIdHolder_SC>();
        if (idGeneratorSc)
        {
            id = idGeneratorSc.GetEFSId(this);
        }
        else
        {
            Debug.LogError("Получение id возможно только в сцене Forge");
        }
    }
}
#endif
