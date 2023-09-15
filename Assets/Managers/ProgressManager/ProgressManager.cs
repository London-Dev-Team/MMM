using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class ProgressManager : MonoBehaviour
{
    [SerializeField]
    private bool[] unlockedActs = { true, false, false, false, false, false };
    
    [SerializeField]
    private List<MechComponent> componentList = new List<MechComponent>();

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
        SaveUnlockProgress();
        SaveActProgress();
        Debug.Log("Game data saved!");
    }

    private void SaveUnlockProgress()
    {
        String sUnlockedActs = "";

        foreach (bool value in unlockedActs)
        {
            sUnlockedActs += value + " ";
        }

        PlayerPrefs.SetString("UnlockedActs", sUnlockedActs);
        PlayerPrefs.Save();
    }

    private void SaveActProgress()
    {
        foreach (MechComponent component in componentList)
        {
            component.SaveProperties();
        }
    }
    
    private void LoadProgress()
    {
        LoadUnlockProgress();
        LoadActProgress();
        Debug.Log("Game data loaded!");
    }

    private void LoadUnlockProgress()
    {
        if (PlayerPrefs.HasKey("UnlockedActs"))
        {
            String sUnlockedActs = PlayerPrefs.GetString("UnlockedActs");

            String[] unlockedActsArray = sUnlockedActs.Split(" ");

            for (int i = 0; i < unlockedActsArray.Length - 1; i++)
            {
                unlockedActs[i] = unlockedActsArray[i] == "True";
            }
        }
        else
            Debug.LogError("There is no unlock save data!");
    }
    
    private void LoadActProgress()
    {
        foreach (MechComponent component in componentList)
        {
            component.LoadProperties();
        }
    }
    
    void ResetAllProgress()
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
        if (GUI.Button(new Rect(transform.position.x, transform.position.y, 125, 50), "Save Your Game"))
            SaveProgress();
        if (GUI.Button(new Rect(transform.position.x, transform.position.y + 60, 125, 50),
                    "Load Your Game"))
            LoadProgress();
        if (GUI.Button(new Rect(transform.position.x, transform.position.y + 120, 125, 50),
                    "Reset Save Data"))
            ResetAllProgress();
    }

}
