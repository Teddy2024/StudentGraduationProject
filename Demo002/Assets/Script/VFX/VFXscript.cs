using UnityEngine;

public class VFXscript : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void PlaySomeDead()
    {
        _particleSystem.Play();
        Invoke("DestPop", 1);
    }

    void DestPop() => EnemyAIManager.Instance.ReleaseDead(this);
}
