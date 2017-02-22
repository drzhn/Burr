using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Класс для указания возможных позиций для перемещения блоков
/// представляет собой параллелепипед с размерами size.x,size.y,size.z единиц, каждая единица unitSize метров величиной
/// центр относительно мира находится в (0,0,0), а относительно параллелепипеда -- center.x, center.y,center.z
/// для каждой возможной координаты есть булева переменная, указывает находится ли в данной координате блок.
/// </summary>
public class Positions
{
    public List<Vector3> positions;

    Vector3 center, size;
    public Positions(Vector3 size, Vector3 center, int unitSize)
    {
        positions = new List<Vector3>();
        this.center = center;
        this.size = size;

        for (int x = -(int)center.x * unitSize; x < (int)(size.x - center.x) * unitSize; x += unitSize)
        {
            for (int y = -(int)center.y * unitSize; y < (int)(size.y - center.y) * unitSize; y += unitSize)
            {
                for (int z = -(int)center.z * unitSize; z < (int)(size.z - center.z) * unitSize; z += unitSize)
                {
                    Vector3 point = new Vector3(x, y, z);
                    if (point != Vector3.zero) //в центре может находиться несколько блоков
                        //когда в центре находятся все блоки, тогда уровень завершен
                        positions.Add(point);
                }
            }
        }
    }
    // проверка на окончание уровня:
    // когда каждое значение словаря positions будет равно false
    // уровень будет завершен. 
    public bool CheckAssemble()
    {
        bool ret = true;
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject go in blocks)
        {
            ret &= go.transform.position == Vector3.zero;
        }
        return ret;
    }

    public bool ExsistKey(Vector3 key)
    {
        return positions.Contains(key);
    }
    // есть ли объект на этом векторе
    public bool TargetHasObject(Vector3 target)
    {
        if (target == Vector3.zero)
            return false;
        else
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (GameObject go in blocks)
            {
                if (go.transform.position == target)
                    return true;
            }
        }
        return false;
    }
}