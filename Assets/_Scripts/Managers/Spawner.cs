using UnityEngine ;
using System.Collections ;

namespace Managers
{
	
	public class Spawner : MonoBehaviour
	{

		[SerializeField]
		LevelScriptableObject LSO;

		[SerializeField]
		ProgressScriptableObject PSO;

		[SerializeField]
		InputManager IPManReference;

		public GameObject OptionLetterButtonPrefab;

		public GameObject ImagePrefab;

		public GameObject AnswerLetterButtonPrefab;

		public Transform ImagesHolder;

		public Transform AnswerButtonsHolder;

		public Transform OptionButtonsHolder;

		private Level CurrentLevel;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	}
}
