using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public abstract class ItemDefault : MonoBehaviour, IPickupable
{
    [Header("回收速度"), SerializeField] private float _speed = 5;
    Transform _playerTransform;
    bool already;

    private void OnEnable() 
    {
        already = false;
    }
    private void OnDisable() 
    {
        transform.DOKill(false);
    }
    
    public virtual void GetItem(){}
    public void OnPickUp() => MoveBackToPlayer();

    private void MoveBackToPlayer()
    {
        if(!already)_playerTransform = GameManager.Instance.GetPlayer()?.transform;
        if(_playerTransform != null)
        {   
            already = true;
            transform.DOLocalMove(_playerTransform.position, _speed).SetSpeedBased(true).OnComplete(() => 
            {
                GetItem();
            });
        }
    }
}
