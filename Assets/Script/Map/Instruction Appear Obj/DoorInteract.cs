using UnityEngine;

public class DoorInteract : InstructionAppearObj, SignalReceiverInterface
{
    [Header("References")]
    [SerializeField] protected Animator animator;

    //Instruction
    protected string keyRequiredInstruction = "Key Required";

    //State
    protected bool openable = false;
    protected bool sceneLoading = false;

    [Header("Scene")]
    [SerializeField] protected string sceneName;


    protected override void LoadReferences()
    {
        base.LoadReferences();
        //Animator
        this.animator = GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find animator for Door of " + gameObject.name);
        //SceneName
        if (this.sceneName == "") Debug.LogError("Need scene name for DoorInteract");
    }

    protected override void Update()
    {
        base.Update();
        if (!base.interactable) return;
        if (!base.interacted) return;

        if(!this.openable)
        {
            base.ShowInstruction(this.keyRequiredInstruction, true);
            base.ResetInteract();
            return;
        }

        if (!this.sceneLoading)
        {
            this.sceneLoading = true;
            SceneLoader.Instance.LoadScene(this.sceneName, SceneLoader.LoadingSceneType.ChangeLevel);
        }
    }

    //SignalReceiverInterface
    public void ReceiveSignal(bool signal)
    {
        this.openable = signal;
        this.animator.SetBool("doorOpen", true);
    }
}
