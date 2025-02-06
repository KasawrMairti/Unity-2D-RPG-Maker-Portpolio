using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDialogueEvent : GameEvent
{
    public ScreenDialogueData screenDialogues;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        DialogManager.Instance.DialogueEvent(screenDialogues.talkDialogues);
        yield return new WaitUntil(() => DialogManager.Instance.IsDialogueEnd);
        IsEventEnd = true;
    }
}
