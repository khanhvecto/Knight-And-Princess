using UnityEngine;

public class RollTutorial : TutorialActivate
{
    [Header("------ ROLL TUTORIAL ------")]

    [Header("References")]
    [SerializeField] protected PlayerStats playerStats;

    protected override void ActivateFeatures()
    {
        this.playerStats.rollAbiity = true;
    }
}
