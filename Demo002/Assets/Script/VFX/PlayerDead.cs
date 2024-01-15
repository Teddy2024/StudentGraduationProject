using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] private float time = 1.5f;
    private void Start() 
    {
        Invoke("Nothing", time);
    }

    void Nothing()
    {
        GameManager.Instance.ChangeGameState(GameState.Lose);
    }
}
