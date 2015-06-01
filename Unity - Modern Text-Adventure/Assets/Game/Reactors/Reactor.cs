using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Reactor : MonoBehaviour
{

    [SerializeField]
    List<Effect> _effects;

    void Start()
    {
        GatherEffects();
    }

    protected void GatherEffects()
    {
        _effects = GetComponentsInChildren<Effect>().ToList();
    }

    public void React()
    {
        foreach (Effect eff in _effects)
        {
            eff.Do();
        }
    }

}
