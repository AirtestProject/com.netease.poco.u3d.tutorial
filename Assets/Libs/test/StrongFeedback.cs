using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class StrongFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public void OnPointerDown (PointerEventData eventData)
	{
		transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		transform.localScale = new Vector3 (1f, 1f, 1f);
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
		