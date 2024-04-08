using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] Button Play, Options, Quit;
    [SerializeField] GameObject loadingScreen, configScreen, mainMenuCanvas, mainMenuScreen, optionsScreen, creditsScreen, saveSlotScreen, levelSelectScreen;
    
    [SerializeField] private SaveManager saveManager;
    [SerializeField] SoundManager sm;

    private AudioSource[] audioInMenu, buttonHovers;



    ArrayList buttonHoverList = new ArrayList();

    private AudioSource buttonClick;

    GameObject currentScreen;
    List<GameObject> Dots = new List<GameObject>();
    int curr = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentScreen = mainMenuScreen;
        audioInMenu = GetComponents<AudioSource>();
        audioSetup();

        //GrabLoadingTextObj(loadingScreen.transform);
        //Invoke("SwitchToConfig", 5);
    }

    private void audioSetup()
    {
        buttonHoverList.Add(audioInMenu[0]);
        buttonHoverList.Add(audioInMenu[1]);
        //audioInMenu[1].Play();
        buttonClick = audioInMenu[2];

    }

    private void GrabLoadingTextObj(Transform loadingScreenTransform)
    {
        foreach (Transform child in loadingScreenTransform)
        {
            if (child.tag == "Loading") LoadingEllipsis(child.transform);
        }
    }

    private void LoadingEllipsis(Transform loadingTextObj)
    {
        int count = 1;
        foreach (Transform child in loadingTextObj)
        {
            Dots.Add(child.gameObject);
            Invoke("setDotActive", count);
            count++;
        }
    }

    private void setDotActive()
    {
        Dots[curr].SetActive(true);
        curr++;
    }

    private void SwitchToConfig()
    {
        currentScreen.SetActive(false);
        mainMenuCanvas.SetActive(true);
        currentScreen = configScreen;
    }

    public void OpenSaveScreen()
    {
        currentScreen.SetActive(false);
        saveSlotScreen.SetActive(true);
        currentScreen = saveSlotScreen;
    }

    public void OpenOptions()
    {
        currentScreen.SetActive(false);
        optionsScreen.SetActive(true);
        currentScreen = optionsScreen;
    }

    public void OpenLevelSelect(){
        currentScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
        currentScreen = levelSelectScreen;
    }

    public void OpenCreditsScreen()
    {
        currentScreen.SetActive(false);
        creditsScreen.SetActive(true);
        currentScreen = creditsScreen;
    }

  

    public void OpenMainMenu()
    {
        currentScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        currentScreen = mainMenuScreen;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

     public void LoadLevel2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }

    public void onButtonHover()
    {
        sm.playAudio(buttonHoverList);
    }

    public void onButtonClick()
    {
        sm.playAudio(buttonClick, "Sound");
    }


}
