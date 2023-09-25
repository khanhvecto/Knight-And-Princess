using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    [SerializeField] EnemyState stateScript;
    private Collider2D targetColl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void move()
    {
        targetColl = stateScript.targetColl;
    }
}
