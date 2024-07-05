using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource basicClick;
    [SerializeField] AudioSource waterBottle;
    [SerializeField] AudioSource iceCream;
    [SerializeField] AudioSource sunscreen;
    [SerializeField] AudioSource parasolDrop;
    [SerializeField] AudioSource boxClick;

    [SerializeField] AudioSource wrongSound;
    [SerializeField] AudioSource towel;
    [SerializeField] AudioSource burning;
    [SerializeField] AudioSource victory;
    [SerializeField] AudioSource gameOver;
    [SerializeField] AudioSource requestPopup;

    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void PlayTowel()
    {
        towel.Play();
    }

    public void PlayBurning()
    {
        burning.Play();
    }

    public void PlayVictory()
    {
        victory.Play();
    }

    public void PlayGameOver()
    {
        gameOver.Play();
    }

    public void PlayRequestPopup()
    {
        requestPopup.Play();
    }

    public void PlayBasicClick()
    {
        basicClick.Play();
    }

    public void PlayWaterBottle()
    {
        waterBottle.Play();
    }

    public void PlayIcecream()
    {
        iceCream.Play();
    }

    public void PlaySunscreen()
    {
        sunscreen.Play();
    }

    public void PlayParasoldrop()
    {
        parasolDrop.Play();
    }

    public void PlayBoxClick()
    {
        boxClick.Play();
    }

    public void PlayWrongSound()
    {
        wrongSound.Play();
    }
    public void PlayRequestReceiveSound(string itemName)
    {
        switch(itemName)
        {
            case "Icecream":
                PlayIcecream();
                break;
            case "Sunscreen":
                PlaySunscreen();
                break;
            case "Waterbottle":
                PlayWaterBottle();
                break;
            case "Towel":
                PlayTowel();
                break;
        }
    }
}
