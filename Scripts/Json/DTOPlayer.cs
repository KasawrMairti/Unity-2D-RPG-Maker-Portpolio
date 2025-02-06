using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DTOPlayer
{
    public PlayerStatus status;

    public DTOPlayer(PlayerStatus status)
    {
        this.status = status;
    }
}
