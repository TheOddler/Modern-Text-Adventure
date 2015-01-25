using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {
	
	public static Player Instance { get; private set; }
	
	[SerializeField]
	Text _sentenceText;
	
	void Awake () {
		if (Instance == null) Instance = this;
		else Debug.Log("Multiple Players in the scene, make sure there is exactly one.");
	}
	
	// Use this for initialization
	void Start () {
		_sentenceText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddWord(IWordInfo wordInfo) {
		string word = wordInfo.Word.ToLower().Trim();
		if (_sentenceText.text.Length == 0) {
			word = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word);
		}
		else {
			word = " " + word;
		}
		
		_sentenceText.text += word;
	}
}
