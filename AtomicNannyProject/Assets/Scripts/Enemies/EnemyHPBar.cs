using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [Header("VARIABLES")]
    public Vector3 offset;
    public float maxHealth;
    public Transform target;

    Canvas bar;
    Image fillBar;

    private void Start()
    {
        bar = GetComponent<Canvas>();
        fillBar = transform.GetChild(1).GetComponent<Image>();
    }

    void Update()
    {
        transform.position = new Vector3(target.position.x, offset.y, target.position.z);
    }

    public void UpdateFillValue(float currenthealth)
    {
        fillBar.fillAmount = currenthealth / maxHealth;
    }
}
