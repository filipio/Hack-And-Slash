using System;
using UnityEngine;

public class AnimationImpactWatcher : MonoBehaviour
{
    public event Action OnImpact;
    // called by animation
 private void Impact()
    {
        if(OnImpact != null)
        {
            OnImpact();
        }
    }
}
