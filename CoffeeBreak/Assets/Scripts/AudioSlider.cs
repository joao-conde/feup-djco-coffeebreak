using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

using static GameManager;
public class AudioSlider : MonoBehaviour{
   
    public Slider musicSlider;
    public Slider sfxSlider;

    private GameManager gameManager;

    public void Start()   {
        gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
        
        musicSlider.onValueChanged.AddListener(delegate {MusicSliderValueChangeCheck(); });
        sfxSlider.onValueChanged.AddListener(delegate {SfxSliderValueChangeCheck(); });

        musicSlider.value = gameManager.musicMultiplier;
        sfxSlider.value = gameManager.sfxMultiplier;
    }

    public void MusicSliderValueChangeCheck()    {
        gameManager.setMusicMultiplier(musicSlider.value);
    }

    public void SfxSliderValueChangeCheck(){
        gameManager.setSFXMultiplier(sfxSlider.value);
    }
}
