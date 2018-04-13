using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ListViewItemClickable : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (eventData.delta.x == 0 && eventData.delta.y == 0) {
			var current_item = GetComponent<Text> ().text;
			GameObject.Find ("list_view_current_selected_item_name").GetComponent<Text> ().text = current_item;
		}
	}
}
