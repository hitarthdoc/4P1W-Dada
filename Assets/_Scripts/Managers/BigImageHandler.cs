using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace Managers.Misc
{
	public class BigImageHandler : MonoBehaviour
	{

		private Sprite imageToShow;

		private float timeToTakeForAnim = 0.5f;

		private Animator anim;

		private Image image;

		void Awake ()
		{
			anim = GetComponent <Animator> ();
			image = GetComponent <Image> ();
			Debug.Log ("Init BIH");
		}

		public void ShowBigImage ( Sprite imgToShow, float tTTFA = 0.5f )
		{
			transform.parent.gameObject.SetActive ( true );
//			gameObject.SetActive ( true );

			imageToShow = imgToShow;
			timeToTakeForAnim = tTTFA;

			image.sprite = imgToShow;

			if (anim == null)
			{
				anim = GetComponent <Animator> ();
			}
			anim.SetFloat ( "SpeedMultiplier", ( 1.0f / timeToTakeForAnim ) );
			anim.SetTrigger ( "Pop_Up" );
			anim.ResetTrigger ( "Pop_Down" );
		}

	
		public void HideBigImage ()
		{
//		anim.SetFloat ( "SpeedMultiplier", ( timeToTakeForAnim ) );
			anim.SetTrigger ( "Pop_Down" );
			anim.ResetTrigger ( "Pop_Up" );
		}

		public void HiddenImage ()
		{
			transform.parent.gameObject.SetActive ( false );
//			gameObject.SetActive ( false );
		}

	}
}
