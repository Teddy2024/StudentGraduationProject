using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Projectile9 : Projectile
{
    protected override void Move()
    {
        if (!player) return;

        StartCoroutine(TrackPlayer());
    }

    private IEnumerator TrackPlayer()
    {
        while (player != null)
        {
            Vector3 targetDirection = (player.position - transform.position).normalized;
            Vector3 newDirection = Vector3.Lerp(rb.velocity.normalized, targetDirection, Time.deltaTime * 3);
            rb.velocity = newDirection * _speed;

            yield return null;
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
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(3,0.5f,0), Quaternion.Euler(0f, 0f, 10f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(-3,-0.5f,0), Quaternion.Euler(0f, 0f, -10f));
                break;
        }
    }
}
