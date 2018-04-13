using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

public class ClickToCatch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public void OnPointerDown (PointerEventData eventData)
	{
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		GameObject.Destroy (gameObject);
		var catch_count_text = GameObject.Find ("catch_count").GetComponent<Text> ();
		var catch_count = Convert.ToInt32 (catch_count_text.text);
		catch_count += 1;
		catch_count_text.text = catch_count.ToString ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
