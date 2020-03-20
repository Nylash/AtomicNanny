using UnityEngine;

public class RayBehaviour : MonoBehaviour
{
    [Header("VARIABLES")]
    public Vector3 endPosition;
    public float width;
    public float damage;
    public float ammoGain;

    LineRenderer ray;
    float timer;

    //Initialize differents variables
    private void Start()
    {
        gameObject.AddComponent<LineRenderer>();
        ray = GetComponent<LineRenderer>();
        ray.SetPosition(0, transform.position);
        ray.SetPosition(1, endPosition);
        ray.widthMultiplier = width;
        AddCollider();
    }

    //Destroy the ray after .5 seconde
    private void Update()
    {
        timer += Time.deltaTime;
		if (timer > .5f)
            Destroy(gameObject);
    }

    //Add a collider to the ray
    void AddCollider()
    {
        Vector3 startPos = ray.GetPosition(0);
        Vector3 endPos = ray.GetPosition(1);
        BoxCollider col = gameObject.AddComponent<BoxCollider>();
        col.gameObject.tag = "Ray";
        float rayLength = Vector3.Distance(startPos, endPos);
        col.size = new Vector3(rayLength, width, width);
        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint;
        col.center = Vector3.zero;
        col.transform.rotation = Quaternion.FromToRotation(Vector3.right, (endPos - startPos).normalized);
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyScriptRef = other.gameObject.GetComponent<Enemy>();
            enemyScriptRef.TakeDamage(damage, ammoGain, AmmunitionManager.AmmoType.atomic);
            //knockback
        }
    }
}
