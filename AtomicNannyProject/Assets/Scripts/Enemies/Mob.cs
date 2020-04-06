using UnityEngine.AI;
using UnityEngine;

public class Mob : Enemy
{
    public bool obstacleObstuction;

    PatternObject actualPattern;

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
            //Or when movingTimerCap is reached
            case BehaviourState.moving:
                if(kittingTimerCap != 0)
                {
                    movingTimer += Time.deltaTime;
                    if (movingTimer > kittingTimerCap)
                    {
                        movingTimer = 0;
                        UpdateRange();
                        DetermineBehaviour();
                    }
                }
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
                //First frame launch the attack, then the pattern handle the rest (reset do by EndOfPattern, called at the end of the pattern)
            case BehaviourState.attacking:
                navAgent.isStopped = true;
                if (actualPattern == null)
                {
                    DetermineAttack();
                    actualPattern.patternScript.originScript = this;
                    StartCoroutine(actualPattern.patternScript.StartPatternExecution());
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
        float rand = Random.Range(0, 100);
        switch (currentRange)
        {
            case RangeState.outRange:
                currentBehaviour = BehaviourState.moving;
                break;
            case RangeState.highRange:
                if (rand < highRangeAttackProb)
                {
                    if (Physics.Linecast(transform.position, player.position, obstacleLayer))
                    {
                        obstacleObstuction = true;
                        currentBehaviour = BehaviourState.moving;
                    }
                    else
                    {
                        currentBehaviour = BehaviourState.attacking;
                        rb.velocity = Vector3.zero;
                    }   
                }  
                else
                    currentBehaviour = BehaviourState.moving;
                break;
            case RangeState.midRange:
                if (rand < midRangeAttackProb)
                {
                    if (Physics.Linecast(transform.position, player.position, obstacleLayer))
                    {
                        obstacleObstuction = true;
                        currentBehaviour = BehaviourState.moving;
                    }
                    else
                    {
                        currentBehaviour = BehaviourState.attacking;
                        rb.velocity = Vector3.zero;
                    }
                }
                else
                    currentBehaviour = BehaviourState.moving;
                break;
            case RangeState.closeRange:
                currentBehaviour = BehaviourState.attacking;
                rb.velocity = Vector3.zero;
                break;
        }
    }

    //Select a pattern depending on probabilities determine on EnemyGameObject
    //To resume, we rand a number between 0 & 99
    //Then we test patterns store in CurrentRangePatterns
    //if the rand is >= to minProb and < to maxProb of this pattern, then it is the good one
    void DetermineAttack()
    {
        int index = Random.Range(0, 100);
        switch (currentRange)
        {
            case RangeState.outRange:
                Debug.LogError("You can't be there.");
                break;
            case RangeState.highRange:
                foreach (PatternObject pattern in highRangePatterns)
                {
                    if(index >= pattern.minProb && index < pattern.maxProb)
                    {
                        actualPattern = pattern;
                        break;
                    }
                }
                break;
            case RangeState.midRange:
                foreach (PatternObject pattern in midRangePatterns)
                {
                    if (index >= pattern.minProb && index < pattern.maxProb)
                    {
                        actualPattern = pattern;
                        break;
                    }
                }
                break;
            case RangeState.closeRange:
                foreach (PatternObject pattern in closeRangePatterns)
                {
                    if (index >= pattern.minProb && index < pattern.maxProb)
                    {
                        actualPattern = pattern;
                        break;
                    }
                }
                break;
        }
        if (actualPattern == null)
            Debug.LogError("Verify all patterns probabilities, especially " + currentRange + " patterns.");
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

    //Method called at the end of attack pattern, to choose new behaviour
    public override void EndOfPattern()
    {
        actualPattern = null;
        UpdateRange();
        DetermineBehaviour();
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
