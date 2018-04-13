using UnityEngine;
using System.Collections;

public class ParabolicMovement : MonoBehaviour
{
	public GameObject target;
	public float vSpeed;
	public float duration;

	private float t = 0;
	private float g = 0;
	private float v0 = 0;
	private float xDistance = 0;
	private float yDistance = 0;

	void Start()
	{
		Reset ();
	}

	public void Reset()
	{
		xDistance = target.transform.position.x - transform.position.x;
		yDistance = target.transform.position.y - transform.position.y;
		t = 0;
		v0 = vSpeed * Screen.height;
		g = -2f / duration * v0;

	}

	void Update()
	{
		t += Time.deltaTime;
		var dt = Time.deltaTime / duration;
		if (t > duration) {
			GameObject.Destroy (gameObject);
			return;
		}
		var dx = xDistance * dt;
		var dy = (v0 + g * t) * dt;
		transform.Translate (dx, dy, 0, Space.World);
	}
}
