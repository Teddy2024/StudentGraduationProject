using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayButton : MonoBehaviour
{
    public string level{ get; set; }
    
    public void PlayButtonClick()
    {
        GameManager.Instance.ChangeGameState(GameState.InPlay);//處理
        LevelManager.Instance.StartPlayScene(level);
    }
}