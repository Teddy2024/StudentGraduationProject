using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image _image;
    GameObject playButton;
    string level;

    public void SetButton(LevelData levelData, GameObject playButton)
    {
       level = levelData.BaseStats.sceneName;
       levelText.text = level;
       _image.sprite = levelData.BaseStats.levelSp;
       this.playButton = playButton;
    }

    public void LevelButtonClick()
    {
        playButton.SetActive(true);
        playButton.GetComponent<PlayButton>().level = this.level;
    }
}
