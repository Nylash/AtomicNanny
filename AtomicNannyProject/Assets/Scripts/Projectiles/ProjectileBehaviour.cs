using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [Header("CONFIGURATION")]
#pragma warning disable 0649
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
    
    Rigidbody rb;
    Vector3 birthPlace;

    //Flamethrower
    Vector3 deathPlace;
    float fullDistance;
    float startSpeed;

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

    protected virtual void Update()
    {
        //Debug.DrawLine(transform.position, transform.position + direction, Color.red, 10);
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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyScriptRef = collision.gameObject.GetComponent<Enemy>();
            enemyScriptRef.TakeDamage(damage);
            if(splashDamage != 0)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, splashDamageRadius, WeaponsManager.instance.enemiesMask);
                foreach(Collider item in colliders)
                {
                    if (item.gameObject == collision.gameObject)
                        continue;
                    Enemy scriptRef = item.gameObject.GetComponent<Enemy>();
                    scriptRef.TakeDamage(splashDamage);
                }
            }
            Destroy(gameObject);
        }
    }

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
