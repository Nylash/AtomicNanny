using System.Collections;
using UnityEngine;

public class ThrustingHit : Pattern
{
    public override IEnumerator StartPatternExecution()
    {
        print(name);
        yield return new WaitForSeconds(1);
        originScript.EndOfPattern();
    }
}
