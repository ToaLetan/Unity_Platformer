using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public enum Direction
	{ LEFT, RIGHT, UP, DOWN};

	private const float SCALEDOWN = 0.01f;
	private const float MAXVELOCITY = 15;

	public float velocityX = 0;
	public float accelerationX = 0;
	public float decelerationX = 0;
	public Direction currentDirectionX = Direction.RIGHT;
	public int currentDirXInt = 1;


	void Start () 
	{
		accelerationX = MAXVELOCITY / 5;
		decelerationX = -1 * (accelerationX * 1.5f);

		InputManager.Instance.OnKeyLeft += OnKeyLeft;
		InputManager.Instance.OnKeyRight += OnKeyRight;
		InputManager.Instance.OnKeyJump += OnKeyJump;

		InputManager.Instance.ReleaseKeyLeftRight += ReleaseKeyLeftRight;
		InputManager.Instance.ReleaseKeyJump += ReleaseKeyJump;
	}

	void Update () 
	{
		InputManager.Instance.Update();
	}

	void OnKeyLeft()
	{
		currentDirectionX = Direction.LEFT;
		currentDirXInt = -1;
		PlayerMove(currentDirXInt);
	}

	void OnKeyRight()
	{
		currentDirectionX = Direction.RIGHT;
		currentDirXInt = 1;
		PlayerMove(currentDirXInt);
	}

	void OnKeyJump()
	{
		Debug.Log("jump");
	}

	void ReleaseKeyLeftRight()
	{
		float newPlayerX = this.transform.position.x;

		if(velocityX > 0)
			velocityX = velocityX + decelerationX;
		else
			velocityX = 0;

		newPlayerX += currentDirXInt * velocityX * SCALEDOWN;
		this.transform.position = new Vector3(newPlayerX, this.transform.position.y, this.transform.position.z);

	}

	void ReleaseKeyJump()
	{

	}

	private void PlayerMove(int moveDirection)
	{
		float newPlayerX = this.transform.position.x;

		//VelocityFinal = VelocityInitial + acceleration * time
			if(velocityX < MAXVELOCITY)
				velocityX = velocityX + accelerationX;
			//if(currentMovementState == ONGROUND)
				//animationManager.PlayAnim("Run_" + currentDirXasString);

		/* If no keys being pressed
		 * {
			if(velocityX > 0)
				velocityX = velocityX + decelerationX;
			else
				velocityX = 0;
		}
		*/
		
		//Check if the  player is touching a wall. If so, prevent any movement in that direction.
		/*if((moveDirection == Direction.LEFT && collisionDetection.CheckCollisionDirection(collisionDetection.LEFT)) || 
		   (moveDirection == Direction.RIGHT && collisionDetection.CheckCollisionDirection(collisionDetection.RIGHT)) )
		{
			velocityX = 0;
		}*/
		
		//Speculative contacts for both left and right. Adjust X-Velocity accordingly.
		/*switch(moveDirection)
		{
		case collisionDetection.LEFT:
			if(collisionDetection.CheckRayTrace(new Point(-velocityX, playerObject.height/2), collisionDetection.RAYHORIZONTAL) == true)
			{
				velocityX = x - (collisionDetection.ObjectCollidingWith.x + collisionDetection.ObjectCollidingWith.width);
			}
			break;
		case collisionDetection.RIGHT:
			if(collisionDetection.CheckRayTrace(new Point(playerObject.width + velocityX, playerObject.height/2), collisionDetection.RAYHORIZONTAL) == true)
			{
				velocityX = collisionDetection.ObjectCollidingWith.x - (x + playerObject.width);
			}
			break;
		}*/
		newPlayerX += moveDirection * velocityX * SCALEDOWN;

		//this.transform.position.x += newPlayerX;
		this.transform.position = new Vector3(newPlayerX, this.transform.position.y, this.transform.position.z);

		//collisionDetection.UpdatePosition();
	}
}
