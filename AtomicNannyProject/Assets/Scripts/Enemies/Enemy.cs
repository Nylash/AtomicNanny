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

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        hpBarScriptRef.UpdateFillValue(currentHealth);
        if (currentHealth < 0)
        {
            Destroy(hpBarRef);
            Destroy(gameObject);
        }  
    }
}
