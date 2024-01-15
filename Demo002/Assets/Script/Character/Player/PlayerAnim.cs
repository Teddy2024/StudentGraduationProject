using UnityEngine;

[RequireComponent(typeof(PlayerDefault))]
public class PlayerAnim : MonoBehaviour
{
    [Header("玩家圖片"), SerializeField] private SpriteRenderer _sp;
    [Header("玩家動畫"), SerializeField] private Animator _anim;

    PlayerDefault _playerDefault;
    Material _redFlash;
    Material _defaultMaterial;
    GameObject _healthCanvas;
    public bool dead { private get; set; }

    private void Start() 
    {
        _healthCanvas = GameObject.Find("HealthCanvas");
        _sp = GetComponent<SpriteRenderer>();
        _playerDefault = GetComponent<PlayerDefault>();
        _redFlash = Resources.Load<Material>("RedFlash");
        _defaultMaterial = _sp.material;
    }

    private void Update() => DamageCheck();
   
    private void DamageCheck()
    {
        _sp.material = _playerDefault.IsTakingDamage ? _redFlash : _defaultMaterial;
        if(_playerDefault.movement == Vector2.zero)
        {
            _anim.Play("idle");
        }
        else
        {
            _anim.Play("Walk");
        }
    }
}
