using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Расширение класса GameObject
/// </summary>
public static class GameObjectExp_SC
{
    /// <summary> Вызов SetActive(activeState) у всех элементов </summary>
    public static void SetActive(this GameObject[] gameObjects, bool activeState)
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(activeState);
        }
    }
}
