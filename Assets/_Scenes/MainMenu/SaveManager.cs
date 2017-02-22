using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// словарь string-int. уровень -- значение уровня
/// 0 -- уровень заблокирован
/// 1 -- уровень открыт, не пройден
/// >= 2 уровень пройден, на после каждого прохождения уровня, его значение в словаре просто будет увеличиваться на 1
/// если >= 2, то следующий за ним =1. 
/// </summary>
public class SaveManager : MonoBehaviour
{

    public GameObject LockIcon;
    public Color[] levelColors;

    bool shake = false;
    int shakeCount = 5;
    GameObject shakeIcon;
    Vector3 shakeIconStartPosition;
    void Start()
    {
        UpdateProgress();
    }

    void UpdateProgress()
    {
        int[] LevelValues = new int[10];
        for (int i = 1; i <= 9; i++)
        {
            LevelValues[i] = PlayerPrefs.GetInt("level" + i.ToString());
        }
        for (int i = 1; i <= 9; i++)
        {
            GameObject go = GameObject.Find("Assembled" + i.ToString());
            if (LevelValues[i] == 0 && i == 1)
            {
                LevelValues[i] = 1;
            }
            if (LevelValues[i] >= 2) // пройден
            {
                if (i < 9) // не последний
                {
                    if (LevelValues[i + 1] == 0) // следующий за ним заблокированный
                    {
                        LevelValues[i + 1] = 1; // становится непройденным
                    }
                }
            }
            if (LevelValues[i] >= 1)
            {
                UnlockLevelObject(go, i);
            }
            else
            {
                LockLevelObject(go);
            }
        }
        for (int i = 1; i <= 9; i++)
        {
            PlayerPrefs.SetInt("level" + i.ToString(), LevelValues[i]);
        }
        PlayerPrefs.Save();
    }
    public void LoadLevel(string level)
    {
        int levelIndex = Int32.Parse(level.Substring(5));
        int levelSaveValue = PlayerPrefs.GetInt(level);
        if (levelSaveValue >= 1)
        {
            GetComponent<MainMenuUI>()._StartLevel(level);
            
        }
        else
        {
            ShakeIcon(levelIndex);
        }
    }
    void LockLevelObject(GameObject go)
    {
        Renderer rend = go.GetComponent<Renderer>();
        rend.material.color = new Color(0, 0, 0);
        Vector3 lockIconPosition = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z - 3);
        GameObject lockIcon = (GameObject)Instantiate(LockIcon, lockIconPosition, Quaternion.identity);
        lockIcon.transform.parent = go.transform;
    }
    void UnlockLevelObject(GameObject go, int levelNumber)
    {
        Renderer rend = go.GetComponent<Renderer>();
        rend.material.color = levelColors[levelNumber - 1];
    }
    public void ResetSaveProgress()
    {
        GameObject[] lockIcons = GameObject.FindGameObjectsWithTag("LockIcon");
        foreach (var go in lockIcons)
        {
            Destroy(go);
        }
        PlayerPrefs.SetInt("level1", 1);
        PlayerPrefs.Save();
        for (int i = 2; i <= 9; i++)
        {
            PlayerPrefs.SetInt("level" + i.ToString(), 0);
            PlayerPrefs.Save();
        }
        UpdateProgress();
    }

    void ShakeIcon(int index)
    {
        shakeIcon = GameObject.Find("Assembled" + index.ToString()).transform.GetChild(0).gameObject;
        shakeIconStartPosition = shakeIcon.transform.position;
        shake = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 1; i <= 9; i++)
            {
                int LevelValue = PlayerPrefs.GetInt("level" + i.ToString());
                Debug.Log("level" + i.ToString() + " - " + LevelValue.ToString());
            }
        }


        float t = 35f, d = 0.2f;
        if (shake)
        {
            shakeIconStartPosition = shakeIcon.transform.parent.position - Vector3.forward * 3f;
            if (shakeCount > 0)
            {
                if (shakeCount % 2 == 0)
                {
                    shakeIcon.transform.position = Vector3.Lerp(shakeIcon.transform.position, shakeIconStartPosition + Vector3.right * d, t * Time.deltaTime);
                    if (Vector3.Distance(shakeIcon.transform.position, shakeIconStartPosition + Vector3.right * d) <= 0.01f)
                    {
                        shakeCount--;
                    }
                }
                else
                {
                    shakeIcon.transform.position = Vector3.Lerp(shakeIcon.transform.position, shakeIconStartPosition + Vector3.left * d, t * Time.deltaTime);
                    if (Vector3.Distance(shakeIcon.transform.position, shakeIconStartPosition + Vector3.left * d) <= 0.01f)
                    {
                        shakeCount--;
                    }
                }
            }
            else
            {
                shakeIcon.transform.position = shakeIconStartPosition;
                shake = false;
                shakeCount = 5;
            }
        }
    }
}
