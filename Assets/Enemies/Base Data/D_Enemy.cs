using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data / Enemy Data / Base Data")]
public class D_Enemy : ScriptableObject
{
    public float wallCheckDistance = 0.4f;
    public float ledgeCheckDistance = 0.65f;

    public LayerMask whatIsGround;
}
