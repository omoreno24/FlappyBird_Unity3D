using UnityEngine;
using System.Collections;

public class obstacle_movement : MonoBehaviour {
	public float speed=0.5f;
	private Transform _transform;
	// Use this for initialization
	void Start () {
		_transform = this.GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3 (speed, 0, 0)*Time.deltaTime;
		Vector3 cp = Camera.main.WorldToScreenPoint (_transform.position);
		if(cp.x<Screen.width*(-1) || cp.x>Screen.width*2)
		{
			Destroy(this.gameObject);
		}
	}
}
