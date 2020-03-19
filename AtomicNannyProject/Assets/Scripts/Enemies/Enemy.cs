using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("CONFIGURATION")]
    public float maxHealth;
    public GameObject hpBar;

    [Header("VARIABLES")]
    public float currentHealth;
    public GameObject hpBarRef;
    public EnemyHPBar hpBarScriptRef;


    private void Start()
    {
        currentHealth = maxHealth;
        hpBarRef = Instantiate(hpBar);
        hpBarScriptRef = hpBarRef.GetComponent<EnemyHPBar>();
        hpBarScriptRef.target = transform;
        hpBarScriptRef.maxHealth = maxHealth;
    }


    //Apply damage to enemy & call update on hpBar
    public void TakeDamage(float damage, AmmunitionManager.AmmoType ammoType)
    {
        currentHealth -= damage;
        hpBarScriptRef.UpdateFillValue(currentHealth);
        AmmunitionManager.instance.RefillAmmo(damage, ammoType);
        if (currentHealth < 0)
        {
            Destroy(hpBarRef);
            Destroy(gameObject);
        }  
    }
}
