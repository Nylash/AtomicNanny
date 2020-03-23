﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("CONFIGURATION")]
    public float maxHealth;
    public GameObject hpBar;

    [Header("VARIABLES")]
    public float currentHealth;
    public GameObject hpBarRef;
    public EnemyHPBar hpBarScriptRef;

    Coroutine dotCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;
        hpBarRef = Instantiate(hpBar);
        hpBarScriptRef = hpBarRef.GetComponent<EnemyHPBar>();
        hpBarScriptRef.target = transform;
        hpBarScriptRef.maxHealth = maxHealth;
    }


    //Apply damage to enemy & call update on hpBar
    //If it's a dot it start a coroutine and stop the previous one (if there is one), it simply apply or reapply the dot
    public void TakeDamage(float damage, float ammoGain, AmmunitionManager.AmmoType ammoType)
    {
        currentHealth -= damage;
        hpBarScriptRef.UpdateFillValue(currentHealth);
        AmmunitionManager.instance.RefillAmmo(ammoGain, ammoType);
        if (currentHealth < 0)
        {
            Destroy(hpBarRef);
            Destroy(gameObject);
        }  
    }

    //Method applying or reapplying a dot
    public void ApplyDot(float dotDamage, float ammoGain, AmmunitionManager.AmmoType ammoType)
    {
        if (dotCoroutine == null)
            dotCoroutine = StartCoroutine(DamageOverTime(dotDamage, ammoGain, ammoType));
        else
        {
            StopCoroutine(dotCoroutine);
            dotCoroutine = StartCoroutine(DamageOverTime(dotDamage, ammoGain, ammoType));
        }
    }

    //Coroutine handling dot
    IEnumerator DamageOverTime(float tickDamage, float ammoGain, AmmunitionManager.AmmoType ammoType)
    {
        for (int i = 0; i < WeaponsStats.instance.numberOfTicks; i++)
        {
            currentHealth -= tickDamage;
            hpBarScriptRef.UpdateFillValue(currentHealth);
            AmmunitionManager.instance.RefillAmmo(ammoGain, ammoType);
            if (currentHealth < 0)
            {
                Destroy(hpBarRef);
                Destroy(gameObject);
                break;
            }
            yield return new WaitForSeconds(WeaponsStats.instance.intervalBtwTicks);
        }
    }
}
