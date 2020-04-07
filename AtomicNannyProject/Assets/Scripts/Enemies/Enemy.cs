using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    //Basic class for all ennemies, including boss & mob

    [Header("CONFIGURATION")]
    public float maxHealth;
    public bool displayRangeDebug;
    public float highRangeCap;
    public float midRangeCap;
    public float closeRangeCap;
    public float kittingTimerCap;
    public float kittingIncreaseProbability;
    public GameObject hpBar;
    public LayerMask obstacleLayer;

    [Header("PATTERNS CONFIGURATION")]
    public float highRangeAttackProb;
    public float midRangeAttackProb;
    public PatternObject[] highRangePatterns;
    public PatternObject[] midRangePatterns;
    public PatternObject[] closeRangePatterns;

    [Header("VARIABLES")]
    public float currentHealth;
    public GameObject hpBarRef;
    public EnemyHPBar hpBarScriptRef;
    public BehaviourState currentBehaviour;
    public RangeState currentRange;
    
    protected Transform player;
    protected float distanceFromPlayer;
    protected NavMeshAgent navAgent;
    protected Rigidbody rb; 
    protected float movingTimer;
    protected float kittingIncreaseProbabilityVariable;
    
    Coroutine dotCoroutine;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.position);
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

    //Method called at the end of attack pattern, to choose new behaviour
    public abstract void EndOfPattern();

    public enum BehaviourState
    {
        moving, attacking
    }

    public enum RangeState
    {
        outRange, highRange, midRange, closeRange
    }

    [System.Serializable]
    public class PatternObject
    {
        public Pattern patternScript;
        public float minProb;
        public float maxProb;
    }
}
