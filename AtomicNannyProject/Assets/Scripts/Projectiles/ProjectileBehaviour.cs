using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [Header("CONFIGURATION")]
#pragma warning disable 0649
    //Curve used to set flame speed, work has a ratio which modify speed
    [SerializeField] AnimationCurve flameSpeedCurve;
#pragma warning restore 0649
    [Header("VARIABLES")]
    public Vector3 direction;
    public float speed;
    public float range;
    public bool isFlame;
    public float damage;
    public float splashDamage;
    public float splashDamageRadius;
    public float enemyKnockback;
    public AmmunitionManager.AmmoType ammoType;
    
    Rigidbody rb;
    Vector3 birthPlace;

    //Flamethrower
    Vector3 deathPlace;
    float fullDistance;
    float startSpeed;

    //Here we initialize the projectile
    protected virtual void Start()
    {
        birthPlace = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
        deathPlace = birthPlace + direction * range;
        if (isFlame)
        {
            startSpeed = speed;
            fullDistance = Vector3.Distance(birthPlace, deathPlace);
        }
    }

    //Destroy the projectile when it reach its range
    //If it's a flame we adapt the speed according to the curve and the distance done
    //When it overpass 90% of the full distance it has to do we destroy it, because it will never reach the full distance (at the end of the curve the speed is 0 so it stop just before)
    protected virtual void Update()
    {
        if (Vector3.Distance(birthPlace, transform.position) > range)
            Destroy(gameObject);
        if (isFlame)
        {
            speed = startSpeed * flameSpeedCurve.Evaluate(Vector3.Distance(birthPlace, transform.position) / fullDistance);
            rb.velocity = direction * speed;
            if (Vector3.Distance(birthPlace, transform.position) / fullDistance > .9f)
                Destroy(gameObject);
        }   
    }

    //Behaviour when it collide with something
    //If it's an enemy it applied damage and knockback to it then dies
    //Otherwise it simply died
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyScriptRef = collision.gameObject.GetComponent<Enemy>();
            enemyScriptRef.TakeDamage(damage, ammoType);
            if (splashDamage != 0)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, splashDamageRadius, WeaponsManager.instance.enemiesMask);
                foreach (Collider item in colliders)
                {
                    if (item.gameObject == collision.gameObject)
                        continue;
                    Enemy scriptRef = item.gameObject.GetComponent<Enemy>();
                    scriptRef.TakeDamage(splashDamage, ammoType);
                    //knockback
                }
            }
            Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }

    //Used to see where the projectile should end
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(deathPlace, new Vector3(.1f,.1f,.1f));
        if(splashDamage != 0)
        {
            Gizmos.DrawWireSphere(transform.position, splashDamageRadius);
        }
    }
}
