using UnityEngine;
using System.Collections;

public class LogMessage : MonoBehaviour, Effect
{
    [SerializeField]
    string _message = "Bebug";

    public void Do()
    {
        Debug.Log(_message);
    }
}
