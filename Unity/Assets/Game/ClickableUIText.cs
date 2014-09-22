using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public struct UITExtWordInfo
{
	public Rect localRect;

	public int firstLetterIndex;
	public int length;
}

[RequireComponent(typeof(Text))]
public class ClickableUIText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	
	Text _text;
	List<UITExtWordInfo> _words;
	public List<UITExtWordInfo> Words
	{
		get
		{
			if (_words == null) _words = GatherWords(_text);
			return _words;
		}
	}

	void Awake()
	{
		_text = GetComponent<Text>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		eventData.Use();
		GetWord(eventData.pressEventCamera.ScreenPointToRay(eventData.position));
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		eventData.Use();
	}

	void GetWord(Ray ray)
	{
		Ray localRay = new Ray(
			transform.InverseTransformPoint(ray.origin),
			transform.InverseTransformDirection(ray.direction));

		Vector3 localClickPos =
			localRay.origin +
			localRay.direction / localRay.direction.z * (transform.localPosition.z - localRay.origin.z);

		Debug.DrawRay(transform.TransformPoint(localClickPos), transform.forward / 10.0f, Color.red, 2.0f);

		foreach (var word in Words)
		{
			if (localClickPos.x < word.localRect.xMin && 
				localClickPos.x > word.localRect.xMax &&
				localClickPos.y > word.localRect.yMin &&
				localClickPos.y < word.localRect.yMax
				)
			{
				Debug.Log(_text.text.Substring(word.firstLetterIndex, word.length));
				break;
			}
			else
			{
				Debug.Log("Not this word: " + _text.text.Substring(word.firstLetterIndex, word.length)
					+ "\nRect: " +
					word.localRect.xMin + ", " +
					word.localRect.yMin + ", " +
					word.localRect.xMax + ", " +
					word.localRect.yMax + ", " +
					"; Click: " + localClickPos);
			}
		}
	}

	int IndexOfAnyNotIn(string text, char[] anyNotInThis, int startIndex)
	{
		while (startIndex < text.Length)
		{
			if (anyNotInThis.Contains(text[startIndex]))
			{
				startIndex++;
			}
			else
			{
				return startIndex;
			}
		}
		return text.Length;
	}

	List<UITExtWordInfo> GatherWords(Text text)
	{
		var words = new List<UITExtWordInfo>();

		var textGen = text.cachedTextGenerator;
		var matches = Regex.Matches(text.text, @"[\w]+");
		foreach (Match match in matches)
		{

			Vector2 totUpperLeft = new Vector2(1000000, -1000000);
			Vector2 totBottomRight = new Vector2(-1000000, 1000000);
			for (int i = match.Index; i < match.Index + match.Length; ++i)
			{
				Vector2 locUpperLeft = new Vector2(textGen.verts[i * 4].position.x, textGen.verts[i * 4].position.y);
				Vector2 locBottomRight = new Vector2(textGen.verts[i * 4 + 2].position.x, textGen.verts[i * 4 + 2].position.y);

				if (locUpperLeft.x < totUpperLeft.x) totUpperLeft.x = locUpperLeft.x;
				if (locUpperLeft.y > totUpperLeft.y) totUpperLeft.y = locUpperLeft.y;

				if (locBottomRight.x > totBottomRight.x) totBottomRight.x = locBottomRight.x;
				if (locBottomRight.y < totBottomRight.y) totBottomRight.y = locBottomRight.y;
			}

			words.Add(new UITExtWordInfo()
			{
				firstLetterIndex = match.Index,
				length = match.Length,
				localRect = new Rect(totUpperLeft.x, totUpperLeft.y, totBottomRight.x - totUpperLeft.x, totBottomRight.y - totUpperLeft.y),
			});
		}

		return words;
	}

	void OnDrawGizmos()
	{
		var text = GetComponent<Text>();
		var textGen = text.cachedTextGenerator;
		var prevMatrix = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;
		for (int i = 0; i < textGen.characterCount; ++i)
		{
			Vector2 locUpperLeft = new Vector2(textGen.verts[i * 4].position.x, textGen.verts[i * 4].position.y);
			Vector2 locBottomRight = new Vector2(textGen.verts[i * 4 + 2].position.x, textGen.verts[i * 4 + 2].position.y);

			Vector3 mid = (locUpperLeft + locBottomRight) / 2.0f;
			Vector3 size = locBottomRight - locUpperLeft;

			Gizmos.DrawWireCube(mid, size);

			Vector2 vertOne = new Vector2(textGen.verts[i * 4].position.x, textGen.verts[i * 4].position.y);
			Vector2 vertTwo = new Vector2(textGen.verts[i * 4 + 1].position.x, textGen.verts[i * 4 + 1].position.y);
			Vector2 vertThree = new Vector2(textGen.verts[i * 4 + 2].position.x, textGen.verts[i * 4 + 2].position.y);
			Vector2 vertFour = new Vector2(textGen.verts[i * 4 + 3].position.x, textGen.verts[i * 4 + 3].position.y);
			Gizmos.DrawSphere(vertOne, 3);
			Gizmos.DrawWireSphere(vertTwo, 3);
			Gizmos.DrawCube(vertThree, Vector3.one * 3);
			Gizmos.DrawWireCube(vertFour, Vector3.one * 3);
		}

		Gizmos.color = Color.red;
		var words = GatherWords(text);
		foreach (var word in words)
		{
			Gizmos.DrawWireCube(word.localRect.center, word.localRect.size);

			Vector2 locUpperLeft = new Vector2(word.localRect.xMin, word.localRect.yMin);
			Vector2 locBottomRight = new Vector2(word.localRect.xMax, word.localRect.yMax);

			Gizmos.DrawWireSphere(locUpperLeft, 4);
			Gizmos.DrawWireCube(locBottomRight, Vector3.one * 4);
		}

		Gizmos.matrix = prevMatrix;
	}

}
