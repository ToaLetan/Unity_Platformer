using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Script shared by all objects that perform movement and require access to variables such as gravity.
public class PlatformerCollision : MonoBehaviour 
{
	public enum Direction
	{ LEFT, RIGHT, UP, DOWN};

	public List<GameObject> ExceptionList = new List<GameObject>();

	private GameObject owner = null;
	public GameObject currentObjCollision = null; //Object currently colliding with the owner

	private Vector2 ownerPos = Vector2.zero;

	// Use this for initialization
	void Start () 
	{
		owner = gameObject;
		ownerPos = transform.position;

		ExceptionList.Add(owner);
	}
	
	// Update is called once per frame
	void Update () 
	{
		ownerPos = transform.position;
	}

	public float GetDistance(float point1, float point2)
	{
		return Mathf.Sqrt( (point1 - point2) * (point1 - point2) );
	}

	public float GetDistance(Vector2 point1, Vector2 point2)
	{
		return Mathf.Sqrt( (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y) );
	}

	public bool CheckRayTrace(Vector2 vectorOrientation, float maxDist = 0.0f) 
	{
		bool collisionResult = false;

		int layerMask = 1 << 8; //bitshifting the index of layer to get a bit mask
		layerMask = ~layerMask; //inverting the bitmask

		RaycastHit2D raycast = Physics2D.Raycast(ownerPos, vectorOrientation, maxDist, layerMask);

		if(raycast)
		{
			collisionResult = true;
			currentObjCollision = raycast.collider.gameObject;
			Debug.DrawRay(ownerPos, vectorOrientation);
		}

		return collisionResult;
	}

	public bool CheckCollisionDirection(Direction direction)
	{
		bool directionResult = false;

		float extraSize = 0.025f;

		float ownerHalfWidth = gameObject.rigidbody2D.renderer.bounds.size.x / 2 + extraSize;
		float ownerHalfHeight = gameObject.rigidbody2D.renderer.bounds.size.y / 2 + extraSize;

		switch(direction)
		{
			case Direction.UP:
				directionResult = CheckRayTrace(Vector2.up, ownerHalfHeight);
				break;
			case Direction.DOWN:
				directionResult = CheckRayTrace(-Vector2.up, ownerHalfHeight);
				break;
			case Direction.LEFT:
				directionResult = CheckRayTrace(-Vector2.right, ownerHalfWidth);
				break;
			case Direction.RIGHT:
				directionResult = CheckRayTrace(Vector2.right, ownerHalfWidth);
				break;
		}
		return directionResult;
	}

}
