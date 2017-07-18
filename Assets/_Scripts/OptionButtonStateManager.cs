using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using Managers;

namespace States.Options
{

    public enum OptionButtonStates
    {
        Clicked,
        NotClicked,
        Disabled,
        Default,
    }

    public class OptionButtonStateManager : MonoBehaviour
    {

        [SerializeField]
        private InputManager IPManReference;

        [SerializeField]
        private AnswerTextManager ATManReference;

        [SerializeField]
        private OptionButtonStates currentButtonState = OptionButtonStates.Default;

        //The letter it will be holding.
        [SerializeField]
        private char letter;

        public char Letter
        {
            get
            {
                return letter;
            }
        }

        [SerializeField]
        private Button buttonComponent;

        [SerializeField]
        private Text textComponent;

        [SerializeField]
        private GameObject childButton;

        public delegate void OnClickedDelegate();

        public OnClickedDelegate OnClickEvent;

        // Use this for initialization before Start
        void Awake()
        {
            buttonComponent = GetComponent<Button>();
            textComponent = GetComponentInChildren<Text>();

            buttonComponent.onClick.AddListener
            (
                delegate
                {
                    this.OnClick();
                }
            );
        }

        // Use this for initialization
        void Start()
        {
        }

        public void AssignLetter(char newLetter)
        {
            currentButtonState = OptionButtonStates.NotClicked;

            letter = newLetter;
            if (textComponent == null)
            {
                textComponent = GetComponentInChildren<Text>();
            }
            textComponent.text = newLetter.ToString();
        }

        public void AssignReferences(InputManager newIPManRef, AnswerTextManager newATManRef)
        {
            IPManReference = newIPManRef;
            ATManReference = newATManRef;

            //textComponent = GetComponentInChildren <Text> ();
            childButton = transform.GetChild(0).gameObject;
        }

        public void ResetClickedStatus()
        {
            if (currentButtonState == OptionButtonStates.Clicked)
            {
                currentButtonState = OptionButtonStates.NotClicked;

                childButton.SetActive(true);
                //textComponent.text = letter.ToString ();
            }
        }

        public void SetStateDisabled()
        {
            childButton.SetActive(false);
            //letter = '\0';
            currentButtonState = OptionButtonStates.Disabled;

            //textComponent.text = letter.ToString ();
        }

        void OnClick()
        {
            if (OnClickEvent != null)
            {
                OnClickEvent();
            }

            switch (currentButtonState)
            {

                case OptionButtonStates.Clicked:
                    break;

                case OptionButtonStates.NotClicked:
                    if (ATManReference.AddLetterToTypedWord(letter, this))
                    {
                        //IPManReference.AcceptOption ( letter );
                        currentButtonState = OptionButtonStates.Clicked;
                        //textComponent.text = "";
                        childButton.SetActive(false);
                    }
                    break;

                //Used for Possible Power-Ups
                case OptionButtonStates.Disabled:
                    break;

                case OptionButtonStates.Default:
                    break;

                default:
                    Debug.Log("We Have a Problem");
                    break;
            }
        }

    }

}
