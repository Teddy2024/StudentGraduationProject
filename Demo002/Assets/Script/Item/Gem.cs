using UnityEngine;

public class Gem : ItemDefault
{
    [SerializeField] private AudioClip _pickUpSound;
    [SerializeField] private bool _ToLevelUp;

    public override void GetItem()
    {
        AudioManager.Instance.PlaySound(_pickUpSound, 1);
        ExpManager.Instance.AddExperience(_ToLevelUp ? ExpManager.Instance.LevelUpExp : 5 );
        if(!_ToLevelUp)
        {
            EnemyAIManager.Instance.ReleaseGem(this);
        }
        else Destroy(gameObject);
    }
}
