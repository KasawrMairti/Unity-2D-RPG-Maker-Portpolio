using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public GameObject OriginalOBJ;

    public void AnimationEvent(int number)
    {
        OriginalOBJ.GetComponent<IAnimationEvent>().AnimEvent(number);
    }
}

public interface IAnimationEvent
{
    public void AnimEvent(int number);
}