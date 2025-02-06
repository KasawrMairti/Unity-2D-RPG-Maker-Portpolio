using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceEvent : GameEvent
{
    public List<ChoiceData> choiceData;

    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform root;
    [SerializeField] private GameObject prefabsOBJ;
    private bool IsClicked = false;

    private GameEventData gameEventData;
    private List<GameObject> rootObj = new();

    protected override void Initialize()
    {

    }

    protected override IEnumerator Event()
    {
        canvas.SetActive(true);
        root.gameObject.SetActive(true);

        foreach (ChoiceData data in choiceData)
        {
            ChoiceBTN btn = Instantiate(prefabsOBJ, root).GetComponent<ChoiceBTN>();
            btn.Set(data.choiceName, data.gameEventData);
            btn.button.onClick.AddListener(() => GetGameEvent(btn.gameEventData));
            rootObj.Add(btn.gameObject);
        }

        yield return new WaitUntil(() => IsClicked);

        foreach (GameObject obj in rootObj)
        {
            Destroy(obj);
        }
        rootObj.Clear();

        yield return new WaitUntil(() => GameEventManager.Instance.EventTrigger(gameEventData));

        root.gameObject.SetActive(false);
        canvas.SetActive(false);

        IsClicked = false;
        IsEventEnd = true;
    }

    private void GetGameEvent(GameEventData data)
    {
        gameEventData = data;
        IsClicked = true;
    }
}
