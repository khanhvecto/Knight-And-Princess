using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : DamagableObj
{
    protected void Start()
    {
        base.damage = 3f;
    }
}
