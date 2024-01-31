using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Singleton")]
    protected static AudioManager instance;
    public static AudioManager Instance { get => instance; }

    [Header("References")]
    [SerializeField] protected AudioSource backGroundMusic;
    [SerializeField] protected AudioClip[] audioClips;

    protected void Awake()
    {
        // Set singleton
        if(AudioManager.Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlayBgm(BgmType bgmType)
    {
        string nameType = bgmType.ToString();
        AudioClip clip = Array.Find(audioClips, x => x.name == nameType);

        if(clip == null)
        {
            Debug.LogError(nameType + " sound not found!");
            return;
        }

        if (backGroundMusic.clip == clip)
            return; 

        this.backGroundMusic.clip = clip;
        this.backGroundMusic.Play();
    }
}

public enum BgmType
{
    Lvl1, Lvl2, Lvl3, 
    Boss, Menu
}