using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] CoinMagnet coinMagnet;
    Vector3 force;
    float range = 5;
    bool prizeGiven;

    private void Start()
    {
        force = new Vector3(Random.Range(-range, range), 5, Random.Range(-range, range));
        rb.AddRelativeForce(force, ForceMode.Impulse);
        GameManager.Instance.OnBeforeSaveEvent += GameManager_OnBeforeSaveEvent;

    }

    private void GameManager_OnBeforeSaveEvent(object sender, GameManager.OnBeforeSaveEventEventArgs e)
    {
        coinMagnet.SetTarget(e.target);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !prizeGiven)
        {
            prizeGiven = true;
            int prizeAmount = 10;
            FindObjectOfType<GameScreen>().AddGold(prizeAmount);
            Destroy(gameObject);
        }
    }
}
