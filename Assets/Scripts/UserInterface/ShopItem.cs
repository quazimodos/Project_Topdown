using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI fireMode;
    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI capacity;
    [SerializeField] TextMeshProUGUI reloadTime;
    [SerializeField] TextMeshProUGUI fireRate;
    [SerializeField] TextMeshProUGUI spreadCount;

    [Header("Buttons")]
    [SerializeField] GameObject buyButton;
    [SerializeField] GameObject useButton;
    [SerializeField] GameObject inactiveButton;
    [SerializeField] TextMeshProUGUI price;

    bool purchased = false;
    int index;

    public void SetGunInfo(Sprite itemImage, string itemName, Gun.FireMode fireMode, float damage, int capacity, float reloadTime, float fireRate, int spreadCount, bool purchased, int index, int lastUsedGunIndex, int price)
    {
        this.itemImage.sprite = itemImage;
        this.itemName.text = itemName;
        this.fireMode.text = string.Format("Fire Mode: {0}", fireMode.ToString());
        this.damage.text = string.Format("Damage: {0}", damage);
        this.capacity.text = string.Format("Capacity: {0}", capacity);
        this.reloadTime.text = string.Format("Reload Time: {0}s", reloadTime);
        this.fireRate.text = string.Format("Fire Rate: {0}s", fireRate);
        this.spreadCount.text = string.Format("Spread: {0}", spreadCount);
        this.price.text = price.ToString();
        this.purchased = purchased;
        this.index = index;


        SelectButton(lastUsedGunIndex, price);
    }

    private void SelectButton(int lastUsedGunIndex, int price)
    {
        if (!purchased)
        {
            buyButton.SetActive(true);
            useButton.SetActive(false);
            inactiveButton.SetActive(false);

            buyButton.GetComponent<AnimatedButton>().AddActionToEvent(() =>
            {
                CoinManager.Instance.BuyGun(index, price);
            });
        }
        else if (purchased && index != lastUsedGunIndex)
        {
            buyButton.SetActive(false);
            useButton.SetActive(true);
            inactiveButton.SetActive(false);

            useButton.GetComponent<AnimatedButton>().AddActionToEvent(() =>
            {
                DatabaseManager.Instance.UpdateLastUsedGun(index);
            });
        }
        else
        {
            buyButton.SetActive(false);
            useButton.SetActive(false);
            inactiveButton.SetActive(true);
        }
    }
}
