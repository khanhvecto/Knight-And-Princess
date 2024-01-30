using UnityEngine;

public class BlockTutorial : TutorialActivate
{
    [Header("------ BLOCK TUTORIAL ------")]

    [Header("References")]
    [SerializeField] protected PlayerStats playerStats;

    protected override void ActivateFeatures()
    {
        this.playerStats.blockAbility = true;
    }
}
