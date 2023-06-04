using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform weaponHold;
    [SerializeField] private Gun gun;
    [SerializeField] private Modifiers modifiers;
    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image reloadBg;
    [SerializeField] ShopSO shopSO;
    private Gun equippedGun;

    private void Start()
    {
        EquipGun(gun);
    }

    public async void EquipGun(Gun gunToEquip)
    {
        int lastUsedGunIndex = await DatabaseManager.Instance.GetLastUsedGun();

        GunModel selectedGun = shopSO.gunModels[lastUsedGunIndex];

        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.SetGunSpecs(selectedGun.fireMode, selectedGun.magazineCapacity, selectedGun.reloadTime, selectedGun.projectileCount, selectedGun.spreadAngle, selectedGun.gunVisual, selectedGun.damage, selectedGun.secBetweenShot, selectedGun.projectileSize);
        equippedGun.SetPlayer(GetComponent<Player>());
        equippedGun.transform.parent = weaponHold;
        equippedGun.AttachUIElements(reloadBg, ammoText);
        equippedGun.SetModifiers(modifiers);
    }

    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }

    public void Reload()
    {
        if (equippedGun != null)
        {
            equippedGun.Reload();
        }
    }

    public void RecalculateMagazineCapacity()
    {
        equippedGun.RecalculateMagazineCapacity();
    }
}
