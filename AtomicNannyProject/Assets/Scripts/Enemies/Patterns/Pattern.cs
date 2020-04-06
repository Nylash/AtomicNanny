using UnityEngine;
using System.Collections;

public abstract class Pattern : MonoBehaviour
{
    [Header("CONFIGURATION")]
    public Animation anim;
    [Header("VARIABLES")]
    public Enemy originScript;

    public abstract IEnumerator StartPatternExecution();
}
