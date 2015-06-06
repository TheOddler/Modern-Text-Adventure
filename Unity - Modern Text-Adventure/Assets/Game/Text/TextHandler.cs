using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(EffectsActivator))]
public class TextHandler : MonoBehaviour
{
    [SerializeField]
    string _text;

    public void React()
    {
        GetComponent<EffectsActivator>().Activate();
    }

    public bool ShouldReactTo(string text)
    {
        return string.Equals(_text, text);
    }
}
