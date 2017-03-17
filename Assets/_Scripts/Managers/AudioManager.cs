using UnityEngine;
using System.Collections;

using States.Answers;
using States.Options;

namespace Managers
{

	public class AudioManager : MonoBehaviour
	{

		[SerializeField]
		private UIManager UIManRef;

		public static AudioClip UIButtonPress;

		public AudioClip OptionButtonPress;

		public AudioClip AnswerButtonPress;

		public AudioClip ImageOutButtonPress;

		public AudioClip ImageInButtonPress;

		public AudioClip CorrectAnswer;

		public AudioClip IncorrectAnswer;

		public AudioClip NextQuestion;

		public AudioClip PowerUp_1;

		public AudioClip PowerUp_2;

		public AudioClip GameLoop;

		public AudioSource UIAudioSource;

		// Use this for initialization
		void Start ()
		{

		}

		void OnEnable ()
		{
			AnswerTextManager.OnOptionButtonPress += PlayOptionUIButtonSound;

			AnswerTextManager.OnAnswerButtonPress += PlayAnswerUIButtonSound;
		}

		void OnDisable ()
		{
			AnswerTextManager.OnOptionButtonPress -= PlayOptionUIButtonSound;

			AnswerTextManager.OnAnswerButtonPress -= PlayAnswerUIButtonSound;
		}

		public void PlayCommonUIButtonSound ()
		{
			Debug.Log ( "Playing UIButtonPress" );

			if ( UIButtonPress != null )
			{
				UIAudioSource.PlayOneShot ( UIButtonPress );
			}

		}

		public void PlayCorrectAnswerSound ()
		{
			Debug.Log ( "Playing CorrectAnswer" );

			if ( CorrectAnswer != null )
			{
				UIAudioSource.PlayOneShot ( CorrectAnswer );
			}

		}

		public void PlayIncorrectAnswerSound ()
		{
			Debug.Log ( "Playing IncorrectAnswer" );

			if ( IncorrectAnswer != null )
			{
				UIAudioSource.PlayOneShot ( IncorrectAnswer );
			}

		}

		public void PlayNextQuestionSound ()
		{

			if ( NextQuestion != null )
			{
				Debug.Log ( "Playing NextQuestion" );
				UIAudioSource.PlayOneShot ( NextQuestion );
			}

		}

		// Use this for initialization of Ans button Delegate
		public void RegisterAnswerButton ( AnswerButtonStateManager ansButtonRef )
		{
			ansButtonRef.OnClickEvent = this.PlayAnswerUIButtonSound;
		}


		void PlayAnswerUIButtonSound ()
		{
			Debug.Log ( "Answer Delegate working" );

			if ( AnswerButtonPress != null )
			{
				UIAudioSource.PlayOneShot ( AnswerButtonPress );
			}

		}

		// Use this for initialization of Ansbutton Delegate
		public void RegisterOptionButton ( OptionButtonStateManager OptButtonRef )
		{
			OptButtonRef.OnClickEvent = this.PlayAnswerUIButtonSound;
		}


		void PlayOptionUIButtonSound ()
		{
			Debug.Log ( "Option Delegate working" );

			if ( OptionButtonPress != null )
			{
				UIAudioSource.PlayOneShot ( OptionButtonPress );
			}

		}

		public void PlayHintImageOutUIButtonSound ()
		{
			Debug.Log ( "ImageOut Delegate working" );

			if ( ImageOutButtonPress != null )
			{
				UIAudioSource.PlayOneShot ( ImageOutButtonPress );
			}

		}

		public void PlayHintImageInUIButtonSound ()
		{
			Debug.Log ( "ImageIn Delegate working" );

			if ( ImageInButtonPress != null )
			{
				UIAudioSource.PlayOneShot ( ImageInButtonPress );
			}

		}

		public void ChangeMuteState ( bool state )
		{
			UIAudioSource.mute = state;
			Debug.Log (string.Format ("Mute State:\t{0}", UIAudioSource.mute));
		}
	}

}