using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelScreen : BaseScreen
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI totalCoinText;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        DatabaseManager.Instance.OnPlayerDataChangedOnDatabase += DatabaseManager_OnCoinsChangedOnDatabase;
        DatabaseManager.Instance.ListenPlayerData();
    }

    private void DatabaseManager_OnCoinsChangedOnDatabase(object sender, DatabaseManager.OnPlayerDataChangedOnDatabaseEventArgs e)
    {
        totalCoinText.text = e.coins.ToString();
    }

    public void OnShopButtonPressed()
    {
        OpenPopup<ShopPopup>("Popups/ShopPopup");
    }
}
