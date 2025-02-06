using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    bool b_IsDamageble();

    void Hit(IDamageble target);
    void Damaged(float life);
}
