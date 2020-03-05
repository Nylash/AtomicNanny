using System.Collections;
using UnityEngine;

public class RocketJump : WeaponMod
{
    [Header("SPECIFIC CONFIGURATION")]
    public float cooldown;
    public float timeBeforeJump;
    public float damage;
    public float explosionRadius;
    public float jumpLenght;
    public float jumpHeight;
    public float jumpDuration;
    public GameObject visualEffect;

    bool startJumping;
    bool endJumping;
    float currentJumpTime;
    Vector3 jumpDirection;

    public override float GetFireRate()
    {
        return cooldown;
    }

    public override float GetTimeBeforeFirstShoot()
    {
        return timeBeforeJump;
    }

    private void Update()
    {
        if (startJumping)
        {
            PlayerMovementManager.instance.recoil.y = Mathf.Lerp(jumpHeight, 0, currentJumpTime / (jumpDuration / 2));
            currentJumpTime += Time.deltaTime;
        }
        else if (endJumping)
        {
            PlayerMovementManager.instance.recoil.y = Mathf.Lerp(0, -jumpHeight, currentJumpTime / (jumpDuration / 2));
            currentJumpTime += Time.deltaTime;
        }
    }

    public override IEnumerator Shot()
    {
        if (reloading)
        {
            WeaponsManager.instance.startSecondaryShotNeeded = true;
            yield break;
        }
        WeaponsManager.instance.waitEndSpecificBehaviour = true;
        StartCoroutine(ReloadSystem());
        DoExplosion();
        CalculateDirection();
        float startY = PlayerMovementManager.instance.transform.position.y;
        startJumping = true;
        PlayerMovementManager.instance.recoil = jumpDirection * jumpLenght + new Vector3(0,jumpHeight,0);
        currentJumpTime = 0;
        yield return new WaitForSeconds(jumpDuration / 2);
        startJumping = false;
        endJumping = true;
        currentJumpTime = 0;
        yield return new WaitForSeconds(jumpDuration / 2);
        endJumping = false;
        PlayerMovementManager.instance.recoil = Vector3.zero;
        PlayerMovementManager.instance.transform.position = new Vector3(PlayerMovementManager.instance.transform.position.x, startY, PlayerMovementManager.instance.transform.position.z);
        PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.moving;
        WeaponsManager.instance.waitEndSpecificBehaviour = false;
    }

    void DoExplosion()
    {
        Collider[] colliders = Physics.OverlapSphere(WeaponsManager.instance.aimGuide.position, explosionRadius, WeaponsManager.instance.enemiesMask);
        foreach (Collider item in colliders)
        {
            Enemy scriptRef = item.gameObject.GetComponent<Enemy>();
            scriptRef.TakeDamage(damage);
            //knockback
        }
        //throw fx
    }

    void CalculateDirection()
    {
        Vector3 shootDirection;
        if (WeaponsManager.instance.usingGamepad)
        {
            float randomAngle = Random.Range(-GetInaccuracyAngle(), GetInaccuracyAngle());
            shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(WeaponsManager.instance.aimDirection.x, 0, WeaponsManager.instance.aimDirection.y) * Vector3.Distance(transform.position, new Vector3(WeaponsManager.instance.aimDirection.x, 0, WeaponsManager.instance.aimDirection.y));
            jumpDirection = shootDirection.normalized;
        }
        else
        {
            float randomAngle = Random.Range(-GetInaccuracyAngle(), GetInaccuracyAngle());
            shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(WeaponsManager.instance.aimDirectionMouse.x, 0, WeaponsManager.instance.aimDirectionMouse.z) * Vector3.Distance(transform.position, new Vector3(WeaponsManager.instance.aimDirectionMouse.x, 0, WeaponsManager.instance.aimDirectionMouse.z));
            jumpDirection = shootDirection.normalized;
        }
    }
}
