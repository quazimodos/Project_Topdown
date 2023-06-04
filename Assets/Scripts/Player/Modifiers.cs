using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GunController gunController;
    public float health { get { return m_healthModifier; } }
    public float reloadSpeed { get { return m_reloadSpeedModifier; } }
    public float playerSpeed { get { return m_playerSpeedModifier; } }
    public float timeBetweenShots { get { return m_timeBetweenShotsModifier; } }
    public float magazineCapacity { get { return m_magazineCapacityModifier; } }
    public float damage { get { return m_damageModifier; } }

    private float m_healthModifier = 1;
    private float m_reloadSpeedModifier = 1;
    private float m_playerSpeedModifier = 1;
    private float m_timeBetweenShotsModifier = 1;
    private float m_magazineCapacityModifier = 1;
    private float m_damageModifier = 1;



    public void ApplyModifiers(ModifierType type, float multiplier)
    {
        switch (type)
        {
            case ModifierType.Health:
                m_healthModifier = Mathf.Clamp(m_healthModifier + multiplier, 0, 99);
                player.ReCalculateHealth();
                break;
            case ModifierType.ReloadSpeed:
                m_reloadSpeedModifier = Mathf.Clamp(m_reloadSpeedModifier - multiplier, 0, 99);
                break;
            case ModifierType.PlayerSpeed:
                m_playerSpeedModifier = Mathf.Clamp(m_playerSpeedModifier + multiplier, 0, 99);
                break;
            case ModifierType.TimeBetweenShots:
                m_timeBetweenShotsModifier = Mathf.Clamp(m_timeBetweenShotsModifier - multiplier, 0, 99);
                break;
            case ModifierType.MagazineCapacity:
                m_magazineCapacityModifier = Mathf.Clamp(m_magazineCapacityModifier + multiplier, 0, 99);
                gunController.RecalculateMagazineCapacity();
                break;
            case ModifierType.Damage:
                m_damageModifier = Mathf.Clamp(m_damageModifier + multiplier, 0, 99);
                break;
        }
    }


}
