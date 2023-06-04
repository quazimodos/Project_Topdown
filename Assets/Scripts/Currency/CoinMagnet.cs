using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    GameObject target;
    [SerializeField] GameObject parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
        }
    }

    private void Update()
    {
        if (target)
        {
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, target.transform.position, 0.3f);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
