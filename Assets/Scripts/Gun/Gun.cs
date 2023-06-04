using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public enum FireMode { Auto, Burst, Single }

    [Header("Gun Options")]
    [SerializeField] private FireMode fireMode;
    [SerializeField] private float msBetweenShot = 100f;
    [SerializeField] private int projectilesPerMag;
    [SerializeField] private float muzzleVelocity = 35f;
    [SerializeField] private int burstCount;
    [SerializeField] float reloadTime = .3f;
    [SerializeField] private Transform protectileSpawn;
    [SerializeField] private Projectile projectile;
    [SerializeField] private Vector2 recoilMinMax = new Vector2(.05f, 0.2f);
    [SerializeField] private float recoilMoveSettleTime = .1f;
    [SerializeField] private Transform shell;
    [SerializeField] private Transform shellEjection;
    [SerializeField] private GameObject muzzleFlash;

    [SerializeField] private Transform gunVisualParent;

    private int numberOfProjectile;
    private float spreadAngle;
    private float damage;
    private float projectileSize;

    private Vector3 recoilSmoothDampVelocity;

    private int projectilesRemainingInMag;
    private int shotsRemainingInBurst;
    private float nextShotTime;

    private bool isReloading;
    private bool triggerReleasedSinceLastShot;
    private Player player;

    Image reloadBg;
    TextMeshProUGUI ammoText;

    Modifiers modifiers;

    private void Start()
    {
        shotsRemainingInBurst = burstCount;
        projectilesRemainingInMag = projectilesPerMag;
        UpdateAmmoText();
    }

    private void LateUpdate()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);

        if (!isReloading && projectilesRemainingInMag == 0)
        {
            Reload();
        }
    }


    void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }

            projectilesRemainingInMag--;
            nextShotTime = Time.time + (msBetweenShot * modifiers.timeBetweenShots) / 1000;

            float startRotation = player.transform.localEulerAngles.y + spreadAngle / 2;

            float angleIncrease = spreadAngle / ((float)numberOfProjectile - 1f);


            for (int j = 0; j < numberOfProjectile; j++)
            {
                float finalRotation = 0;

                if (angleIncrease > 0)
                {
                    finalRotation = startRotation + (angleIncrease * -j);
                }
                else
                {
                    finalRotation = startRotation;
                }

                Projectile newProjectile = Instantiate(projectile, protectileSpawn.position, Quaternion.Euler(0, finalRotation, 0));
                newProjectile.SetSpeedAndDamageModifier(muzzleVelocity, modifiers.damage, damage, projectileSize);
                if (muzzleFlash != null)
                    Instantiate(muzzleFlash, protectileSpawn.position, protectileSpawn.transform.rotation);
            }

            if (shellEjection != null)
                Instantiate(shell, shellEjection.position, shellEjection.rotation);
            transform.localPosition -= Vector3.forward * Random.Range(recoilMinMax.x, recoilMinMax.y);
            UpdateAmmoText();
        }
    }

    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMag != projectilesPerMag)
        {
            StartCoroutine(AnimateReload());
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1f / (reloadTime * modifiers.reloadSpeed);
        float percent = 0;
        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;
            reloadBg.fillAmount = percent;

            yield return null;
        }


        isReloading = false;
        projectilesRemainingInMag = projectilesPerMag;
        UpdateAmmoText();
    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }


    private void UpdateAmmoText()
    {
        ammoText.text = string.Format("{0}/{1}", projectilesRemainingInMag, projectilesPerMag);
    }

    public void AttachUIElements(Image reloadBg, TextMeshProUGUI ammoText)
    {
        this.reloadBg = reloadBg;
        this.ammoText = ammoText;
    }

    public void RecalculateMagazineCapacity()
    {
        projectilesPerMag = (int)(projectilesPerMag * modifiers.magazineCapacity);
        UpdateAmmoText();
    }

    public void SetModifiers(Modifiers modifiers)
    {
        this.modifiers = modifiers;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetGunSpecs(FireMode mode, int magazineCapacity, float reloadTime, int numberOfProjectile, float spreadAngle, GameObject gun, float damage, float secBetweenShot, float size)
    {
        fireMode = mode;
        projectilesPerMag = magazineCapacity;
        this.reloadTime = reloadTime;
        this.numberOfProjectile = numberOfProjectile;
        this.spreadAngle = spreadAngle;
        this.damage = damage;
        msBetweenShot = secBetweenShot * 1000;
        projectileSize = size;
        Instantiate(gun, gunVisualParent);
    }
}
