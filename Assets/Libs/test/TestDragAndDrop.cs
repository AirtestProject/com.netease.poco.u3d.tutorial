using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine.EventSystems;

public class TestDragAndDrop : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

	public void OnDrag (PointerEventData eventData)
	{
		GetComponent<RectTransform> ().pivot.Set (0, 0);
		transform.position = Input.mousePosition;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		transform.localScale = new Vector3 (1f, 1f, 1f);
		var shellBounds = GameObject.Find ("shell").GetComponent<BoxCollider2D> ().bounds;
		Debug.Log (shellBounds);
		Debug.Log (transform.position);
		Debug.Log (shellBounds.Contains (transform.position));
		if (shellBounds.Contains (transform.position)) {
			GetComponent<Image> ().CrossFadeAlpha (0f, 1f, false);
			var score = GameObject.Find ("scoreVal").GetComponent<Text> ();
			var iscore = Convert.ToInt32 (score.text);
			iscore += 20;
			score.text = Convert.ToString (iscore);
		}
	}

	public void Reset(Vector3 pos)
	{
		transform.localPosition = pos;
		GetComponent<Image> ().CrossFadeAlpha (1f, 0.01f, true);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
