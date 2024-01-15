using UnityEngine;

public class BarRainbow : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    void Update() => ChangeAnim();

    void ChangeAnim()
    {
        if(_anim == null)return;
        int weight = (GameManager.Instance.State == GameState.OnUpgrade) ? 1 : 0;
        _anim.SetLayerWeight(1, weight);
    }
}
