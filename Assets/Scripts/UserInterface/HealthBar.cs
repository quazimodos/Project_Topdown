using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Slider slider;
    [SerializeField] LivingEntity livingEntity;
    Camera mainCam;
    private void Awake()
    {
        mainCam = Camera.main;
        canvas.worldCamera = mainCam;
    }
    private void Start()
    {
        slider.maxValue = livingEntity.GetTotalHealth();
    }
    private void Update()
    {
        LookCamera();
        slider.value = livingEntity.GetCurrentHealth();
    }

    private void LookCamera()
    {
        canvas.transform.LookAt(mainCam.transform);
    }
}
