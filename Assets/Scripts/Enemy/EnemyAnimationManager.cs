using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    public void HitMeleeAttack()
    {
        enemy.HitMeleeAttack();
    }

    public void HitRangedAttack()
    {
        enemy.HitRangedAttack();
    }
}
