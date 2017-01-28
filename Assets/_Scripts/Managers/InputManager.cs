using UnityEngine;
using System.Collections;

namespace Managers
{

	public class InputManager : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void OnClickInputLetter ( char letterPressed )
		{
			Debug.Log ( letterPressed );
		}

	}
}
