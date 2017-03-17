using UnityEngine;
using System.Collections;

public class FadeInOutHandler : MonoBehaviour
{

	[SerializeField]
	Animator animRef;

	[SerializeField]
	GameObject previousPanel;

	[SerializeField]
	GameObject nextPanel;

	[SerializeField]
	float timeToTakeForAnimation;

	// Use this for initialization
	void Start ()
	{
		animRef = GetComponent <Animator> ();
	}

	public void StartFadeInOutBetween ( GameObject prev, GameObject next, float TimeToTake )
	{
		previousPanel = prev;
		nextPanel = next;
		timeToTakeForAnimation = TimeToTake;

		animRef.Play ( "FadeIn" );

	}

	public void TransitionDone ()
	{
		gameObject.SetActive ( false );
	}

}
