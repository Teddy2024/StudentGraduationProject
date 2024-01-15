using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class LevelManager : Singleton<LevelManager>
{
    [Header("場景過度動畫")]
    [SerializeField] private Animator trastion;
    [SerializeField] private float traTime;

    public void StartPlayScene(string level)
    {
        StartCoroutine(AnimTrastion(traTime, level));
    }

    private IEnumerator AnimTrastion(float traTime, string level)
    {
        if(trastion != null)trastion.Play("Trastion_Start");
        yield return new WaitForSeconds(traTime);
        DOTween.KillAll();
        SceneManager.LoadScene(level);
    }

    public void BackToMainMenu()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("MainMenu");
    }
}
