using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    public Arum target;

    public bool CheckTarget()
    {
        return target != null;
    }
}
