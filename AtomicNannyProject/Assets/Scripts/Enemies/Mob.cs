using UnityEngine.AI;
using UnityEngine;

public class Mob : Enemy
{
    public bool obstacleObstuction;

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        hpBarRef = Instantiate(hpBar);
        hpBarScriptRef = hpBarRef.GetComponent<EnemyHPBar>();
        hpBarScriptRef.target = transform;
        hpBarScriptRef.maxHealth = maxHealth;
    }

    protected override void Update()
    {
        base.Update();
        switch (currentBehaviour)
        {
            //If the mob is moving, everytime he pass a threshold we call DetermineBehaviour()
            case BehaviourState.moving:
                navAgent.isStopped = false;
                switch (currentRange)
                {
                    case RangeState.outRange:
                        if(distanceFromPlayer < highRangeCap)
                        {
                            currentRange = RangeState.highRange;
                            DetermineBehaviour();
                        }
                        else
                        {
                            navAgent.SetDestination(player.position);
                        }
                        break;
                    case RangeState.highRange:
                        if(distanceFromPlayer < midRangeCap)
                        {
                            currentRange = RangeState.midRange;
                            DetermineBehaviour();
                        }
                        else
                        {
                            navAgent.SetDestination(player.position);
                        }
                        break;
                    case RangeState.midRange:
                        if (distanceFromPlayer < closeRangeCap)
                        {
                            currentRange = RangeState.closeRange;
                            DetermineBehaviour();
                        }
                        else
                        {
                            navAgent.SetDestination(player.position);
                        }
                        break;
                    case RangeState.closeRange:
                        currentBehaviour = BehaviourState.attacking;
                        break;
                }
                //If there was a obstacle last time we called DetermineBehaviour() we verify at each frame if there is still one, else we call DetermineBehaviour()
                if (obstacleObstuction)
                {
                    if (!Physics.Linecast(transform.position, player.position, obstacleLayer))
                    {
                        UpdateRange();
                        DetermineBehaviour();
                    }
                }
                break;
                //Here launch an attack, then when it ends we UpdateRange and DetermineBehaviour
            case BehaviourState.attacking:
                navAgent.isStopped = true;
                switch (currentRange)
                {
                    case RangeState.outRange:
                        UpdateRange();
                        DetermineBehaviour();
                        break;
                    case RangeState.highRange:
                        print("highRange attack");
                        UpdateRange();
                        DetermineBehaviour();
                        break;
                    case RangeState.midRange:
                        print("midRange attack");
                        UpdateRange();
                        DetermineBehaviour();
                        break;
                    case RangeState.closeRange:
                        print("closeRange attack");
                        UpdateRange();
                        DetermineBehaviour();
                        break;
                }
                break;
        }
    }

    //This method works depending the current range
    //OutRange => the mob moves
    //HighRange => depending on highRangeAttackProb we decide if he keeps moving or if he attacks, if he attacks we check if there is an obstacle, if there is one, we move and wait until there none (in Update)
    //MidRange => same as HighRange
    //CloseRange => the mob can't be closer to the player so he attacks
    void DetermineBehaviour()
    {
        obstacleObstuction = false;
        switch (currentRange)
        {
            case RangeState.outRange:
                currentBehaviour = BehaviourState.moving;
                break;
            case RangeState.highRange:
                float randHigh = Random.Range(0, 100);
                if (randHigh < highRangeAttackProb)
                {
                    if (Physics.Linecast(transform.position, player.position, obstacleLayer))
                    {
                        obstacleObstuction = true;
                        currentBehaviour = BehaviourState.moving;
                    }
                    else
                        currentBehaviour = BehaviourState.attacking;
                }  
                else
                    currentBehaviour = BehaviourState.moving;
                break;
            case RangeState.midRange:
                float randMid = Random.Range(0, 100);
                if (randMid < midRangeAttackProb)
                {
                    if (Physics.Linecast(transform.position, player.position, obstacleLayer))
                    {
                        obstacleObstuction = true;
                        currentBehaviour = BehaviourState.moving;
                    }
                    else
                        currentBehaviour = BehaviourState.attacking;
                }
                else
                    currentBehaviour = BehaviourState.moving;
                break;
            case RangeState.closeRange:
                currentBehaviour = BehaviourState.attacking;
                break;
        }
    }

    //Simply update currentRange variable, needed for example after an attack
    void UpdateRange()
    {
        if (distanceFromPlayer < highRangeCap)
        {
            if (distanceFromPlayer < midRangeCap)
            {
                if (distanceFromPlayer < closeRangeCap)
                {
                    currentRange = RangeState.closeRange;
                }
                else
                    currentRange = RangeState.midRange;
            }
            else
                currentRange = RangeState.highRange;
        }
        else
            currentRange = RangeState.outRange;
    }

    //Used to display the range's threshold
    private void OnDrawGizmos()
    {
        if (displayRangeDebug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, highRangeCap);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, midRangeCap);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, closeRangeCap);
        }
    }
}
