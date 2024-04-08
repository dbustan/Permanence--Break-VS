using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Scene = UnityEngine.SceneManagement.Scene;

public class SoundManager : MonoBehaviour
{
    //NOTE: Value of scale refers to if the sound source has a predetermined volume and you want it to continue to abide to that
    private float musicVol, soundVol, masterVol;

    private float musicSliderVal, soundSliderVal, masterSliderVal, sensSliderVal;
    const float DEFAULT_SENSITIVITY = 100f;
    private GameObject backgroundMusicGameObj;
    private AudioSource backgroundMusic;

    private float originalMusicVol, originalSoundVol;

    public static SoundManager instance;
    void Start()
    {
        if (instance == null){
            instance = this;
             DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }

        masterVol = 1;
        musicVol = 1;
        soundVol = 1;

        masterSliderVal = 1;
        musicSliderVal = 1;
        soundSliderVal = 1;
        originalMusicVol = 1;
        originalSoundVol = 1;

        sensSliderVal = DEFAULT_SENSITIVITY;

        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayBackgroundMusic();

        
       
    }

    private void Update(){
        ChangeMasterVol(masterSliderVal);
        ChangeMusicVol(musicSliderVal);
        ChangeSoundVol(soundSliderVal);
    }

    public void playAudio(AudioSource soundSource, string type, float valueToScale = 1)
    {
        if (type == "Sound")
        {
            soundSource.volume = soundVol * masterVol * valueToScale;
            soundSource.Play();
        }
        else if (type == "Music")
        {

            soundSource.volume = originalMusicVol * musicSliderVal * masterVol * valueToScale;
            
            soundSource.Play(); 
            Debug.Log(soundSource.volume);
            
        }
        else
        {
            Debug.Log("Inputted wrong prompt");
        }
    }

    //Useful for audios that may play the same audio more than once.
    public void playAudio(ArrayList soundSources, float valueToScale = 1)
    {
        for (int i = 0; i < soundSources.Count; i++)
        {
            AudioSource audio = (AudioSource)soundSources[i];
            if (audio != audio.isPlaying)
            {
                audio.volume = soundVol;
                audio.Play();
                break;
            }

        }

    }

    public void playAudio(AudioSource[] soundSources, float valueToScale = 1)
    {
        int index = UnityEngine.Random.Range(0, soundSources.Length - 1);
        AudioSource soundToPlay = soundSources[index];
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.volume = soundVol * valueToScale;
            soundToPlay.Play();
        }
        else
        {
            playAudio(soundSources, valueToScale);
        }


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusic();
        
    }



    private void PlayBackgroundMusic()
    {
        backgroundMusicGameObj = GameObject.Find("BackgroundMusic");
        if(backgroundMusicGameObj != null){
            backgroundMusic = backgroundMusicGameObj.GetComponent<AudioSource>();
            playAudio(backgroundMusic, "Music");
        }
        
    }

    public void ChangeMasterVol(float sliderVal)
    {
        masterSliderVal = sliderVal;
        masterVol = sliderVal;
        musicVol = originalMusicVol * masterVol;
        if (backgroundMusic != null){
            backgroundMusic.volume = musicVol;
        }
        
        soundVol = originalSoundVol * masterVol;

    }

     public void ChangeMasterVol(double audioVal)
    {
        masterVol = (float)audioVal;
        musicVol = originalMusicVol * masterVol;
        backgroundMusic.volume = musicVol;
        soundVol = originalSoundVol * masterVol;

    }

    public void ChangeMusicVol(float sliderVal)
    {
        musicSliderVal = sliderVal;
        musicVol = originalMusicVol * musicSliderVal * masterVol;
        //originalMusicVol = musicVol;
        if (backgroundMusic != null){
            backgroundMusic.volume = musicVol;
        }
        
    }

    public void ChangeMusicVol(double audioVal)
    {
        musicVol = (float)audioVal * originalMusicVol  * masterVol;
        //originalMusicVol = musicVol;
        backgroundMusic.volume = musicVol;
    }

    public void ChangeSoundVol(float sliderVal)
    {
        soundSliderVal = sliderVal;
        soundVol = sliderVal * originalSoundVol;
        originalSoundVol = soundVol;
        soundVol = sliderVal;

    }

    public void ChangeSoundVol(double audioVal)
    {
        soundVol = (float)audioVal * originalSoundVol  * masterVol;

    }

    public float GetMasterVol(){
        return masterVol;
    }

    public float GetSoundVol(){
        return soundVol;
    }

    public float GetMusicVol(){
        return musicVol;
    }

    public float GetOriginalMusicVol(){
        return originalMusicVol;
    }

    public float GetOriginalSoundVol(){
        return originalSoundVol;
    }
    public float GetMusicSliderVal()
    {
        return musicSliderVal;
    }

    public float GetMasterSliderVal()
    {
        return masterSliderVal;
    }

    public float GetSoundSliderVal()
    {
        return soundSliderVal;
    }

    public void SetOriginalMusic(float val){
        originalMusicVol = val;
    }

    public void SetOriginalSound(float val){
        originalSoundVol = val;
    }

    public void SetSoundSliderVal(float val){
        soundSliderVal = val;
    }

    public void SetMasterSliderVal(float val){

    }

    public void SetMusicSliderVal(float val){

    }

    public void updateSens(float val) {
        sensSliderVal = val;
    }

    public float getSens() {
        return sensSliderVal;
    }
}
