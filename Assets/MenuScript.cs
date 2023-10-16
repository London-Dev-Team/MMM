using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    private bool isPaused = false;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
        isPaused = false;
        Debug.Log("WAHOOO");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") ){
            if (isPaused){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }
}
