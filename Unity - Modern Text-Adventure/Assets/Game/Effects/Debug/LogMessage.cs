using UnityEngine;
using System.Collections;

public class LogMessage : MonoBehaviour, Effect
{
    [SerializeField]
    string _message = "Bebug";

    public void Activate()
    {
        Debug.Log(_message);
    }
}
