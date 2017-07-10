#define TESTING

using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using UnityEngine.UI;

using UnityEngine.Audio;

using SO.Progress;
using SO.Money;

using Effects;

using Managers.Misc;

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
			GameOver,
			ExitPanel,
			Default
		}

		[SerializeField]
		private ProgressScriptableObject PSO;

		[SerializeField]
		private MoneyScriptableObject MSO;

		public GameObject MainMenu;

		public GameObject PlayScreen;

		public GameObject NotificationPanel;

		public GameObject LevelCompletePanel;

		public GameObject GameOverPanel;

		public GameObject ExitPanel;

		public BigImageHandler BigImagePanel;

		public Text CurrentLevelTextInNotificationPanel;

		public Text CurrentLevelTextInMainMenu;

		public Text CoinsText;

		public Image BigImageReference;

		public AudioSource UIAudioSource;

		public AudioMixer Mixer;

		public AudioManager AudioManRef;

		public Toggle SoundToggleRef;

		public Spawner SpawnerReference;

		public FadeInOutHandler FadeManager;

		public delegate void StartGame ();

		public event StartGame OnStartGame;
		public event StartGame OnStartNextLevel;

		public delegate void PowerUps ( SO.Levels.Level currentLevel );

		public static event PowerUps OnPowerUp1;
		public static event PowerUps OnPowerUp2;

		UIStates current = UIStates.Default;
		UIStates previous = UIStates.Default;

		void Awake ()
		{
			Screen.sleepTimeout = 0;
		}

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

			OnStartGame += UpdateUI;

		}

		void OnDisable ()
		{
			AnswerTextManager.OnLevelComplete -= ToLevelCompletePanel;

			InputManager.OnEscapePressed -= EscapePressed;

			OnStartGame -= UpdateUI;

		}

		public void NoMenus ()
		{
			MainMenu.SetActive ( false );
			PlayScreen.SetActive ( false );
			NotificationPanel.SetActive ( false );
			LevelCompletePanel.SetActive ( false );
			ExitPanel.SetActive ( false );
			GameOverPanel.SetActive ( false );
			BigImagePanel.transform.parent.gameObject.SetActive ( false );

		}


		public void ToMainMenu ()
		{
			FadeManager.StartFadeInOutBetween ( GetCurrentActiveScreen (), new GameObject[] { MainMenu }, 1.0f );

//			MainMenu.SetActive ( true );
			PlayScreen.SetActive ( false );

			current = UIStates.MainMenu;

			UpdateUI ();
//
//			NotificationPanel.SetActive ( false );
//			ExitPanel.SetActive ( false );
//
//			LevelCompletePanel.SetActive ( false );
//			GameOverPanel.SetActive ( false );
		}

		public void ToPlayScreen ()
		{
			if ( SpawnerReference.CurrentLevel != null )
			{
				FadeManager.StartFadeInOutBetween ( GetCurrentActiveScreen (), new GameObject[] { PlayScreen, NotificationPanel }, 0.5f );

				current = UIStates.PlayScreen;

//			PlayScreen.SetActive ( true );
//			NotificationPanel.SetActive ( true );
//			
//			LevelCompletePanel.SetActive ( false );
//
//			MainMenu.SetActive ( false );
//			ExitPanel.SetActive ( false );
//
				if ( OnStartGame != null )
				{
					OnStartGame ();
				}

				PlaySound ( commonButton );
				PlaySound ( nextLevelButton );
			}
			else
			{
				ToGameOverPanel ();
			}
			UpdateUI ();
		}

		public void ToExitPanel ()
		{
			FadeManager.StartFadeInOutBetween ( GetCurrentActiveScreen (), new GameObject[] { ExitPanel }, 0.5f );

			current = UIStates.ExitPanel;

//			ExitPanel.SetActive ( true );
//			MainMenu.SetActive ( false );

			PlaySound ( commonButton );

		}

		public void ExitGame ()
		{
			Application.Quit ();
		}

		public void OnSmallImagePressed ( int selfIndex )
		{
//			BigImageReference.sprite = SpawnerReference.GetImageAtIndex ( selfIndex );
			BigImagePanel.ShowBigImage ( SpawnerReference.GetImageAtIndex ( selfIndex ), 0.5f );

			PlaySound ( smallImageButton );

		}

		public void OnBigImagePressed ()
		{
			BigImagePanel.HideBigImage ();

			PlaySound ( bigImageButton );

		}

		public void ToLevelCompletePanel ()
		{
			FadeManager.StartFadeInOutBetween ( new GameObject[] { }, new GameObject[] { LevelCompletePanel }, 0.5f );

			current = UIStates.LevelComplete;

//			LevelCompletePanel.SetActive ( true );
//
//			PlayScreen.SetActive ( true );
//			NotificationPanel.SetActive ( true );
//
//			MainMenu.SetActive ( false );
//			ExitPanel.SetActive ( false );

			PlaySound ( levelComplete );
			UpdateUI ();
		}

		public void ToGameOverPanel ()
		{
			#if NOT_WORKING
			if ( current == UIStates.PlayScreen || current == UIStates.LevelComplete )
			{
				FadeManager.StartFadeInOutBetween ( new GameObject[] { LevelCompletePanel, PlayScreen, NotificationPanel }, new GameObject[] { GameOverPanel }, 1.0f, false );
			#if TESTING && true
				Debug.Log ( "LPN to GO" );
			#endif
			}
			else
			if ( current == UIStates.MainMenu )
			{
				FadeManager.StartFadeInOutBetween ( new GameObject[] { MainMenu }, new GameObject[] { GameOverPanel } );
			}
			#endif

			current = UIStates.GameOver;

			GameOverPanel.SetActive ( true );
			
			PlayScreen.SetActive ( false );
			NotificationPanel.SetActive ( false );

			MainMenu.SetActive ( false );
			ExitPanel.SetActive ( false );

			LevelCompletePanel.SetActive ( false );

			PlaySound ( levelComplete );
//			Debug.Log ( "Game Over" );
		}

		public void ToNextLevel ()
		{
			FadeManager.StartFadeInOutBetween ( GetCurrentActiveScreen (), new GameObject[] { PlayScreen, NotificationPanel }, 1.5f, false );

			current = UIStates.PlayScreen;

//			PlayScreen.SetActive ( true );
//			NotificationPanel.SetActive ( true );
//			
//			LevelCompletePanel.SetActive ( false );
//
			MainMenu.SetActive ( false );
//			ExitPanel.SetActive ( false );

			Invoke ( "CallOnStartNextLevel", 1.4f );

			PlaySound ( nextLevelButton );
		}

		void CallOnStartNextLevel ()
		{

			if ( OnStartNextLevel != null )
			{
				OnStartNextLevel ();
			}

			UpdateUI ();

		}

		public void PowerUp1 ()
		{
			if ( OnPowerUp1 != null && MSO.DecreaseMoneyOnPowerUp_1 () )
			{
				OnPowerUp1 ( SpawnerReference.CurrentLevel );
			}

			AudioManRef.PlayPowerUpClip_1 ();
			UpdateUI ();
		}

		public void PowerUp2 ()
		{
			if ( OnPowerUp2 != null && MSO.DecreaseMoneyOnPowerUp_2 () )
			{
				OnPowerUp2 ( SpawnerReference.CurrentLevel );
			}

			AudioManRef.PlayPowerUpClip_2 ();
			UpdateUI ();
		}

		void UpdateUI ()
		{
			CurrentLevelTextInNotificationPanel.text = ( PSO.CurrentLevel + 0 /*1*/ ).ToString ();
			CurrentLevelTextInMainMenu.text = ( PSO.CurrentLevel + 0 /*1*/ ).ToString ();
			CoinsText.text = MSO.MoneyEarned.ToString ();
		}

		public void SoundToggle ( bool soundEnabled )
		{
			#if UNITY_EDITOR && TESTING && false
			Debug.Log ( string.Format ( "mute State:\t{0}\tToggleState:\t{1}" +
					"\nif ( !( PlayerPrefs.GetInt ( muteToggle ) == 0 ) & !SoundEnabled ) = {2}" +
					"\nif ( !( PlayerPrefs.GetInt ( muteToggle ) == 0 ) & !SoundEnabled ) = {3}",
				                           ( ( PlayerPrefs.GetInt ( muteToggle ) == 0 ) ? "False" : "True" ), SoundEnabled,
				                           ( ( PlayerPrefs.GetInt ( muteToggle ) == 0 ) & !SoundEnabled ),
				                           ( ( PlayerPrefs.GetInt ( muteToggle ) == 1 ) & SoundEnabled ) ) );
			#endif

			if ( soundEnabled )
			{
				SoundToggleRef.transform.GetChild ( 0 ).gameObject.SetActive ( true );
				SoundToggleRef.transform.GetChild ( 1 ).gameObject.SetActive ( false );
			}
			else
			{
				SoundToggleRef.transform.GetChild ( 0 ).gameObject.SetActive ( false );
				SoundToggleRef.transform.GetChild ( 1 ).gameObject.SetActive ( true );
			}


			if ( ( PlayerPrefs.GetInt ( muteToggle ) == 0 ) & !soundEnabled )
			{
				#if UNITY_EDITOR && TESTING
//				Debug.Log ( "Here 1" );
				#endif
				PlayerPrefs.SetInt ( muteToggle, 1 );
			}
			else
			if ( ( PlayerPrefs.GetInt ( muteToggle ) == 1 ) & soundEnabled )
			{
				PlayerPrefs.SetInt ( muteToggle, 0 );
			}
			PlayerPrefs.Save ();

			AudioManRef.ChangeMuteState ( !soundEnabled );

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

		private GameObject[] GetCurrentActiveScreen ()
		{
			switch ( current )
			{

				case UIStates.MainMenu:
					return new GameObject[] { MainMenu };

				case UIStates.PlayScreen:
					return new GameObject[] { PlayScreen, NotificationPanel };

				case UIStates.LevelComplete:
					return new GameObject[] { LevelCompletePanel, PlayScreen, NotificationPanel };

				case UIStates.GameOver:
					return new GameObject[] { GameOverPanel };

				case UIStates.ExitPanel:
					return new GameObject[] { ExitPanel };

				case UIStates.Default:
					#if UNITY_EDITOR && TESTING
					Debug.Log ( "Initialization Prob" );
					#endif
					return new GameObject[] { };

				default:
					#if UNITY_EDITOR && TESTING
					Debug.Log ( "Bigger Prob" );
					#endif
					return null;
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
					SpawnerReference.GetNextLevelBackend ();
					ToMainMenu ();
					break;

				case UIStates.GameOver:
					ToMainMenu ();
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