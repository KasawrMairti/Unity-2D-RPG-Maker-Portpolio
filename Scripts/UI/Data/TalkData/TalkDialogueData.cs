using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable] public struct TalkDialogue
{
    [Serializable] public struct TalkData
    {
        public string name;
        public float talkEndWaitTime;
        public string transformPos;
        public Vector2 vectorPos;
        [Multiline] public string[] s_talk;
    }

    public TalkData talkData;
    public float EndWaitTime;
    public bool IsSkip;
}

public class TalkDialogueData : ScriptableObject
{
    public TalkDialogue[] talkDialogues;

    [MenuItem("Window/Data/TalkDialogueData")]
    public static void CreateAssets()
    {
        AssetDatabase.CreateAsset(CreateInstance<TalkDialogueData>(), AssetDatabase.GenerateUniqueAssetPath("Assets/ResourceDeta/Data/DialogueData/Talk/TalkDialog.asset"));
    }
}
