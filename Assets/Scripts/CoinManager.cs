using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    [HideInInspector] public int TotalCoin { get { return m_collectedGold; } }
    protected private int m_collectedGold = 0;

    public async void AddCoin(int amount)
    {
        bool state = await DatabaseManager.Instance.AddCoins(amount);
        if (state)
        {
            Debug.Log("Completed");
        }
        else
        {
            Debug.Log("Error on transaction");
        }
    }

    public async void BuyGun(int gunIndex, int price)
    {
        bool state = await DatabaseManager.Instance.BuyGun(gunIndex, price);
        if (state)
        {
            Debug.Log("Purchase Completed");
        }
        else
        {
            Debug.Log("You dont have enough money");
        }
    }

    public void AddCoinToShow(int amount)
    {
        m_collectedGold += amount;
    }

    public void ResetCoin()
    {
        m_collectedGold = 0;
    }
}
