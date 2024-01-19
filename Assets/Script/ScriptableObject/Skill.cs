using UnityEngine;

[CreateAssetMenu(fileName = "New skill", menuName = "Scriptable Object/ New skill information")]
public class Skill : ScriptableObject
{
    public float damage;
    public float enduranceDecrement;
}