using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChangeScrollValue : MonoBehaviour
{
    [SerializeField] private Scrollbar ScrollbarVertical;

    public IEnumerator Start()
    {
        yield return null;
        ScrollbarVertical.value = 1;
    }

    public void Testing()
    {
        ScrollbarVertical.value = 1;
    }
}
