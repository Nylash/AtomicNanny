using UnityEngine;

public class AtomicBallBehaviour : ProjectileBehaviour
{
    //Override of Projectile class, with an explosion on CollisionEnter and a trigger detector

    /*protected override void Start()
    {
        base.Start();
    }*/

    protected override void OnCollisionEnter(Collision collision)
    {
        if (splashDamage != 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, splashDamageRadius, WeaponsManager.instance.enemiesMask);
            foreach (Collider item in colliders)
            {
                Enemy scriptRef = item.gameObject.GetComponent<Enemy>();
                scriptRef.TakeDamage(splashDamage, ammoType);
                //knockback
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ray"))
            OnCollisionEnter(null);
    }
}
