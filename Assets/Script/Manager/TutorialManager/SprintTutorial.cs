using UnityEngine;

public class SprintTutorial : TutorialActivate
{
    [Header("------ SPRINT TUTORIAL ------")]
    [Header("References")]
    [SerializeField] protected PlayerStats playerStats;

    protected override void ActivateFeatures()
    {
        this.playerStats.sprintAbility = true;
    }
}
