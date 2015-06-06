using UnityEngine;
using System.Collections;

public class AllEffectsActivator : MonoBehaviour, EffectsActivator
{
    public void Activate()
    {
        var effects = GetComponentsInChildren<Effect>();
        foreach (var effect in effects)
        {
            effect.Activate();
        }
    }
}
