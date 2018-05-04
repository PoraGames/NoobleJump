#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class EFSChanger_SC : MonoBehaviour
{
    [Header("Поставить галку для выполнения")]
    public bool needCheck = false;

    public GameObject[] etalons;

    public GameObject[] objectsForCheck;

    void Update()
    {
        if (!needCheck)
            return;

        // Перенос всех образцов в словарь
        Dictionary<int, EFS_SC> etalonsDic = new Dictionary<int, EFS_SC>();
        foreach (GameObject obj in etalons)
        {
            EFS_SC efs = obj.GetComponent<EFS_SC>() ?? obj.GetComponentInChildren<EFS_SC>();
            if (efs)
                etalonsDic.Add(efs.id, efs);
            else
                Debug.LogError("error in etalons : " + obj.name);
        }

        // Замена объектов во всех группах
        foreach (GameObject objBock in objectsForCheck)
        {
            foreach (EFS_SC efs in objBock.GetComponentsInChildren<EFS_SC>())
            {
                if (etalonsDic.ContainsKey(efs.id))
                {
                    GameObject newObject = Instantiate(etalonsDic[efs.id].gameObject,
                        efs.transform.position, efs.transform.rotation);
                    newObject.transform.SetParent(efs.transform.parent);

                    DestroyImmediate(efs.gameObject);
                    Debug.Log("changed : " + efs.id);
                }
            }
        }

        needCheck = false;
    }
}
#endif
