using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] LivingEntity livingEntity;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;
    private void Start()
    {
        healthSlider.maxValue = livingEntity.GetTotalHealth();
    }

    private void Update()
    {
        healthSlider.maxValue = livingEntity.GetTotalHealth();
        healthSlider.value = livingEntity.GetCurrentHealth();
        healthText.text = string.Format("{0} HP", livingEntity.GetCurrentHealth());
    }
}
