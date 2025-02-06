using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkDialogueEvent : GameEvent
{
    public TalkDialogueData talkDialogues;
    public bool IsWait = true;

    [SerializeField] private GameObject chatboxPrefeb;
    private List<ChatSystem> chatSystem = new List<ChatSystem>();
    private ChatSystem chatSystemCur;

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        bool chatNoHave = true;
        foreach (var chat in chatSystem)
        {
            if (!chat.isActive)
            {
                chatSystemCur = chat;
                chat.gameObject.SetActive(true);
                chat.Ondialogue(talkDialogues.talkDialogues);
                chatNoHave = false;
                break;
            }
        }

        if (chatNoHave)
        {
            chatSystemCur = Instantiate(chatboxPrefeb).GetComponent<ChatSystem>();
            chatSystemCur.Ondialogue(talkDialogues.talkDialogues);
            chatSystem.Add(chatSystemCur);
        }

        yield return new WaitUntil(() => !IsWait || chatSystemCur.Trigger());

        IsEventEnd = true;
    }
}
