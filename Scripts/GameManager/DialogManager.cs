using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : GameManager<DialogManager>
{
    // 전체 화면 다이얼로그
    [Header("Dialogue Box Full")]
    [SerializeField] private GameObject dialogueFullScreen;
    [SerializeField] private TextMeshProUGUI dialogueFullScreenText;

    [Header("Dialogue Box Lower")]
    [SerializeField] private GameObject dialogueLowScreen;
    [SerializeField] private TextMeshProUGUI dialogueLowScreenText;

    [Header("Others")]
    [SerializeField] private Image dialogueImage;

    // 스크린 다이얼로그 감지 변수
    private bool IsdialogueEnd = false;
    public bool IsDialogueEnd 
    { 
        get
        {
            if (IsdialogueEnd)
            {
                IsdialogueEnd = false;
                return true;
            }
            else return false;
        }
    }

    private Queue<string> sentenses;
    private string currentSentense;
    private bool b_TouchSkip = false;
    private bool IscurrentSkip = false;
    private Func<TextMeshProUGUI> dialoguetext;


    protected override void Awake()
    {
        base.Awake();

        IsdialogueEnd = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !b_TouchSkip)
        {
            if (IscurrentSkip)
                b_TouchSkip = true;
        }
    }

    public void DialogueEvent(ScreenDialogue[] data)
    {
        if (sentenses == null) sentenses = new Queue<string>();
        sentenses.Clear();

        SystemManager.isControl = SystemManager.Control.NOTCONTROL;
        StartCoroutine(DialogueEvents(data));
    }

    private IEnumerator DialogueEvents(ScreenDialogue[] data)
    {
        foreach (ScreenDialogue talkDialogue in data)
        {
            // 이미지 여부
            if (talkDialogue.dialogueData.image != null)
            {
                dialogueImage.gameObject.SetActive(true);
                dialogueImage.sprite = talkDialogue.dialogueData.image;
                dialogueImage.color = new Color(dialogueImage.color.r, dialogueImage.color.g, dialogueImage.color.b, 0);

                float t1 = 0f;
                while (dialogueImage.color.a <= 0.99f)
                {
                    yield return null;

                    t1 += Time.deltaTime;
                    dialogueImage.color = Color.Lerp(new Color(dialogueImage.color.r, dialogueImage.color.g, dialogueImage.color.b, 0.0f), new Color(dialogueImage.color.r, dialogueImage.color.g, dialogueImage.color.b, 1.0f), t1);
                }
            }
            
            // 다이얼로그 분류
            if (talkDialogue.IsFullScreen)
            {
                dialogueFullScreen.SetActive(true);
                dialoguetext = FullScreenText;
            }
            else
            {
                dialogueLowScreen.SetActive(true);
                dialoguetext = LowScreenText;
            }
            TextMeshProUGUI TMPro = dialoguetext();
            TMPro.text = "";

            // 스킵 여부
            IscurrentSkip = talkDialogue.IsSkip;

            // 메세지 설정
            foreach (string talk in talkDialogue.dialogueData.s_talk)
            {
                sentenses.Enqueue(talk);
            }

            // 메세지 출력
            while (sentenses.Count > 0)
            {
                currentSentense = sentenses.Dequeue();
                b_TouchSkip = false;

                // 한 글자씩 출력
                for (int i = 0; i < currentSentense.Length; i++)
                {
                    TMPro.text += currentSentense[i];

                    if (!b_TouchSkip) yield return new WaitForSeconds(0.1f);
                }

                TMPro.text += "\n\n";

                if (!IscurrentSkip) yield return new WaitForSeconds(talkDialogue.dialogueData.talkEndWaitTime);
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

            if (talkDialogue.IsFullScreen)
            {
                TMPro.text = "";
                dialogueFullScreen.SetActive(false);
            }
            else
            {
                TMPro.text = "";
                dialogueLowScreen.SetActive(false);
            }

            float t2 = 0f;
            while (dialogueImage.color.a > 0.01f)
            {
                yield return null;

                t2 += Time.deltaTime;
                dialogueImage.color = Color.Lerp(new Color(dialogueImage.color.r, dialogueImage.color.g, dialogueImage.color.b, 1.0f), new Color(dialogueImage.color.r, dialogueImage.color.g, dialogueImage.color.b, 0.0f), t2);
            }
            dialogueImage?.gameObject?.SetActive(false);
        }

        IsdialogueEnd = true;
    }

    private TextMeshProUGUI FullScreenText () { return dialogueFullScreenText; }
    private TextMeshProUGUI LowScreenText () { return dialogueLowScreenText; }
}
