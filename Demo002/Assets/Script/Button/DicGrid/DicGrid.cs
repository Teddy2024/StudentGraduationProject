using UnityEngine;
using UnityEngine.UI;

public class DicGrid<T> : MonoBehaviour
{
    public Image icon;
    public T CurrentData { get; set; }

    public virtual void SetButton(T DataBase){}
    public virtual void DicGridButtonClick(){}
}
