using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/**
 * This class listens to input in it's text field.
 * Then sends it to any TextReactor child object. If multiple reactors want to react, a random one is chosen.
 */
public class TextListener : MonoBehaviour
{
    [SerializeField]
    InputField _input;

    void Start()
    {
        _input.onEndEdit.AddListener(ReactToText);
    }

    void ReactToText(string text)
    {
        // find the reactors that should react
        List<TextHandler> reactors = GetComponentsInChildren<TextHandler>().ToList().FindAll(r => r.ShouldReactTo(text));
        if (reactors.Count > 0)
        {
            reactors[Random.Range(0, reactors.Count)].React();
        }
        _input.text = "";
    }
}
