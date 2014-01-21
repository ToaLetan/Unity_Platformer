using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private const float SCALEDOWN = 0.01f;
	private const float MAXVELOCITY = 5;
	private const float JUMPVELOCITY = 5;
	private const float MAXJUMPHEIGHT = 2;
	private const float MAXFALLSPEED = -20;

	public enum PlayerMovementState 
	{ OnGround, Midair };

	private PlatformerCollision collisionScript = null;

	//Horizontal movement variables
	public float velocityX = 0;
	public float accelerationX = 0;
	public float decelerationX = 0;
	public PlatformerCollision.Direction currentDirectionX = PlatformerCollision.Direction.RIGHT;
	public int currentDirXInt = 1;

	//Vertical movement variables
	public float velocityY = 0;
	public float accelerationY = 0;
	public float gravity = 0;
	public float distTravelledY = 0;
	public float prevY = 0;
	public PlatformerCollision.Direction currentDirectionY = PlatformerCollision.Direction.DOWN;
	private PlayerMovementState currentMovementState = PlayerMovementState.OnGround;

	//Player sprite variables
	public float width = 0;
	public float height = 0;

	private Vector3 resetPos = Vector3.zero;

	private bool pressedJumpKey = false;
	private bool releasedJumpKey = true; //Used to prevent jump spamming
	private bool isJumping = false;

	void Start () 
	{
		collisionScript = this.GetComponentInChildren<PlatformerCollision>();

		accelerationX = MAXVELOCITY / 5;
		decelerationX = -1 * (accelerationX * 1.5f);

		accelerationY = 5;
		gravity = 1 * accelerationY;

		resetPos = gameObject.transform.position;

		width = gameObject.rigidbody2D.renderer.bounds.size.x;
		height = gameObject.rigidbody2D.renderer.bounds.size.y;

		InputManager.Instance.OnKeyLeft += OnKeyLeft;
		InputManager.Instance.OnKeyRight += OnKeyRight;
		InputManager.Instance.OnKeyJump += OnKeyJump;
		InputManager.Instance.OnKeyReset += OnKeyReset;

		InputManager.Instance.ReleaseKeyLeftRight += ReleaseKeyLeftRight;
		InputManager.Instance.ReleaseKeyJump += ReleaseKeyJump;
	}

	void Update () 
	{
		InputManager.Instance.Update();

		if(isJumping)
			PlayerJump();
	}

	void OnKeyLeft()
	{
		currentDirectionX = PlatformerCollision.Direction.LEFT;
		currentDirXInt = -1;
		PlayerMove(currentDirXInt);
	}

	void OnKeyRight()
	{
		currentDirectionX = PlatformerCollision.Direction.RIGHT;
		currentDirXInt = 1;
		PlayerMove(currentDirXInt);
	}

	//if(pressedJumpKey && releasedJumpKey && !isJumping && currentMovementState == ONGROUND)
	void OnKeyJump()
	{
		if(releasedJumpKey && !isJumping)
		{
			isJumping = true;
			gameObject.rigidbody2D.gravityScale = 0;
			pressedJumpKey = true;
			releasedJumpKey = false;
			prevY = transform.position.y;
			velocityY = JUMPVELOCITY;
		}
	}

	void OnKeyReset()
	{
		gameObject.rigidbody2D.gravityScale = gravity;
		gameObject.transform.position = resetPos;
	}

	void ReleaseKeyJump()
	{
		pressedJumpKey = false;
		releasedJumpKey = true;
	}

	void ReleaseKeyLeftRight()
	{
		float newPlayerX = this.transform.position.x;

		if(velocityX > 0)
			velocityX = velocityX + decelerationX;
		else
			velocityX = 0;

		newPlayerX += currentDirXInt * velocityX * Time.deltaTime;
		this.transform.position = new Vector3(newPlayerX, this.transform.position.y, this.transform.position.z);

	}

	private void PlayerMove(int moveDirection)
	{
		float newPlayerX = this.transform.position.x;

		//VelocityFinal = VelocityInitial + acceleration * time
		if(velocityX < MAXVELOCITY)
			velocityX = velocityX + accelerationX;
		//if(currentMovementState == ONGROUND)
		//animationManager.PlayAnim("Run_" + currentDirXasString);

		//Check if the  player is touching a wall. If so, prevent any movement in that direction.
		if( (currentDirectionX == PlatformerCollision.Direction.LEFT && collisionScript.CheckCollisionDirection(PlatformerCollision.Direction.LEFT) ) || 
		   (currentDirectionX == PlatformerCollision.Direction.RIGHT && collisionScript.CheckCollisionDirection(PlatformerCollision.Direction.RIGHT) ) )
		{
			velocityX = 0;
		}

		//Speculative contacts for both left and right. Adjust X-Velocity accordingly.
		float sideOffset = width/2 + (velocityX * Time.deltaTime);

		if(collisionScript.CheckRayTrace(Vector2.right * moveDirection, sideOffset) )
		{
			//Apply speculative contacts by setting the velocity to be the distance between this object and the colliding object.
			float collObjWidth = collisionScript.currentObjCollision.renderer.bounds.size.x;
			float collObjPosX = collisionScript.currentObjCollision.transform.position.x;

			velocityX = collisionScript.GetDistance(collObjPosX + collObjWidth, gameObject.transform.position.x + width) * Time.deltaTime;
		}

		newPlayerX += moveDirection * velocityX * Time.deltaTime;

		this.transform.position = new Vector3(newPlayerX, this.transform.position.y, this.transform.position.z);
	}

	private void PlayerJump()
	{
		float newPlayerY = this.transform.position.y;

		if(distTravelledY <= -MAXJUMPHEIGHT || !pressedJumpKey)
		{
			//isJumping = false;
			gameObject.rigidbody2D.gravityScale = gravity;
			velocityY = 0;
			//applyGravity = true;

			//Perform a downwards speculative contacts check
			float sideOffset = height/2 + (velocityY * Time.deltaTime);
			
			if(collisionScript.CheckRayTrace(-Vector2.up, sideOffset) )
			{
				//Apply speculative contacts by setting the velocity to be the distance between this object and the colliding object.
				float collObjHeight = collisionScript.currentObjCollision.renderer.bounds.size.y;
				float collObjPosY = collisionScript.currentObjCollision.transform.position.y;
				
				velocityY = collisionScript.GetDistance(collObjPosY + collObjHeight, gameObject.transform.position.y + height) * Time.deltaTime;
			}

			//Check if the  player is touching the ground. If so, prevent any further movement in that direction and stop jumping.
			if(collisionScript.CheckCollisionDirection(PlatformerCollision.Direction.DOWN) )
			{
				isJumping = false;
				gameObject.rigidbody2D.gravityScale = gravity;
				velocityY = 0;
				distTravelledY = 0;
			}
		}
		else
		{
			velocityY = JUMPVELOCITY;

			//Perform an upwards speculative contacts check
			float sideOffset = height/2 + (velocityY * Time.deltaTime);


			if(collisionScript.CheckRayTrace(Vector2.up, sideOffset) )
			{
				//Apply speculative contacts by setting the velocity to be the distance between this object and the colliding object.
				float collObjHeight = collisionScript.currentObjCollision.renderer.bounds.size.y;
				float collObjPosY = collisionScript.currentObjCollision.transform.position.y;
				
				velocityY = collisionScript.GetDistance(collObjPosY + collObjHeight, gameObject.transform.position.y + height) * Time.deltaTime;
				isJumping = false;
				gameObject.rigidbody2D.gravityScale = gravity;
				distTravelledY = 0;
			}

			//Check if the  player is touching the ceiling. If so, prevent any further movement in that direction and stop jumping.
			if(collisionScript.CheckCollisionDirection(PlatformerCollision.Direction.UP) )
			{
				isJumping = false;
				gameObject.rigidbody2D.gravityScale = gravity;
				velocityY = 0;
				distTravelledY = 0;
			}

			newPlayerY += velocityY * Time.deltaTime;
			this.transform.position = new Vector3(this.transform.position.x, newPlayerY, this.transform.position.z);
			
			distTravelledY += (prevY - this.transform.position.y);
			
			prevY = this.transform.position.y;
		}
	}

	/*void OnCollisionEnter2D(Collision2D colliderObj)
	{
		if(colliderObj.gameObject.tag == "Level")
		{
			if(isJumping)
				isJumping = false;
			if(velocityY < 0)
				velocityY = 0;
			if(distTravelledY < 0)
				distTravelledY = 0;
		}
	}*/

}
