using UnityEngine;

public class TextReactor : Reactor
{

    [SerializeField]
    string _text;

    public bool WillReactTo(string text)
    {
        return string.Equals(_text, text);
    }

}
