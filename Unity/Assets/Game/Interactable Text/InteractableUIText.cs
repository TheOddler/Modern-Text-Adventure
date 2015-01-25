using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System.Text.RegularExpressions;

public class UIWordInfo : IWordInfo {
	
	InteractableUIText _owner;
	
	public string Word { get; set; }
	
	public int Index { get; set; }
	public Rect PosRect { get; set; }
	
	public UIWordInfo (InteractableUIText owner) {
		_owner = owner;
	}
	
	public void ReturnToOwner() {
		_owner.ReturnText(this);
	}
}

[RequireComponent(typeof(Text))]
public class InteractableUIText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	
	public void OnPointerDown(PointerEventData eventData)
	{
		var localPos = GetLocalPointerPosition(eventData);
		
		var word = GetWordInfo(GetComponent<Text>(), localPos);
		
		if (word != null) Player.Instance.AddWord(word);
		
		eventData.Use();
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		eventData.Use();
	}
	
	public UIWordInfo GetWordInfo(Text text, Vector3 localClickPos) {
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
			
			var wordRect = new Rect(totUpperLeft.x, totUpperLeft.y, totBottomRight.x - totUpperLeft.x, totBottomRight.y - totUpperLeft.y);
			
			if (wordRect.Contains(localClickPos, true)) {
				var info = new UIWordInfo(this);
				info.Word = match.Value;
				info.PosRect = wordRect;
				info.Index = match.Index;
				return info;
			}
		}
		
		return null;
	}
	
	public static Vector3 GetLocalPointerPosition(PointerEventData eventData) {
		Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.position);
		
		Ray localRay = new Ray(
			transform.InverseTransformPoint(ray.origin),
			transform.InverseTransformDirection(ray.direction));
		
		Vector3 localPos =
			localRay.origin +
			localRay.direction / localRay.direction.z * (transform.localPosition.z - localRay.origin.z);
		
		return localPos;
	}
	
	
	
	public void ReturnText(UIWordInfo text) {
		
	}
}
