using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using Unity.VisualScripting;
using System.Transactions;

public class DatabaseManager : Singleton<DatabaseManager>
{
    public const string COIN = "Coins";
    public const string SCORE = "Score";
    public const string LAST_GUN = "LastUsedGun";
    public const string PURCHASED_GUNS = "PurchasedGuns";

    FirebaseFirestore database;


    public event EventHandler<OnPlayerDataChangedOnDatabaseEventArgs> OnPlayerDataChangedOnDatabase;
    public event EventHandler<OnNewPurchaseHappenOnDatabaseEventArgs> OnNewPurchaseHappenOnDatabase;

    public class OnPlayerDataChangedOnDatabaseEventArgs
    {
        public int coins;
        public int score;
    }

    public class OnNewPurchaseHappenOnDatabaseEventArgs
    {
        public AvailableGunList gunList;
    }

    public override void Awake()
    {
        database = FirebaseFirestore.DefaultInstance;
        base.Awake();
    }

    public async void CreateUser()
    {
        WriteBatch batch = database.StartBatch();

        DocumentReference documentReference = database.Collection("Player").Document("UserInfo");
        DocumentSnapshot snapshot = await documentReference.GetSnapshotAsync();
        if (!snapshot.Exists)
        {
            AvailableGunList gunList = new AvailableGunList
            {
                Gun0 = true,
                Gun1 = false,
                Gun2 = false,
                Gun3 = false,
                Gun4 = false,
                Gun5 = false,
                Gun6 = false,
                Gun7 = false,
                Gun8 = false
            };

            Dictionary<string, object> userData = new Dictionary<string, object>
            {
                { COIN, 0 },
                { SCORE, 0 },
                { LAST_GUN, 0 },
                { PURCHASED_GUNS, gunList }
            };
            batch.Set(documentReference, userData);
        }

        await batch.CommitAsync();
    }

    public async Task<bool> BuyGun(int gunIndex, int amount)
    {
        DocumentReference documentReference = database.Collection("Player").Document("UserInfo");
        bool transactionState = await database.RunTransactionAsync(async transaction =>
        {
            DocumentSnapshot snapshot = await transaction.GetSnapshotAsync(documentReference);
            int currentCoins = snapshot.GetValue<int>(COIN);

            var alldata = snapshot.ToDictionary();

            var gunlist = (Dictionary<string, object>)alldata[PURCHASED_GUNS];

            gunlist["Gun" + gunIndex] = true;

            if (currentCoins >= amount)
            {
                Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { COIN, currentCoins - amount},
                    { PURCHASED_GUNS, gunlist}
                };
                transaction.Update(documentReference, updates);

                return true;
            }
            else
            {
                return false;
            }
        });

        return transactionState;
    }

    public async Task<bool> AddCoins(int amount)
    {
        DocumentReference documentReference = database.Collection("Player").Document("UserInfo");
        bool transactionState = await database.RunTransactionAsync(async transaction =>
        {
            DocumentSnapshot snapshot = await transaction.GetSnapshotAsync(documentReference);
            int currentCoins = snapshot.GetValue<int>(COIN);

            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { COIN, currentCoins + amount}
            };
            transaction.Update(documentReference, updates);

            return true;
        });

        return transactionState;
    }

    public async Task<bool> UpdateScore(int newScore)
    {
        DocumentReference documentReference = database.Collection("Player").Document("UserInfo");
        bool transactionState = await database.RunTransactionAsync(async transaction =>
        {
            DocumentSnapshot snapshot = await transaction.GetSnapshotAsync(documentReference);
            int lastScore = snapshot.GetValue<int>(SCORE);

            if (lastScore < newScore)
            {
                Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { SCORE, newScore}
                };
                transaction.Update(documentReference, updates);
            }
            return true;
        });
        return transactionState;
    }

    public void ListenPlayerData()
    {
        DocumentReference documentReference = database.Collection("Player").Document("UserInfo");
        documentReference.Listen(snapshot =>
        {
            Dictionary<string, object> playerData = snapshot.ToDictionary();
            OnPlayerDataChangedOnDatabase?.Invoke(this, new OnPlayerDataChangedOnDatabaseEventArgs
            {
                coins = Convert.ToInt32(playerData[COIN]),
                score = Convert.ToInt32(playerData[SCORE])
            });

            OnNewPurchaseHappenOnDatabase?.Invoke(this, new OnNewPurchaseHappenOnDatabaseEventArgs { gunList = snapshot.GetValue<AvailableGunList>(PURCHASED_GUNS) });
        });
    }

    public async Task<int> GetLastUsedGun()
    {
        DocumentReference documentReference = database.Collection("Player").Document("UserInfo");
        DocumentSnapshot snapshot = await documentReference.GetSnapshotAsync();
        return snapshot.GetValue<int>(LAST_GUN);
    }

    public async void UpdateLastUsedGun(int gunIndex)
    {
        DocumentReference documentReference = database.Collection("Player").Document("UserInfo");
        Dictionary<string, object> newData = new Dictionary<string, object>
        {
            {LAST_GUN, gunIndex }
        };
        await documentReference.UpdateAsync(newData);
    }

    public async Task<(Dictionary<string, object> gunlist, int lastUsedGun)> GetPurchasedGunList()
    {
        DocumentReference documentReference = database.Collection("Player").Document("UserInfo");
        DocumentSnapshot snapshot = await documentReference.GetSnapshotAsync();
        var dictionary = snapshot.ToDictionary();
        Dictionary<string, object> gunMap = new Dictionary<string, object>();
        gunMap = (Dictionary<string, object>)dictionary[PURCHASED_GUNS];
        return
            (
            gunlist: gunMap,
            lastUsedGun: snapshot.GetValue<int>(LAST_GUN)
            );
    }

}

[FirestoreData]
public class AvailableGunList
{
    [FirestoreProperty] public bool Gun0 { get; set; }
    [FirestoreProperty] public bool Gun1 { get; set; }
    [FirestoreProperty] public bool Gun2 { get; set; }
    [FirestoreProperty] public bool Gun3 { get; set; }
    [FirestoreProperty] public bool Gun4 { get; set; }
    [FirestoreProperty] public bool Gun5 { get; set; }
    [FirestoreProperty] public bool Gun6 { get; set; }
    [FirestoreProperty] public bool Gun7 { get; set; }
    [FirestoreProperty] public bool Gun8 { get; set; }
}
