using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	[SerializeField]
	private float speed = 1.0f;
	public float CriticalDownCameraPosition = 3.5f;

	[SerializeField]
	private Transform target;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	private void Awake () 
	{
		if (!target) target = FindObjectOfType<Bowman_Character> ().transform;
	}

	private void FixedUpdate()
	{
		Vector3 position = target.position;
		position.x = position.x + FindObjectOfType<Bowman_Character>().speed*FindObjectOfType<Bowman_Character>().hspeed*0.8f;
		//if (position.x > transform.position.x) убрал строка не дает камере перемещаться назад
		{
			position.z = -10.0f;
			if ((target.position.y + 1) > CriticalDownCameraPosition)
				position.y = target.position.y + 1;
			else position.y = CriticalDownCameraPosition;
			transform.position = Vector3.Lerp (transform.position, position, speed * Time.deltaTime);
		}
	}
}
