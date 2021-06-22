using System;
using UnityEngine;

public interface IDie
{
    //why is it of action with type IDie interface?
    //we are passing the thing that implements OnDied? -> not OnDied, we are passing the thing that implements IDie
    event Action<IDie> OnDied;
    event Action<int, int> OnHealthChanged;
    GameObject gameObject { get; }
}
