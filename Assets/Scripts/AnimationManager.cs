using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour 
{
	private Animator objectAnimator = null;

	// Use this for initialization
	void Start () 
	{
		objectAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void PlayAnimation(string animName)
	{
		objectAnimator.Play(animName);
	}

	public bool CheckIfAnimationIsPlaying(string animName)
	{
		if(objectAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName) )
			return true;
		else
			return false;
	}
}