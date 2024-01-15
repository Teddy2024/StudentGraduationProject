using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile16 : Projectile
{
    [SerializeField] private CircleCollider2D attractionCollider;
    bool isAttracting = false;

    private void FixedUpdate()
    {
        if (isAttracting)
        {
            AttractEnemies();
        }
    }

    protected override void Attack(IDamageable enemy)
    {
        base.Attack(enemy);
        StartAttracting();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            StopAttracting();
        }
    }

    private void StartAttracting()
    {
        isAttracting = true;
    }

    private void StopAttracting()
    {
        isAttracting = false;
    }

    private void AttractEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2.5f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                _knockBackForce = Mathf.Max(_knockBackForce, 1000);
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                Vector2 direction = (transform.position - collider.transform.position).normalized;
                enemyRb.AddForce(direction * _knockBackForce);
            }
        }
    }

    public override void ApplyAmountBuff(int level)
    {
        level = Mathf.Clamp(level, 1, 3);
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                break;
            case 2:  
                break;
            case 3:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0f, 0f, 10f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(0,-0.5f,0), Quaternion.Euler(0f, 0f, -10f));
                break;
        }
    }
}
