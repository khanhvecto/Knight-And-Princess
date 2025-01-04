using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    [SerializeField] protected Transform parrySoundContainerObj;
    [SerializeField] protected Transform hurtSoundContainerObj;
    [SerializeField] protected Transform blockSoundContainerObj;
    [SerializeField] protected Transform deadSoundContainerObj;
    [SerializeField] protected Transform swordSlashSoundContainerObj;
    [SerializeField] protected Transform shieldSwingSoundContainerObj;
    [SerializeField] protected Transform landingSoundContainerObj;
    [SerializeField] protected Transform rollSoundContainerObj;
    [SerializeField] protected Transform jumpSoundContainerObj;
    [SerializeField] protected Transform runSoundContainerObj;
    [SerializeField] protected Transform sprintSoundContainerObj;

    [Header("--- SOUND LISTS ---")]
    protected List<AudioSource> parrySoundList = new List<AudioSource>();
    protected List<AudioSource> hurtSoundList = new List<AudioSource>();
    protected List<AudioSource> blockSoundList = new List<AudioSource>();
    protected List<AudioSource> deadSoundList = new List<AudioSource>();
    protected List<AudioSource> swordSlashSoundList = new List<AudioSource>();
    protected List<AudioSource> shieldSwingSoundList = new List<AudioSource>();
    protected List<AudioSource> landingSoundList = new List<AudioSource>();
    protected List<AudioSource> rollSoundList = new List<AudioSource>();
    protected List<AudioSource> jumpSoundList = new List<AudioSource>();
    protected List<AudioSource> runSoundList = new List<AudioSource>();
    protected List<AudioSource> sprintSoundList = new List<AudioSource>();

    [Header("--- STATES ---")]
    [Header("Running")]
    protected bool isPlayingRunSound = false;
    protected AudioSource runSoundPlaying;
    [Header("Sprinting")]
    protected bool isPlayingSprintSound = false;
    protected AudioSource sprintSoundPlaying;
    [Header("Hurt")]
    protected AudioSource hurtSoundPlaying;

    protected void Start()
    {
        this.CheckReferences();
        this.LoadSounds();
    }
    
    protected void CheckReferences()
    {
        // Parry sound container object
        if (this.parrySoundContainerObj == null)
            Debug.LogError("Can't find parry sound container object for PlayerSound");
        // Hurt sound container object
        if (this.hurtSoundContainerObj == null)
            Debug.LogError("Can't find hurt sound container object for PlayerSound");
        // Block sound container object
        if (this.blockSoundContainerObj == null)
            Debug.LogError("Can't find block sound container object for PlayerSound");
        // Dead sound container object
        if (this.deadSoundContainerObj == null)
            Debug.LogError("Can't find dead sound container object for PlayerSound");
        // Sword slash sound container object
        if (this.swordSlashSoundContainerObj == null)
            Debug.LogError("Can't find sword slash sound container object for PlayerSound");
        // Shield swing sound container object
        if (this.shieldSwingSoundContainerObj == null)
            Debug.LogError("Can't find shield swing sound container object for PlayerSound");
        // Landing sound container object
        if (this.landingSoundContainerObj == null)
            Debug.LogError("Can't find landing sound container object for PlayerSound");
        // Roll sound container object
        if (this.rollSoundContainerObj == null)
            Debug.LogError("Can't find roll sound container object for PlayerSound");
        // Jump sound container object
        if (this.jumpSoundContainerObj == null)
            Debug.LogError("Can't find jump sound container object for PlayerSound");
        // Run sound container object
        if (this.runSoundContainerObj == null)
            Debug.LogError("Can't find run sound container object for PlayerSound");
        // Sprint sound container object
        if (this.sprintSoundContainerObj == null)
            Debug.LogError("Can't find sprint sound container object for PlayerSound");
    }

    protected void LoadSounds()
    {
        // parry
        for(int i=0; i<this.parrySoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.parrySoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.parrySoundList.Add(sound);
        }
        // hurt
        for(int i=0; i<this.hurtSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.hurtSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.hurtSoundList.Add(sound);
        }
        // Block
        for(int i=0; i<this.blockSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.blockSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.blockSoundList.Add(sound);
        }
        // Dead
        for(int i=0; i<this.deadSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.deadSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.deadSoundList.Add(sound);
        }
        // Sword slash
        for(int i=0; i<this.swordSlashSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.swordSlashSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.swordSlashSoundList.Add(sound);
        }
        // Shield swing
        for(int i=0; i<this.shieldSwingSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.shieldSwingSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.shieldSwingSoundList.Add(sound);
        }
        // Landing
        for(int i=0; i<this.landingSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.landingSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.landingSoundList.Add(sound);
        }
        // Roll
        for(int i=0; i<this.rollSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.rollSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.rollSoundList.Add(sound);
        }
        // Jump
        for(int i=0; i<this.jumpSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.jumpSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.jumpSoundList.Add(sound);
        }
        // Run
        for(int i=0; i<this.runSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.runSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.runSoundList.Add(sound);
        }
        // Sprint
        for(int i=0; i<this.sprintSoundContainerObj.childCount; i++)
        {
            AudioSource sound = this.sprintSoundContainerObj.GetChild(i).GetComponent<AudioSource>();
            this.sprintSoundList.Add(sound);
        }
    }

    public void PlayRandomParrySound()
    {
        int rand = Random.Range(0, this.parrySoundList.Count);
        this.parrySoundList[rand].Play();
    }
    
    public void PlayRandomHurtSound()
    {
        if (this.hurtSoundPlaying != null && this.hurtSoundPlaying.isPlaying)
            return;

        int rand = Random.Range(0, this.hurtSoundList.Count);
        this.hurtSoundList[rand].Play();
        this.hurtSoundPlaying = this.hurtSoundList[rand];
    }
    
    public void PlayRandomBlockSound()
    {
        int rand = Random.Range(0, this.blockSoundList.Count);
        this.blockSoundList[rand].Play();
    }
    
    public void PlayRandomDeadSound()
    {
        int rand = Random.Range(0, this.deadSoundList.Count);
        this.deadSoundList[rand].Play();
    }
    
    public void PlayRandomSwordSlashSound()
    {
        int rand = Random.Range(0, this.swordSlashSoundList.Count);
        this.swordSlashSoundList[rand].Play();
    }
    
    public void PlayRandomShieldSwingSound()
    {
        int rand = Random.Range(0, this.shieldSwingSoundList.Count);
        this.shieldSwingSoundList[rand].Play();
    }
    
    public void PlayRandomLandingSound()
    {
        int rand = Random.Range(0, this.landingSoundList.Count);
        this.landingSoundList[rand].Play();
    }
    
    public void PlayRandomRollSound()
    {
        int rand = Random.Range(0, this.rollSoundList.Count);
        this.rollSoundList[rand].Play();
    }
    
    public void PlayRandomJumpSound()
    {
        int rand = Random.Range(0, this.jumpSoundList.Count);
        this.jumpSoundList[rand].Play();
    }

    #region Run and sprint

    public void PlayRandomRunSound()
    {
        if (this.isPlayingRunSound)
            return;

        this.StopSprintSound();

        int rand = Random.Range(0, this.runSoundList.Count);
        this.runSoundList[rand].Play();
        this.runSoundPlaying = this.runSoundList[rand];
        this.isPlayingRunSound = true;
    }
    
    public void StopRunSound()
    {
        if (this.isPlayingRunSound)
        {
            this.runSoundPlaying.Stop();
            this.isPlayingRunSound = false;
        }
    }

    public void PlayRandomSprintSound()
    {
        if (this.isPlayingSprintSound)
            return;

        this.StopRunSound();

        int rand = Random.Range(0, this.sprintSoundList.Count);
        this.sprintSoundList[rand].Play();
        this.sprintSoundPlaying = this.sprintSoundList[rand];
        this.isPlayingSprintSound = true;
    }

    public void StopSprintSound()
    {
        if (this.isPlayingSprintSound)
        {
            this.sprintSoundPlaying.Stop();
            this.isPlayingSprintSound = false;
        }
    }

    #endregion
}
