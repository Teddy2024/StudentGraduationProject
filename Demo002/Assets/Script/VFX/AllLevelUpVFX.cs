using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLevelUpVFX : MonoBehaviour
{
    [ContextMenuItem("CatchAllVFX", "CatchAllVFX")]
    [SerializeField] private List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

    void CatchAllVFX()
    {
        _particleSystems.Clear();
        foreach(Transform children in gameObject.transform)
        {
            var vfx = children.gameObject.GetComponent<ParticleSystem>();
            _particleSystems.Add(vfx);
        }
    }
}
