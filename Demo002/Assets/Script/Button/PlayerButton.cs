using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    [SerializeField] Image icon;
    public PlayerDataBase CurrentPlayer { private get; set; }
    GameObject selectButton;
    StudentPanel studentPanel;

    private void Start() 
    {
        studentPanel =  GameObject.FindObjectOfType<StudentPanel>();
    }
    
    public void SetButton(PlayerDataBase playerDataBase, GameObject selectButton)
    {
        CurrentPlayer = playerDataBase;
        icon.sprite = playerDataBase.BaseStats.playerSp;
        this.selectButton = selectButton;
    }

    public void PlayerButtonClick()
    {
        GameManager.Instance._playerData = CurrentPlayer;//處理
        studentPanel.SetStudentPanel(CurrentPlayer);
        selectButton.SetActive(true);
    }
}
