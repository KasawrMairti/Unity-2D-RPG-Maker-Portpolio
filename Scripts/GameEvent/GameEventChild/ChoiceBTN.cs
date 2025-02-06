using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBTN : MonoBehaviour
{
    public TextMeshProUGUI TMPtext;
    public GameEventData gameEventData { get; private set; }

    public Button button { get; private set; }

    private void Awake()
    {
        TMPtext = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    public void Set(string text, GameEventData data)
    {
        TMPtext.text = text;
        gameEventData = data;
    }
}
