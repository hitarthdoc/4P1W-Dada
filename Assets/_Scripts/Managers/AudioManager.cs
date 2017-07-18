#undef TESTING

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

            UIAudioSource.clip = GameLoop;
            UIAudioSource.loop = true;
            UIAudioSource.Play ();

        }

        void OnDisable ()
        {
            AnswerTextManager.OnOptionButtonPress -= PlayOptionUIButtonSound;

            AnswerTextManager.OnAnswerButtonPress -= PlayAnswerUIButtonSound;

        }

        public void PlayCommonUIButtonSound ()
        {
            #if TESTING
            Debug.Log ( "Playing UIButtonPress" );
            #endif

            if ( UIButtonPress != null )
            {
                UIAudioSource.PlayOneShot ( UIButtonPress );
            }

        }

        public void PlayCorrectAnswerSound ()
        {
            #if TESTING
            Debug.Log ( "Playing CorrectAnswer" );
            #endif

            if ( CorrectAnswer != null )
            {
                UIAudioSource.PlayOneShot ( CorrectAnswer );
            }

        }

        public void PlayPowerUpClip_1 ()
        {
            #if TESTING
            Debug.Log ( "Playing CorrectAnswer" );
            #endif

            if ( PowerUp_1 != null )
            {
                UIAudioSource.PlayOneShot ( PowerUp_1 );
            }

        }

        public void PlayPowerUpClip_2 ()
        {
            #if TESTING
            Debug.Log ( "Playing CorrectAnswer" );
            #endif

            if ( PowerUp_2 != null )
            {
                UIAudioSource.PlayOneShot ( PowerUp_2 );
            }

        }

        public void PlayIncorrectAnswerSound ()
        {
            #if TESTING
            Debug.Log ( "Playing IncorrectAnswer" );
            #endif

            if ( IncorrectAnswer != null )
            {
                UIAudioSource.PlayOneShot ( IncorrectAnswer );
            }

        }

        public void PlayNextQuestionSound ()
        {

            if ( NextQuestion != null )
            {
                #if TESTING
                Debug.Log ( "Playing NextQuestion" );
                #endif
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
            #if TESTING
            Debug.Log ( "Answer Delegate working" );
            #endif

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
            #if TESTING
            Debug.Log ( "Option Delegate working" );
            #endif

            if ( OptionButtonPress != null )
            {
                UIAudioSource.PlayOneShot ( OptionButtonPress );
            }

        }

        public void PlayHintImageOutUIButtonSound ()
        {
            #if TESTING
            Debug.Log ( "ImageOut Delegate working" );
            #endif

            if ( ImageOutButtonPress != null )
            {
                UIAudioSource.PlayOneShot ( ImageOutButtonPress );
            }

        }

        public void PlayHintImageInUIButtonSound ()
        {
            #if TESTING
            Debug.Log ( "ImageIn Delegate working" );
            #endif

            if ( ImageInButtonPress != null )
            {
                UIAudioSource.PlayOneShot ( ImageInButtonPress );
            }

        }

        public void ChangeMuteState ( bool state )
        {
            #if TESTING
            Debug.Log (string.Format ("Mute State:\t{0}", UIAudioSource.mute));
            #endif
            UIAudioSource.mute = state;
        }
    }

}
