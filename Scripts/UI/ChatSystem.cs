using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ChatSystem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer BOXsentenses;
    [SerializeField] private TextMeshPro TMPsentenses;
    [SerializeField] private SpriteRenderer BOXname;
    [SerializeField] private TextMeshPro TMPname;

    private Queue<string> sentenses;
    private string currentSentense;
    private Transform originalObject;

    [HideInInspector] public bool isActive = false;
    private bool b_TouchSkip = false;
    private bool IscurrentSkip = false;

    private bool IsEvent = false;
    private bool IsEventEnd = false;
    public bool Trigger()
    {
        if (IsEvent)
        {
            if (IsEventEnd)
            {
                IsEventEnd = false;
                return true;
            }
            
            return false;
        }
        else
        {
            IsEvent = true;

            return false;
        }
    }

    private void OnEnable()
    {
        if (sentenses == null) sentenses = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !b_TouchSkip)
        {
            if (isActive && IscurrentSkip)
                b_TouchSkip = true;
        }

        if (originalObject != null)
        {
            BOXsentenses.transform.position = 
                    new Vector2(originalObject.transform.position.x, 
                    originalObject.transform.position.y + 1.0f + TMPsentenses.preferredHeight * 0.35f);

            BOXname.transform.position =
                new Vector2(((BOXsentenses.transform.position.x - (BOXsentenses.size.x * 0.5f)) + (BOXname.size.x * 0.5f)),
                ((BOXsentenses.transform.position.y + (BOXsentenses.size.y * 0.5f)) + (BOXname.size.y * 0.5f)));
        }
    }

    public void Ondialogue(TalkDialogue[] talkDialogue)
    {
        // Initialize
        TMPname.text = "";
        TMPsentenses.text = "";
        sentenses.Clear();
        b_TouchSkip = false;
        IscurrentSkip = false;

        isActive = true;

        StartCoroutine(DialogueFlow(talkDialogue));
    }

    IEnumerator DialogueFlow(TalkDialogue[] talkDialogues)
    {
        yield return null;

        foreach (TalkDialogue talkDialogue in talkDialogues)
        {
            // 스킵 여부
            IscurrentSkip = talkDialogue.IsSkip;

            // name 크기 설정
            string name = talkDialogue.talkData.name;
            if (name != "")
            {
                BOXname.gameObject.SetActive(true);
                TMPname.text = talkDialogue.talkData.name;
                BOXname.size =
                    new Vector2(TMPname.preferredWidth + 0.4f, TMPsentenses.preferredHeight + 0.7f);
            }
            else BOXname.gameObject.SetActive(false);

            // 메세지 설정
            foreach (string talk in talkDialogue.talkData.s_talk)
            {
                sentenses.Enqueue(talk);
            }

            // 메세지 위치 고정
            if (talkDialogue.talkData.transformPos != "")
                originalObject = ObjectManager.objects[talkDialogue.talkData.transformPos].transform;

            // 메세지 출력
            while (sentenses.Count > 0)
            {
                TMPsentenses.text = "";
                currentSentense = sentenses.Dequeue();
                b_TouchSkip = false;

                // 한 글자씩 출력
                for (int i = 0; i < currentSentense.Length; i++)
                {
                    TMPsentenses.text += currentSentense[i];

                    BOXsentenses.size = 
                        new Vector2(TMPsentenses.preferredWidth + 0.2f, TMPsentenses.preferredHeight + 0.45f);

                    if (!b_TouchSkip) yield return new WaitForSeconds(0.1f);
                }

                if (!IscurrentSkip) yield return new WaitForSeconds(talkDialogue.talkData.talkEndWaitTime);
                else yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }


            if (IscurrentSkip)
            {
                if (talkDialogue.EndWaitTime != 0)
                {
                    float startTime = Time.time;
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Time.time - startTime > talkDialogue.EndWaitTime);
                }
                else yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }
            else yield return new WaitForSeconds(talkDialogue.EndWaitTime);
        }
        IsEventEnd = true;
        isActive = false;
        gameObject.SetActive(false);
    }
}
