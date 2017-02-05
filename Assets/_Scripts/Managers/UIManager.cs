using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UnityEngine.Audio;

namespace Managers
{

	public class UIManager : MonoBehaviour
	{

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

		public Spawner SpawnerReference;

		public delegate void StartGame ();

		public event StartGame OnStartGame;

		UIStates current = UIStates.Default;
		UIStates previous = UIStates.Default;

		// Use this for initialization
		void Start ()
		{
			NoMenus ();
			ToMainMenu ();
			current = previous = UIStates.MainMenu;
		}

		void OnEnable ()
		{
			InputManager.OnEscapePressed += EscapePressed;
		}

		void OnDisable ()
		{
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

		}

		public void ToExitPanel ()
		{
			ExitPanel.SetActive ( true );
			current = UIStates.ExitPanel;

			MainMenu.SetActive ( false );
		}

		public void OnSmallImagePressed (int selfIndex)
		{
			BigImageReference.sprite = SpawnerReference.GetImageatindex (selfIndex);
			BigImagePanel.SetActive (true);
		}

		public void OnBigImagePressed ()
		{
			BigImagePanel.SetActive (false);
		}

		public void ToLevelCompletePanel ()
		{
			LevelCompletePanel.SetActive ( true );
			current = UIStates.LevelComplete;

			PlayScreen.SetActive ( true );
			NotificationPanel.SetActive ( true );

			MainMenu.SetActive ( false );
			ExitPanel.SetActive ( false );
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
					Debug.Log ( "initialization Prob" );
					break;
				
				default:
					Debug.Log ( "Bigger Prob" );
					break;
			}
		}
	}
}