using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetManager : MonoBehaviour
{
    //Target
    public List<Transform> enemyList = new List<Transform>();


    public Transform TargetEnemy()
    {
        Transform closestEnemy = null;

        if (enemyList.Count > 0)
        {
            float minDistance = Mathf.Infinity;
            foreach (Transform enemy in enemyList)
            {
                if (enemy != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
        return closestEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyList.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyList.Remove(other.transform);
        }
    }
}
