using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoreHealthBar : MonoBehaviour
{
    [SerializeField] LivingEntity livingEntity;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] Slider healthSlider;
    private void Start()
    {
        healthSlider.maxValue = livingEntity.GetTotalHealth();
    }

    private void Update()
    {
        healthSlider.value = livingEntity.GetCurrentHealth();
        float percentage = ((healthSlider.value / healthSlider.maxValue) * 100);
        textMeshProUGUI.text = string.Format("%{0}", Mathf.RoundToInt(percentage));
    }
}
