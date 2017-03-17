#undef TESTING

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UnityEngine.Audio;

namespace Managers
{

	public class UIManager : MonoBehaviour
	{
		const string commonButton = "common button";

		const string nextLevelButton = "next level button";

		const string smallImageButton = "small image button";

		const string bigImageButton = "big image button";

		const string levelComplete = "level complete";

		const string muteToggle = "MuteToggle";

		enum UIStates
		{
			MainMenu,
			PlayScreen,
			LevelComplete,
			ExitPanel,
			Default
		}

		public GameObject MainMenu;

		public GameObject PlayScreen;

		public GameObject NotificationPanel;

		public GameObject LevelCompletePanel;

		public GameObject ExitPanel;

		public GameObject BigImagePanel;

		public Image BigImageReference;

		public AudioSource UIAudioSource;

		public AudioMixer Mixer;

		public AudioManager AudioManRef;

		public Toggle SoundToggleRef;

		public Spawner SpawnerReference;

		public delegate void StartGame ();

		public event StartGame OnStartGame;
		public event StartGame OnStartNextLevel;

		UIStates current = UIStates.Default;
		UIStates previous = UIStates.Default;

		// Use this for initialization
		void Start ()
		{

			if ( PlayerPrefs.HasKey ( muteToggle ) )
			{
				if ( PlayerPrefs.GetInt ( muteToggle ) == 1 )
				{
					AudioManRef.ChangeMuteState ( true );
					SoundToggleRef.isOn = false;
				}

				if ( PlayerPrefs.GetInt ( muteToggle ) == 0 )
				{
					AudioManRef.ChangeMuteState ( false );
					SoundToggleRef.isOn = true;
				}

			}
			else
			{
				PlayerPrefs.SetInt ( muteToggle, 0 );
				
				AudioManRef.ChangeMuteState ( false );
			}
			PlayerPrefs.Save ();
			NoMenus ();
			ToMainMenu ();
			current = previous = UIStates.MainMenu;
		}

		void OnEnable ()
		{
			AnswerTextManager.OnLevelComplete += ToLevelCompletePanel;

			InputManager.OnEscapePressed += EscapePressed;
		}

		void OnDisable ()
		{
			AnswerTextManager.OnLevelComplete -= ToLevelCompletePanel;

			InputManager.OnEscapePressed -= EscapePressed;
		}

		public void NoMenus ()
		{
			MainMenu.SetActive ( false );
			PlayScreen.SetActive ( false );
			NotificationPanel.SetActive ( false );
			LevelCompletePanel.SetActive ( false );
			ExitPanel.SetActive ( false );

		}


		public void ToMainMenu ()
		{
			MainMenu.SetActive ( true );
			current = UIStates.MainMenu;

			PlayScreen.SetActive ( false );
			NotificationPanel.SetActive ( false );
			ExitPanel.SetActive ( false );
		}

		public void ToPlayScreen ()
		{
			PlayScreen.SetActive ( true );
			NotificationPanel.SetActive ( true );

			current = UIStates.PlayScreen;

			LevelCompletePanel.SetActive ( false );

			MainMenu.SetActive ( false );
			ExitPanel.SetActive ( false );

			if ( OnStartGame != null )
			{
				OnStartGame ();
			}

			PlaySound ( commonButton );
			PlaySound ( nextLevelButton );

		}

		public void ToExitPanel ()
		{
			ExitPanel.SetActive ( true );
			current = UIStates.ExitPanel;

			MainMenu.SetActive ( false );

			PlaySound ( commonButton );

		}

		public void OnSmallImagePressed ( int selfIndex )
		{
			BigImageReference.sprite = SpawnerReference.GetImageatindex ( selfIndex );
			BigImagePanel.SetActive ( true );

			PlaySound ( smallImageButton );

		}

		public void OnBigImagePressed ()
		{
			BigImagePanel.SetActive ( false );

			PlaySound ( bigImageButton );

		}

		public void ToLevelCompletePanel ()
		{
			LevelCompletePanel.SetActive ( true );
			current = UIStates.LevelComplete;

			PlayScreen.SetActive ( true );
			NotificationPanel.SetActive ( true );

			MainMenu.SetActive ( false );
			ExitPanel.SetActive ( false );

			PlaySound ( levelComplete );
		}

		public void ToNextLevel ()
		{
			PlayScreen.SetActive ( true );
			NotificationPanel.SetActive ( true );

			current = UIStates.PlayScreen;

			LevelCompletePanel.SetActive ( false );

			MainMenu.SetActive ( false );
			ExitPanel.SetActive ( false );

			if ( OnStartNextLevel != null )
			{
				OnStartNextLevel ();
			}

			PlaySound ( nextLevelButton );
		}

		public void SoundToggle ( bool SoundEnabled )
		{
			#if UNITY_EDITOR && TESTING
			Debug.Log ( string.Format ( "mute State:\t{0}\tToggleState:\t{1}" +
					"\nif ( !( PlayerPrefs.GetInt ( muteToggle ) == 0 ) & !SoundEnabled ) = {2}" +
					"\nif ( !( PlayerPrefs.GetInt ( muteToggle ) == 0 ) & !SoundEnabled ) = {3}",
				                           ( ( PlayerPrefs.GetInt ( muteToggle ) == 0 ) ? "False" : "True" ), SoundEnabled,
				                           ( ( PlayerPrefs.GetInt ( muteToggle ) == 0 ) & !SoundEnabled ),
				                           ( ( PlayerPrefs.GetInt ( muteToggle ) == 1 ) & SoundEnabled ) ) );
			#endif

			if ( ( PlayerPrefs.GetInt ( muteToggle ) == 0 ) & !SoundEnabled )
			{
				#if UNITY_EDITOR && TESTING
//				Debug.Log ( "Here 1" );
				#endif
				PlayerPrefs.SetInt ( muteToggle, 1 );
			}
			else
			if ( ( PlayerPrefs.GetInt ( muteToggle ) == 1 ) & SoundEnabled )
			{
				PlayerPrefs.SetInt ( muteToggle, 0 );
			}
			PlayerPrefs.Save ();

			AudioManRef.ChangeMuteState ( !SoundEnabled );

		}

		private void PlaySound ( string soundEffectToPlay )
		{
			switch ( soundEffectToPlay )
			{
				case commonButton:
					AudioManRef.PlayCommonUIButtonSound ();
					break;

				case levelComplete:
					AudioManRef.PlayCorrectAnswerSound ();
					break;

				case nextLevelButton:
					AudioManRef.PlayNextQuestionSound ();
					break;

				case smallImageButton:
					AudioManRef.PlayHintImageInUIButtonSound ();
					break;

				case bigImageButton:
					AudioManRef.PlayHintImageOutUIButtonSound ();
					break;

				default:
					break;
			}
		}

		public void EscapePressed ()
		{
			switch ( current )
			{
				
				case UIStates.MainMenu:
					ToExitPanel ();
					break;
			
				case UIStates.PlayScreen:
					ToMainMenu ();
					break;

				case UIStates.LevelComplete:
					break;
				
				case UIStates.ExitPanel:
					ToMainMenu ();
					break;
				
				case UIStates.Default:
					#if UNITY_EDITOR
					Debug.Log ( "initialization Prob" );
					#endif
					break;
				
				default:
					#if UNITY_EDITOR
					Debug.Log ( "Bigger Prob" );
					#endif
					break;
			}
		}
	}
}