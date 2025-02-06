using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct ScreenDialogue
{
    [Serializable]
    public struct ScreenDialogueData
    {
        public float talkEndWaitTime;
        public Sprite image;
        [Multiline] public string[] s_talk;
    }

    public ScreenDialogueData dialogueData;
    public float EndWaitTime;
    public bool IsFullScreen;
    public bool IsSkip;
}

public class ScreenDialogueData : ScriptableObject
{
    public ScreenDialogue[] talkDialogues;

    [MenuItem("Window/Data/ScreenDialogueData")]
    public static void CreateAssets()
    {
        AssetDatabase.CreateAsset(CreateInstance<ScreenDialogueData>(), AssetDatabase.GenerateUniqueAssetPath("Assets/ResourceDeta/Data/DialogueData/Dialogue/ScreenDialog.asset"));
    }
}
