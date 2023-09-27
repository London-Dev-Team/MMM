using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class ProgressManager : MonoBehaviour
{
    [SerializeField]
    private ActManager actManager;
    
    [SerializeField]
    private ProgressAsset progressAsset;
    
    private List<MechComponent> componentList;

    void Start()
    {
        componentList = actManager.GetComponentList();
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

        foreach (bool value in progressAsset.unlockedActs)
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
        actManager.SaveProperties();
    }
    
    private void LoadProgress()
    {
        LoadUnlockProgress();
        LoadActProgress();
        actManager.LoadProperties();
    }

    private void LoadUnlockProgress()
    {
        if (PlayerPrefs.HasKey("UnlockedActs"))
        {
            String sUnlockedActs = PlayerPrefs.GetString("UnlockedActs");

            String[] progressAssetArray = sUnlockedActs.Split(" ");

            for (int i = 0; i < progressAssetArray.Length - 1; i++)
            {
                progressAsset.unlockedActs[i] = progressAssetArray[i] == "True";
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
        progressAsset.unlockedActs = new bool[6] { true, false, false, false, false, false };
        Debug.Log("Data reset complete");
    }

    public void UnlockAct(int levelIndex)
    {
        if (levelIndex < progressAsset.unlockedActs.Length)
        {
            progressAsset.unlockedActs[levelIndex] = true;
        }
        else
        {
            Debug.LogError("levelIndex " + levelIndex.ToString() + " is out of bounds.");
        }
    }

    public void LockAct(int levelIndex)
    {
        progressAsset.unlockedActs[levelIndex] = false;
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
