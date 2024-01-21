using Unity.VisualScripting;
using UnityEngine;

public abstract class InstructionAppearObj: InteractableObject
{
    [Header("References")]
    [SerializeField] protected Transform instructionStorage;

    protected Transform showingInstruction;

    protected override void LoadReferences()
    {
        base.LoadReferences();
        //Instruction storage
        this.instructionStorage = transform.Find("InstructionStorage");
        if (this.instructionStorage == null) Debug.LogError("Can't find instruction storage for InstructionAppearObj of " + transform.name);
    }

    protected virtual void ShowInstruction(string name, bool state)
    {
        //Set display instruction
        Transform instruction = this.instructionStorage.Find(name);
        if (instruction == null)
        {
            Debug.LogError("Can't find " + name + " instruction for InstructionAppearObj of " + transform.name);
            return;
        }
        instruction.gameObject.SetActive(state);

        //Store showing instruction
        if(state==true) this.showingInstruction = instruction;
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if(this.showingInstruction != null)
        {
            this.showingInstruction.gameObject.SetActive(false);
        }
    }
}