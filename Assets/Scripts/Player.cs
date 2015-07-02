using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float mass=30;
	public float gravity=3f;
	public float jumForce=23f;
	public float orientation=0f;
	public Vector2 velocity;
	public Vector2 aceleration;
	public Vector2 steerforce;
	public bool canJump=true;
	public bool isDead=false;
	public int score=0;
		
	public float maxAceleration=50f;
	public float minAceleration=1.3f;

	private Transform _transform;

	// Use this for initialization
	void Start () {
		_transform = this.GetComponent<Transform> ();
		velocity = Vector2.zero;
		aceleration = Vector2.zero;
		steerforce = Vector2.zero;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		aceleration = steerforce / mass;
		aceleration.y -= gravity;
		if(aceleration.y<0)
		{
			Truncate(ref aceleration,minAceleration);
			//Debug.Log("Min Aceleration:"+aceleration);
		}
		else
		{
			Truncate(ref aceleration,maxAceleration);
			//Debug.Log("Max Aceleration:"+aceleration);
		}
		velocity += aceleration;
		if(velocity.y>0)
		{
			float b= ((velocity.y * 160)/10)>60?60:((velocity.y * 160)/10);
			orientation =Mathf.LerpAngle(orientation,b,Time.deltaTime*5);
		}
		else
		{
			float b=((velocity.y * 160)/10)<-60?-60:((velocity.y * 160)/10);
			orientation = Mathf.LerpAngle(orientation,b,Time.deltaTime*5);
		}
		_transform.position +=new Vector3(velocity.x,velocity.y,0)*Time.deltaTime;
		_transform.rotation = Quaternion.AngleAxis(orientation,Vector3.back);
		steerforce = Vector2.zero;
	
	}
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space) && canJump==true) 
		{
			//steerforce.y+=jumForce;
			velocity.y=jumForce;
		}
		if(isDead==true)
		{

			Vector3 cp = Camera.main.WorldToScreenPoint (_transform.position);
			if(cp.y<Screen.height*(-1))
			{
				FindObjectOfType<manager>().SendMessage("stop");
				Destroy(this.gameObject);
			}
		}

	}
	private void Truncate(ref Vector2 myVector, float myMax) //not above max
	{
		if (myVector.magnitude > myMax)
		{
			myVector.Normalize(); // Vector3.normalized returns this vector with a magnitude of 1
			myVector *= myMax; //scale to max
		}
	}
	void OnTriggerEnter (Collider other) 
	{
		if (other.tag == "obstacle") 
		{
			dead ();
		}
		else
		{

			score+=1;
			TextMesh text=FindObjectOfType<TextMesh>();
			text.text=""+score;
		}
	}
	IEnumerator Shake(float duration,float magnitude) {
		
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = Camera.main.transform.position;
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;          
			
			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;
			
			Camera.main.transform.position = new Vector3(x, y, originalCamPos.z);
			
			yield return null;
		}
		Camera.main.transform.position = originalCamPos;
	}
	private void dead()
	{
		velocity=Vector3.zero;
		canJump=false;
		aceleration=new Vector3(0,-3,0);
		isDead=true;
		Shake(0.5f,2.0f);
	}
}
