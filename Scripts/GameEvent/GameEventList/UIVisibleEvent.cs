using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisibleEvent : GameEvent
{
    public bool IsActive;

    [SerializeField] private GameObject UIObj;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        UIObj.SetActive(IsActive);

        yield return null;

        IsEventEnd = true;
    }
}
