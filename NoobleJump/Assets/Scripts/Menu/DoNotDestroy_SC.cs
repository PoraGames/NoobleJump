using UnityEngine;
using System.Collections;

public class DoNotDestroy_SC : MonoBehaviour
{
    public GameObject[] immortaleObjects;

    void Awake()
    {
        foreach (GameObject obj in immortaleObjects)
        {
            DontDestroyOnLoad(obj);
        }
    }
}
