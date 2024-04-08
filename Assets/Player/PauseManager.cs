using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas, menu, optionsScreen, controlsScreen;
    const KeyCode PAUSE_KEY = KeyCode.Escape;
    public bool isPaused;
    public static PauseManager pauseManagerInstance;

    void Awake()
    {
        pauseManagerInstance = this;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        menuCanvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        optionsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        menu.SetActive(true);
        menuCanvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void OpenMenu()
    {
        optionsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        menu.SetActive(true);
    }

    public void OpenOptions()
    {
        menu.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void OpenControls()
    {
        menu.SetActive(false);
        controlsScreen.SetActive(true);
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void ReturnToMenu()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}