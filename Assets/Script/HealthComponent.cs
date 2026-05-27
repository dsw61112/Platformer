using UnityEngine;
using System.Collections;
using System;

using UnityEditor.SearchService;
using JetBrains.Annotations;

public class HealthComponent : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    private bool invincibility;

    public delegate void OnHealthChangedHandler(float newHealth, float amountChanged);
    public event OnHealthChangedHandler OnHealthChanged;

    public delegate void OnHealthInitializedHandler(float newHealth);
    public event OnHealthInitializedHandler OnHealthInitialized;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthInitialized?.Invoke(currentHealth);
    }


    public void RecieveDamage(float amount, GameObject go)
    {
        Debug.Log(go);
        if (!invincibility)
        {
            currentHealth -= amount;
            OnHealthChanged?.Invoke(currentHealth, amount);
            invincibility = true;
            StartCoroutine(ResetInvicibility(3));
        }

        if (currentHealth <= 0)
        {
            GetComponent<SceneOpener>().OpenScene();
        }
    }

    IEnumerator ResetInvicibility(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        invincibility = false;
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;
        OnHealthChanged?.Invoke(currentHealth, amount);
        //Debug.Log(currentHealth)
        
    }
}