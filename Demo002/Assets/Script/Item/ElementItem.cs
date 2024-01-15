using UnityEngine;

public class ElementItem : ItemDefault
{
    [SerializeField] private SpriteRenderer _sp;
    [SerializeField] private ElementData elementData;

    private void Start() 
    {
        if(elementData == null)GetRandomElement();
        _sp.sprite = elementData.BaseStats.elementSp;
    }

    public override void GetItem()
    {
        DictionaryManager.Instance.ElementAdd(elementData);
        DictionaryManager.Instance.CheckSpawnElementGrid(elementData);
        Destroy(gameObject);
    }

    void GetRandomElement()
    {
        var eleList = ResourceManager.Instance.ElementDataLibrary;
        elementData = eleList[Random.Range(0, eleList.Count)];
    }
}
