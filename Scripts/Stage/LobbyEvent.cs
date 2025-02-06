using System.Collections;
using UnityEngine;

public class LobbyEvent : MonoBehaviour
{
    [Header("TalkEvent")]
    [SerializeField] private GameEventData gameEventData;


    private void Start()
    {
        StartCoroutine(event_Tutorial());
    }

    private IEnumerator event_Tutorial()
    {
        yield return new WaitUntil(() => !LoadingManager.Instance.IsSceneLoad);

        if (gameEventData != null)
            yield return new WaitUntil(() => GameEventManager.Instance.EventTrigger(gameEventData));
    }
}
