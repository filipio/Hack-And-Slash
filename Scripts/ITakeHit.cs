using System;
using UnityEngine;
public interface ITakeHit
{
    event Action OnHit;
    Transform transform { get; }
    void TakeHit(IDamage hitBy);
    bool Alive { get; }
}
