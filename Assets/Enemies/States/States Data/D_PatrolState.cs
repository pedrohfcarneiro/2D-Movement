using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPatrolStateData", menuName = "Data/States Data/PatrolState Data")]
public class D_PatrolState : ScriptableObject
{
    public float patrolSpeed = 3f;
    public float moveDuration = 4f;
}
