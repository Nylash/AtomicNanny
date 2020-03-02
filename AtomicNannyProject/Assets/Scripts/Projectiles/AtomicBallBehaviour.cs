using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicBallBehaviour : ProjectileBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ray"))
            Destroy(gameObject);
    }
}
