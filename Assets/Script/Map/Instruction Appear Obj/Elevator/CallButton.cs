using UnityEngine;

public class CallButton : InstructionAppearObj
{
    [Header("------ CALL BUTTON ------")]

    [Header("References")]
    [SerializeField] protected Elevator elevator;

    [Header("Position")]
    [SerializeField] Transform defaultPos;

    protected override void Update()
    {
        base.Update();
        this.CheckCallElevator();
    }

    protected void CheckCallElevator()
    {
        if (!base.interactable || !base.interacted)
            return;

        if (!this.elevator.working)
        {
            base.ResetInteract();
            base.ShowInstruction("SignalRequired", true);
            return;
        }

        if(this.elevator.moving)
        {
            this.elevator.newPos = this.defaultPos.position;
            base.ResetInteract();
            base.ShowInstruction("ElevatorComming", true);
            return;
        }

        if(this.elevator.transform.position != this.defaultPos.position)
        {
            this.elevator.newPos = this.defaultPos.position;
            this.elevator.StartMove();
            base.ResetInteract();
            base.ShowInstruction("ElevatorComming", true);
            return;
        }

        base.ResetInteract();
        base.ShowInstruction("ElevatorHereAlready", true);
    }
}
