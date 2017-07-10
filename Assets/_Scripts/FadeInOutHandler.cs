#define TESTING

using UnityEngine;
using System.Collections;

namespace Effects
{

	public class FadeInOutHandler : MonoBehaviour
	{

		[SerializeField]
		Animator animRef;

		[SerializeField]
		GameObject [] previousPanel;

		[SerializeField]
		GameObject [] nextPanel;

		[SerializeField]
		float timeToTakeForAnimation;

		[SerializeField]
		bool hideNextPanel;

		// Use this for initialization
		void Start ()
		{
			animRef = GetComponent <Animator> ();
		}

		public void StartFadeInOutBetween ( GameObject [] prev, GameObject [] next, float timeToTake = 1.0f, bool hNP = true )
		{
			
			#if TESTING && false
			Debug.Log ( "Switching to\t" + next [ 0 ].name );
			#endif

			gameObject.SetActive ( true );

			previousPanel = prev;
			nextPanel = next;
			timeToTakeForAnimation = timeToTake;

			hideNextPanel = hNP;

			animRef.SetFloat ( "AnimationSpeedMultiplier", ( 1.0f / timeToTake ) );
			animRef.Play ( "Panel_Fade_In" );

			#if UNITY_EDITOR && TESTING && false
			if ( previousPanel.Length > 1 && previousPanel [ 1 ].name.Contains ( "Play" ) )
			{
				previousPanel [ 1 ].SetActive ( false );
				Debug.Log ( "Deactivated Play Screen" );
			}
			#endif
		}

		public void TransitionDone ()
		{
			gameObject.SetActive ( false );
		}

		public void ShowPrev ()
		{
			if ( previousPanel.Length > 0 )
			{
				foreach ( var panel in previousPanel )
				{
					panel.SetActive ( true );
				}
			}
			if ( hideNextPanel )
			{
				foreach ( var panel in nextPanel )
				{
					panel.SetActive ( false );
				}
			}
		}

		public void ShowNext ()
		{
			if ( previousPanel.Length > 0 )
			{
//				Debug.Log ( "In Deactivation Loop" );
				foreach ( var panel in previousPanel )
				{
					panel.SetActive ( false );

					#if UNITY_EDITOR && TESTING && false
					Debug.Log ( "panel name:\t" + panel.name );
					if ( panel.name.Contains ( "Play" ) )
					{
						Debug.Break ();
						Debug.Log ( "Deactivated Play Screen" );
					}
					#endif

				}
			}
			foreach ( var panel in nextPanel )
			{
				panel.SetActive ( true );
			}
		}

	}
}