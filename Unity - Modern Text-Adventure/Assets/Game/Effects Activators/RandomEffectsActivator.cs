using UnityEngine;
using System.Collections;
using System.Linq;

public class RandomEffectsActivator : MonoBehaviour, EffectsActivator
{
    public void Activate()
    {
        var effects = GetComponentsInChildren<Effect>();
        effects[Random.Range(0, effects.Length)].Activate();
    }
}
