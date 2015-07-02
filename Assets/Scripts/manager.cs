using UnityEngine;
using System.Collections;

public class manager : MonoBehaviour {

	public GameObject pj;
	public GameObject inst;
	public bool started=false;
	void OnGUI() {
		if (GUI.Button(new Rect(10, 70, 50, 30), "Restart"))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	void Start()
	{
		started=false;
		inst.GetComponent<instantiateTest>().enabled=false;
		pj.GetComponent<Player>().enabled=false;

	}
	void Update()
	{
		if(!started)
		{
			if(Input.GetKeyDown (KeyCode.Space))
			{
				inst.GetComponent<instantiateTest>().enabled=true;
				pj.GetComponent<Player>().enabled=true;
				started=true;
			}
		}
	}
	public void stop()
	{
		inst.GetComponent<instantiateTest>().enabled=false;
		obstacle_movement[] obstaculos=GameObject.FindObjectsOfType<obstacle_movement>();
		foreach(obstacle_movement obstaculo in obstaculos)
		{
			obstaculo.enabled=false;
		}
	}
}

