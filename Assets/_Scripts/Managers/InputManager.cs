using UnityEngine;
using System.Collections;

namespace Managers
{

    public class InputManager : MonoBehaviour
    {

        public delegate void EscapePressed ();

        public static event EscapePressed OnEscapePressed;

        // Use this for initialization
        void Start ()
        {

        }

        // Update is called once per frame
        void Update ()
        {
            if ( Input.GetKeyDown ( KeyCode.Escape ) )
            {
                if ( OnEscapePressed != null )
                {
                    OnEscapePressed ();
                }
            }
        }

        public void OnClickInputLetter ( char letterPressed )
        {
            Debug.Log ( letterPressed );
        }

    }
}
