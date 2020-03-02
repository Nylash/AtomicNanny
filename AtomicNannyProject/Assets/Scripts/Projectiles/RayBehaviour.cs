using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayBehaviour : MonoBehaviour
{
    public Vector3 endPosition;
    public float width;

    LineRenderer ray;
    float timer;

    private void Start()
    {
        gameObject.AddComponent<LineRenderer>();
        ray = GetComponent<LineRenderer>();
        ray.SetPosition(0, transform.position);
        ray.SetPosition(1, endPosition);
        ray.widthMultiplier = width;
        AddCollider();
    }

    private void Update()
    {
        timer += Time.deltaTime;
		if (timer > 1)
            Destroy(gameObject);
    }

    void AddCollider()
    {
        Vector3 startPos = ray.GetPosition(0);
        Vector3 endPos = ray.GetPosition(1);
        BoxCollider col = new GameObject("RayCollider").AddComponent<BoxCollider>();
        col.gameObject.tag = "Ray";
        col.transform.parent = ray.transform;
        float rayLength = Vector3.Distance(startPos, endPos);
        col.size = new Vector3(rayLength, width, width);
        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint;
        float angle = (Mathf.Abs(startPos.z - endPos.z) / Mathf.Abs(startPos.x - endPos.x));
        col.transform.rotation = Quaternion.FromToRotation(Vector3.right, (endPos - startPos).normalized);
        col.isTrigger = true;
    }
}
