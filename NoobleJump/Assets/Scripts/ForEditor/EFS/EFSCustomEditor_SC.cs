#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EFS_SC))]
public class EFSCustomEditor_SC : Editor
{
    public override void OnInspectorGUI()
    {
        // Все что было до изменений
        DrawDefaultInspector();

        // Получение скрипта объекта для управления
        EFS_SC myScript = (EFS_SC)target;

        // Кнопка для вызова метода из инспектора
        if (GUILayout.Button("Generate id"))
        {
            myScript.GenerateId();
        }
    }
}
#endif
