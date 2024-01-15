using UnityEngine;
using UnityEngine.UI;

public class ChangeGridLayout : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    Vector2 oringalCellSize;

    void OnEnable() 
    {
        GameManager.OnGameStateChange += OnLevelUpOrPaused;
        oringalCellSize = _gridLayoutGroup.cellSize;
    }

    void OnDestroy() 
    {
        GameManager.OnGameStateChange -= OnLevelUpOrPaused;
    }

    private void OnLevelUpOrPaused(GameState state)
    {
        if(state == GameState.OnUpgrade || state == GameState.ExitMenu)
        {
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.constraintCount = 1;
            _gridLayoutGroup.cellSize = new Vector2(80,80);
        }
        else
        {
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.constraintCount = 4;
            _gridLayoutGroup.cellSize = oringalCellSize;
        }
    }
}
