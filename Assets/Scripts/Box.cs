﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Box))]
public class BoxEditor : Editor
{
    //グリッドの幅
    public const float GRID = 0.5f;

    void OnSceneGUI()
    {
        Box box = target as Box;

        //グリッドの色
        Color color = Color.cyan * 0.7f;

        //グリッドの中心座標
        Vector3 orig = Vector3.zero;

        const int num = 10;
        const float size = GRID * num;

        //グリッド描画
        for (int x = -num; x <= num; x++)
        {
            Vector3 pos = orig + Vector3.right * x * GRID;
            Debug.DrawLine(pos + Vector3.up * size, pos + Vector3.down * size, color);
        }
        for (int y = -num; y <= num; y++)
        {
            Vector3 pos = orig + Vector3.up * y * GRID;
            Debug.DrawLine(pos + Vector3.left * size, pos + Vector3.right * size, color);
        }

        //グリッドの位置にそろえる
        Vector3 position = box.transform.position;
        // position.x = Mathf.Floor(position.x / GRID) * GRID;
        int numX = (int)(position.x * 10) / (int)(GRID * 10);
        position.x = (float)numX * GRID;
        //position.y = Mathf.Floor(position.y / GRID) * GRID;
        int numY = (int)(position.y * 10) / (int)(GRID * 10);
        position.y = (float)numY * GRID;
        int numZ = (int)(position.z * 10) / (int)(GRID * 10);
        position.z = (float)numZ * GRID;
        //int i = 0;
        //while (!(position.x >= (float)i*0.3&& position.x < (float)(i+1) * 0.3))
        // {
        //    if (position.x > 0) i++;
        //    else i--;
        //}
        //position.x = i * 0.3f;
        box.transform.position = position;

        //Sceneビュー更新
        EditorUtility.SetDirty(target);
    }

    //フォーカスが外れたときに実行
    void OnDisable()
    {
        //Sceneビュー更新
        EditorUtility.SetDirty(target);
    }
}
#endif //UNITY_EDITOR

public class Box : MonoBehaviour
{
}