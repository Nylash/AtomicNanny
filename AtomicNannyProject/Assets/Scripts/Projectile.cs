using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float range;

    Vector3 birthPlace;

    private void Start()
    {
        birthPlace = transform.position;
        GetComponent<Rigidbody>().velocity = direction * speed;
        Debug.DrawLine(transform.position, transform.position + direction, Color.green, 10);
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + direction, Color.red, 10);
        if (Vector3.Distance(birthPlace, transform.position) > range)
            Destroy(gameObject);
    }
}
