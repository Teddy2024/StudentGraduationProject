using UnityEngine;
using System;

public class Soda : ItemDefault
{
    [Header("蘇打口味")]
    public Flavor sodaFlavor;

    public override void GetItem()
    {
        switch (sodaFlavor)
        {
            case Flavor.Banana:
            GameManager.Instance.GetPlayer().speed += 0.3f;
            break;
            case Flavor.Strawberry:
            GameManager.Instance.GetPlayer().HealthRecover(25);
            break;
            case Flavor.Watermelon:
            Debug.Log("Watermelon");
            break;
        }
        Destroy(gameObject);
    }
}

[Serializable]
public enum Flavor
{
   Banana,
   Strawberry,
   Watermelon
}
