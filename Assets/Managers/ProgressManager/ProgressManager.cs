using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class ProgressManager : MonoBehaviour
{
    [SerializeField]
    private bool[] unlockedActs = { true, false, false, false, false, false };


    void Start()
    {
        LoadProgress();
    }

    void OnApplicationQuit()
    {
        SaveProgress();
    }

    public void SaveProgress()
    {

        String sUnlockedActs = "";

        foreach (bool value in unlockedActs)
        {
            sUnlockedActs += value + " ";
        }

        PlayerPrefs.SetString("UnlockedActs", sUnlockedActs);
        PlayerPrefs.Save();
        Debug.Log("Game data saved!");
        Debug.Log(sUnlockedActs);
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("UnlockedActs"))
        {
            String sUnlockedActs = PlayerPrefs.GetString("UnlockedActs");

            String[] unlockedActsArray = sUnlockedActs.Split(" ");

            for (int i = 0; i < unlockedActsArray.Length - 1; i++)
            {
                unlockedActs[i] = unlockedActsArray[i] == "True";
            }

            Debug.Log("Game data loaded!");
            Debug.Log(unlockedActs);
        }
        else
            Debug.LogError("There is no save data!");
    }

    void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        unlockedActs = new bool[6] { true, false, false, false, false, false };
        Debug.Log("Data reset complete");
    }

    public void UnlockAct(int levelIndex)
    {
        if (levelIndex < unlockedActs.Length)
        {
            unlockedActs[levelIndex] = true;
        }
        else
        {
            Debug.LogError("levelIndex " + levelIndex.ToString() + " is out of bounds.");
        }
    }

    public void LockAct(int levelIndex)
    {
        unlockedActs[levelIndex] = false;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(750, 0, 125, 50), "Save Your Game"))
            SaveProgress();
        if (GUI.Button(new Rect(750, 100, 125, 50),
                    "Load Your Game"))
            LoadProgress();
        if (GUI.Button(new Rect(750, 200, 125, 50),
                    "Reset Save Data"))
            ResetProgress();
    }

}
