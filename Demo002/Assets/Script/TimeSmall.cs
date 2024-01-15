using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class TimeSmall : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 0.5f;

    private void Start()
    {
        StartCoroutine(ScaleObject());
    }

    private IEnumerator ScaleObject()
    {
        while (transform.localScale.x > 1)
        {
            // 縮小物體的縮放值
            transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;

            // 等待一幀
            yield return null;
        }
    }
}

