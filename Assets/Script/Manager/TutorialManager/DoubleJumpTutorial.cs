using UnityEngine;

public class DoubleJumpTutorial : TutorialActivate
{
    [Header("------ DOUBLE JUMP TUTORIAL ------")]

    [Header("References")]
    [SerializeField] protected PlayerStats playerStats;

    protected override void ActivateFeatures()
    {
        this.playerStats.stackJumpsAmount = 2;
    }
}
