using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShopPopup : Popup
{
    [SerializeField] ShopSO shopSO;
    [SerializeField] GameObject content;
    [SerializeField] ShopItem shopItem;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        PopulateShop();
        DatabaseManager.Instance.OnNewPurchaseHappenOnDatabase += DatabaseManager_OnNewPurchaseHappenOnDatabase;
    }

    private void DatabaseManager_OnNewPurchaseHappenOnDatabase(object sender, DatabaseManager.OnNewPurchaseHappenOnDatabaseEventArgs e)
    {
        PopulateShop();
    }

    public void Unsubscribe()
    {
        DatabaseManager.Instance.OnNewPurchaseHappenOnDatabase -= DatabaseManager_OnNewPurchaseHappenOnDatabase;
    }

    private void ClearContent()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private async void PopulateShop()
    {
        ClearContent();

        var operation = await DatabaseManager.Instance.GetPurchasedGunList();
        var gunMap = operation.gunlist;
        for (int i = 0; i < shopSO.gunModels.Count; i++)
        {
            GunModel selectedGun = shopSO.gunModels[i];

            StartCoroutine(CreateItem(operation, gunMap, i, selectedGun));
        }
    }

    private IEnumerator CreateItem((Dictionary<string, object> gunlist, int lastUsedGun) operation, Dictionary<string, object> gunMap, int i, GunModel selectedGun)
    {
        ShopItem item = Instantiate(shopItem, content.transform);
        item.SetGunInfo(
            selectedGun.gunSprite,
            selectedGun.name,
            selectedGun.fireMode,
            selectedGun.damage,
            selectedGun.magazineCapacity,
            selectedGun.reloadTime,
            selectedGun.spreadAngle,
            selectedGun.projectileCount,
            Convert.ToBoolean(gunMap["Gun" + i]),
            i,
            operation.lastUsedGun,
            selectedGun.price
            );

        yield return null;
    }
}
