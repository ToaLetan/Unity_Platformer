using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour 
{
	public GameObject FollowTarget = null;

	private Vector3 targetPos = Vector3.zero;

	private float cameraZPos = 0;

	// Use this for initialization
	void Start () 
	{
		cameraZPos = this.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//TODO: Maybe make camera a more gradual follower instead of direct
		targetPos = FollowTarget.transform.position;
		this.transform.position = new Vector3(FollowTarget.transform.position.x, FollowTarget.transform.position.y, cameraZPos);
	}
}
