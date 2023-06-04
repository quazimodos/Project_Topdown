using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    [SerializeField] private float startingHealth;
    [SerializeField] MMF_Player damagedFeedback;
    protected private float currentHealth;
    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        currentHealth = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }
    public virtual void TakeDamage(float damage)
    {
        damagedFeedback?.PlayFeedbacks();
        currentHealth -= damage;
        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }

    public float GetTotalHealth()
    {
        return startingHealth;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetTotalHealth(float health)
    {
        startingHealth = health;
    }
    public void SetCurrentHealth(float health)
    {
        currentHealth = health;
    }
}
