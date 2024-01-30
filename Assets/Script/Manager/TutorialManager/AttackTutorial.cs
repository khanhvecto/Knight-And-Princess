using UnityEngine;

public class AttackTutorial : TutorialActivate
{
    [Header("------ ATTACK TUTORIAL ------")]

    [Header("References")]
    [SerializeField] protected PlayerStats playerStats;

    protected override void ActivateFeatures()
    {
        this.playerStats.attackAbility = true;
    }
}
