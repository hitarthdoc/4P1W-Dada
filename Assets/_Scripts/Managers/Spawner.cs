#define ENABLE_PROFILER
#define TESTING

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UnityEngine.Profiling;

using System;

using System.IO;

//using UnityEngine.Events;

using SO.Levels;
using SO.Levels.SavingUtility;
using SO.Progress;

using States.Options;
using States.Answers;

using Special.Saver;

using Constant;

namespace Managers
{

    public class Spawner : MonoBehaviour
    {
        //const string Constants.LevelFileName = "/SavedAnswers.SAVE";

        [SerializeField]
        LevelScriptableObject LSO;

        [SerializeField]
        ProgressScriptableObject PSO;

        [SerializeField]
        InputManager IPManReference;

        [SerializeField]
        AnswerTextManager ATManReference;

        [SerializeField]
        UIManager UIManReference;

        [SerializeField]
        AudioManager AudioManReference;

        [SerializeField]
        GridLayoutGroup Layout_7AndLess;

        [SerializeField]
        GridLayoutGroup Layout_8To10;

        [SerializeField]
        GridLayoutGroup Layout_11And12;

        public GameObject OptionLetterButtonPrefab;

        public GameObject ImagePrefab;

        public GameObject AnswerLetterButtonPrefab;

        public GameObject AnswerSuffixPrefab;

        public Transform ImagesHolder;

        public Transform AnswerButtonsHolder;

        public Transform AnswerSuffixHolder;

        public Transform OptionButtonsHolder;

        public Transform OptionButtonsHolderAtIndex ( int index )
        {
            if ( index < OptionButtonsHolder.childCount )
            {
                return OptionButtonsHolder.GetChild ( index );
            }
            else
            {
                new ArgumentOutOfRangeException ( string.Format ( "The index {0} provided is greater than the childCount of OptionButtonHolder {1}", index, OptionButtonsHolder.childCount ), new Exception () );
                return null;
            }
        }

        public GameObject PowerUpHolder;

        public Transform AnswerLettersOnCompletionPanelHolder;

        public Transform AnswerSuffixLettersOnCompletionPanelHolder;

        [SerializeField]
        private Level currentLevel;

        public Level CurrentLevel {
            get
            {
                return currentLevel;
            }
        }

        // Use this for initialization
        void Awake ()
        {
            currentLevel = PSO.GetCurrentLevelToSpawn ();
            if ( currentLevel != null )
            {
                try
                {
                    PowerUpUseSaver demo = MyXMLSerializer.Deserialize <PowerUpUseSaver> ( Application.persistentDataPath + Constants.LevelFileName );

                    if ( currentLevel.Word.Equals ( demo.Word ) )
                    {
                        currentLevel.SetAnswerAndRemovedLetters ( demo );
                    }
                    else
                    {
                        throw new FileNotFoundException ();
                    }
                }
                catch
                {
                    MyXMLSerializer.Serialize <PowerUpUseSaver> ( Application.persistentDataPath + Constants.LevelFileName, currentLevel.GetAnswerAndRemovedLetters () );
                    Debug.Log ( "FirstRun" );
                }
            }
            #if Commented
            else
            {
                UIManReference.ToGameOverPanel ();
            }
            #endif
        }


        // Use this for initialization
        void Start ()
        {

        }

        void OnEnable ()
        {
            AnswerTextManager.OnLevelComplete += ClearAndAddAnswerOnCompletionPanel;

            AnswerTextManager.OnLevelComplete += ClearAndAddAnswerSuffixOnCompletionPanel;

            AnswerTextManager.OnLevelComplete += HideOptionLettersAndPowerUps;

            UIManReference.OnStartGame += SpawnCurrentLevel;

            UIManReference.OnStartNextLevel += GetAndSpawnNextLevel;

            UIManReference.OnStartGame += ShowOptionLettersAndPowerUps;

            UIManReference.OnStartNextLevel += ShowOptionLettersAndPowerUps;
        }

        void OnDisable ()
        {
            AnswerTextManager.OnLevelComplete -= ClearAndAddAnswerOnCompletionPanel;

            AnswerTextManager.OnLevelComplete -= ClearAndAddAnswerSuffixOnCompletionPanel;

            AnswerTextManager.OnLevelComplete -= HideOptionLettersAndPowerUps;

            UIManReference.OnStartGame -= SpawnCurrentLevel;

            UIManReference.OnStartNextLevel -= GetAndSpawnNextLevel;

            UIManReference.OnStartGame -= ShowOptionLettersAndPowerUps;

            UIManReference.OnStartNextLevel -= ShowOptionLettersAndPowerUps;
        }

        private void SpawnCurrentLevel ()
        {
            ClearCurrentLevel ();

            /*    DONE:
             *         SEND currentLevel. Word to someOne for tracking.
            */

            if ( currentLevel != null )
            {
                ATManReference.AnswerWord = currentLevel.Word;

                int imageIndex = 0;

                foreach ( Sprite image in currentLevel.Pics )
                {
                    GameObject newImageReference = Instantiate ( ImagePrefab, ImagesHolder ) as GameObject;
                    newImageReference.GetComponent <Image> ().sprite = image;

                    newImageReference.GetComponent <RectTransform> ().localScale = Vector3.one;

                    AttachListener ( newImageReference.GetComponent <Button> (), 1, Convert.ToChar ( imageIndex++ ) );
                }

                for ( int index = 0, CurrentLevelOtherCharsCount = currentLevel.OtherChars.Count; index < CurrentLevelOtherCharsCount; index++ )
                {
                    char optionLetter = currentLevel.OtherChars [ index ];
                    GameObject newOptionButtonReference = Instantiate ( OptionLetterButtonPrefab, OptionButtonsHolder ) as GameObject;
                    newOptionButtonReference.GetComponent<RectTransform> ().localScale = Vector3.one;
                    OptionButtonStateManager tempRef = newOptionButtonReference.GetComponent<OptionButtonStateManager> ();
                    tempRef.AssignReferences ( IPManReference, ATManReference );

                    if ( !currentLevel.RemovedLetters [ index ] )
                    {
                        tempRef.AssignLetter ( optionLetter );
                    }
                    else
                    {
                        tempRef.SetStateDisabled ();
                    }
                    //Removed both as Now it is done by OBSM.
                    //newOptionButtonReference.GetComponentInChildren <Text> ().text = optionLetter.ToString ();
                    //AttachListener ( newOptionButtonReference.GetComponent <Button> (), 1, optionLetter );
                }

                Profiler.BeginSample ( "Spawnig Answers", this );
                {
                    for ( int index = 0, CurrentLevelWordCount = currentLevel.Word.Length; index < CurrentLevelWordCount; index++ )
                    {
                        char answerLetter = currentLevel.Word [ index ];
                        GameObject newAnswerButtonReference = Instantiate ( AnswerLetterButtonPrefab, AnswerButtonsHolder ) as GameObject;
                        newAnswerButtonReference.GetComponent<RectTransform> ().localScale = Vector3.one;
                        AnswerButtonStateManager tempRef = newAnswerButtonReference.GetComponent<AnswerButtonStateManager> ();
                        tempRef.AssignReferences ( IPManReference, ATManReference );

                        if ( currentLevel.AnsweredLetters [ index ] )
                        {
                            tempRef.SetStateAnswered ( answerLetter );
                        }

                        //Removed both as Now it is done by OBSM.
                        //newOptionButtonReference.GetComponentInChildren <Text> ().text = optionLetter.ToString ();
                        //AttachListener ( newOptionButtonReference.GetComponent <Button> (), 1, optionLetter );
                    }
                    //Debug.Break ();
                }
                Profiler.EndSample ();

                #region ChangeGridLayoutGroupProperties for AnswerButtonsHolder
                GridLayoutGroup tempRefForGLG = AnswerButtonsHolder.GetComponent <GridLayoutGroup> ();

                if ( currentLevel.Word.Length <= 7 )
                {
                    ChangeGridLayoutGroupProperties ( Layout_7AndLess, ref tempRefForGLG );
                    //tempRefForAnswerButtonsHolder = Layout_7AndLess.GetComponent <GridLayoutGroup> ();
                }
                else
                if ( currentLevel.Word.Length <= 10 )
                {
                    ChangeGridLayoutGroupProperties ( Layout_8To10, ref tempRefForGLG );
                    //tempRefForAnswerButtonsHolder = Layout_8To10.GetComponent <GridLayoutGroup> ();

                }
                else
                if ( currentLevel.Word.Length <= 12 )
                {
                    ChangeGridLayoutGroupProperties ( Layout_11And12, ref tempRefForGLG );
                    //tempRefForAnswerButtonsHolder = Layout_11And12.GetComponent <GridLayoutGroup> ();

                }
                else
                {
                    ChangeGridLayoutGroupProperties ( Layout_11And12, ref tempRefForGLG );
                    //tempRefForAnswerButtonsHolder = Layout_11And12.GetComponent <GridLayoutGroup> ();
                    Debug.Log ( "WE have a VERYYYY BIIIIGGGG WORD." );
                }
                #endregion

                if ( currentLevel.Word2.Length > 0 )
                {
                    foreach ( char suffixLetter in currentLevel.Word2 )
                    {
                        GameObject newAnswerSuffixReference = Instantiate ( AnswerSuffixPrefab, AnswerSuffixHolder ) as GameObject;

                        newAnswerSuffixReference.GetComponent <RectTransform> ().localScale = Vector3.one;

                        newAnswerSuffixReference.GetComponentInChildren <Text> ().text = suffixLetter.ToString ();

                        //Removed both as Now it is done by OBSM.
                        //newOptionButtonReference.GetComponentInChildren <Text> ().text = optionLetter.ToString ();
                        //AttachListener ( newOptionButtonReference.GetComponent <Button> (), 1, optionLetter );

                    }
                }

                #region ChangeGridLayoutGroupProperties for AnswerSuffixHolder
                tempRefForGLG = AnswerSuffixHolder.GetComponent <GridLayoutGroup> ();

                if ( currentLevel.Word.Length <= 7 )
                {
                    ChangeGridLayoutGroupProperties ( Layout_7AndLess, ref tempRefForGLG );
                    //tempRefForAnswerButtonsHolder = Layout_7AndLess.GetComponent <GridLayoutGroup> ();
                }
                else
                if ( currentLevel.Word.Length <= 10 )
                {
                    ChangeGridLayoutGroupProperties ( Layout_8To10, ref tempRefForGLG );
                    //tempRefForAnswerButtonsHolder = Layout_8To10.GetComponent <GridLayoutGroup> ();

                }
                else
                if ( currentLevel.Word.Length <= 12 )
                {
                    ChangeGridLayoutGroupProperties ( Layout_11And12, ref tempRefForGLG );
                    //tempRefForAnswerButtonsHolder = Layout_11And12.GetComponent <GridLayoutGroup> ();

                }
                else
                {
                    ChangeGridLayoutGroupProperties ( Layout_11And12, ref tempRefForGLG );
                    //tempRefForAnswerButtonsHolder = Layout_11And12.GetComponent <GridLayoutGroup> ();
                    Debug.Log ( "WE have a VERYYYY BIIIIGGGG WORD." );
                }
                #endregion

                StartCoroutine ( "ATMAddNewAnswerButtonsCaller" );
            }
            else
            {
                #if TESTING && true
                Debug.Log ( "Ever Here" );
                #endif
                UIManReference.ToGameOverPanel ();
            }

        }

        IEnumerator ATMAddNewAnswerButtonsCaller ()
        {
            yield return new WaitForEndOfFrame ();

            ATManReference.AddNewAnswerButtonsReference ( AnswerButtonsHolder );
            ATManReference.UpdateAnsweredLetters ( currentLevel );
        }

        public void GetNextLevelBackend ()
        {
            currentLevel = PSO.GetNextLevelToSpawn ();

            MyXMLSerializer.Serialize <PowerUpUseSaver> ( Application.persistentDataPath + Constants.LevelFileName, currentLevel.GetAnswerAndRemovedLetters () );
        }

        private void GetAndSpawnNextLevel ()
        {
            currentLevel = PSO.GetNextLevelToSpawn ();

            #if TESTING
            Debug.Log ( Application.persistentDataPath );
            #endif

            if ( currentLevel != null )
            {
                MyXMLSerializer.Serialize <PowerUpUseSaver> ( Application.persistentDataPath + Constants.LevelFileName, currentLevel.GetAnswerAndRemovedLetters () );
            }

            SpawnCurrentLevel ();
        }


        /*    Reused for:
         * images Listener
        */
        private void AttachListener ( Button attachToThis, int typeOfCall, char argutmentToPass )
        {
            switch ( typeOfCall )
            {
                case 1:

                    attachToThis.onClick.AddListener (
                        delegate
                        {
                            UIManReference.OnSmallImagePressed ( Convert.ToInt32 ( argutmentToPass ) );
                            AudioManReference.PlayHintImageOutUIButtonSound ();
                        }
                    );
                    //attachToThis.onClick.AddListener (
                    //                              delegate
                    //{
                    //    AudioManReference.PlayHintImageOutUIButtonSound ();
                    //}
                    //                             );
                    break;

                case 2:

                    attachToThis.onClick.AddListener (
                        delegate
                        {
                            IPManReference.OnClickInputLetter ( argutmentToPass );
                        }
                    );
                    break;


                default:
                    break;
            }

        }

        private void ClearCurrentLevel ()
        {
            ImagesHolder.GetComponent <GridLayoutGroup> ().enabled = false;

            AnswerButtonsHolder.GetComponent <GridLayoutGroup> ().enabled = false;

            AnswerSuffixHolder.GetComponent <GridLayoutGroup> ().enabled = false;

            OptionButtonsHolder.GetComponent <GridLayoutGroup> ().enabled = false;

            for ( int i = 0; i < ImagesHolder.childCount; i++ )
            {
                Destroy ( ImagesHolder.GetChild ( i ).gameObject );
            }

            for ( int i = 0; i < AnswerButtonsHolder.childCount; i++ )
            {
                Destroy ( AnswerButtonsHolder.GetChild ( i ).gameObject );
            }

            for ( int i = 0; i < AnswerSuffixHolder.childCount; i++ )
            {
                Destroy ( AnswerSuffixHolder.GetChild ( i ).gameObject );
            }

            for ( int i = 0; i < OptionButtonsHolder.childCount; i++ )
            {
                OptionButtonsHolder.GetChild ( i ).GetComponent <Button> ().onClick.RemoveAllListeners ();
                Destroy ( OptionButtonsHolder.GetChild ( i ).gameObject );
            }

            ImagesHolder.GetComponent <GridLayoutGroup> ().enabled = true;

            AnswerButtonsHolder.GetComponent <GridLayoutGroup> ().enabled = true;

            AnswerSuffixHolder.GetComponent <GridLayoutGroup> ().enabled = true;

            OptionButtonsHolder.GetComponent <GridLayoutGroup> ().enabled = true;

            /*
             * Donot destroy until a solution is found.
             *
            */
            //Destroy ( AnswerButtonsHolder.GetComponent <GridLayoutGroup> () );

        }

        private void ClearAndAddAnswerOnCompletionPanel ()
        {
            int childrenCount = AnswerLettersOnCompletionPanelHolder.childCount;

            for ( int index = 0; index < childrenCount; index++ )
            {
                DestroyImmediate ( AnswerLettersOnCompletionPanelHolder.GetChild ( 0 ).gameObject );
            }

            childrenCount = AnswerButtonsHolder.childCount;

            for ( int index = 0; index < childrenCount; index++ )
            {
                AnswerButtonsHolder.GetChild ( 0 ).GetComponentInChildren <AnswerButtonStateManager> ().AddingToCompletePanel ();
                AnswerButtonsHolder.GetChild ( 0 ).SetParent ( AnswerLettersOnCompletionPanelHolder );
            }
            GridLayoutGroup tempRef = AnswerLettersOnCompletionPanelHolder.GetComponent <GridLayoutGroup> ();
            ChangeGridLayoutGroupProperties ( AnswerButtonsHolder.GetComponent <GridLayoutGroup> (), ref tempRef );

        }

        private void HideOptionLettersAndPowerUps ()
        {
            OptionButtonsHolder.gameObject.SetActive ( false );
            PowerUpHolder.gameObject.SetActive ( false );
        }


        private void ShowOptionLettersAndPowerUps ()
        {
            OptionButtonsHolder.gameObject.SetActive ( true );
            PowerUpHolder.gameObject.SetActive ( true );
        }

        private void ClearAndAddAnswerSuffixOnCompletionPanel ()
        {
            int childrenCount = AnswerSuffixLettersOnCompletionPanelHolder.childCount;

            for ( int index = 0; index < childrenCount; index++ )
            {
                DestroyImmediate ( AnswerSuffixLettersOnCompletionPanelHolder.GetChild ( 0 ).gameObject );
            }

            childrenCount = AnswerSuffixHolder.childCount;

            for ( int index = 0; index < childrenCount; index++ )
            {
                AnswerSuffixHolder.GetChild ( 0 ).SetParent ( AnswerSuffixLettersOnCompletionPanelHolder );
            }
            GridLayoutGroup tempRef = AnswerSuffixLettersOnCompletionPanelHolder.GetComponent <GridLayoutGroup> ();
            ChangeGridLayoutGroupProperties ( AnswerSuffixHolder.GetComponent <GridLayoutGroup> (), ref tempRef );

        }

        public void UpdateLevel ()
        {
            MyXMLSerializer.Serialize <PowerUpUseSaver> ( Application.persistentDataPath + Constants.LevelFileName, currentLevel.GetAnswerAndRemovedLetters () );

            for ( int index = 0; index < currentLevel.AnsweredLetters.Count; index++ )
            {
                if ( currentLevel.AnsweredLetters [ index ] )
                {
                    AnswerButtonsHolder.GetChild ( index ).GetComponent <AnswerButtonStateManager> ().SetStateAnswered ( currentLevel.Word [ index ] );
                }
            }

            for ( int index = 0; index < currentLevel.RemovedLetters.Count; index++ )
            {
                if ( currentLevel.RemovedLetters [ index ] )
                {
                    OptionButtonsHolder.GetChild ( index ).GetComponent <OptionButtonStateManager> ().SetStateDisabled ();
                }
            }

            ATManReference.UpdateAnsweredLetters ( currentLevel );

        }

        public Sprite GetImageAtIndex ( int index )
        {
            return currentLevel.Pics [ index ];
        }

        void ChangeGridLayoutGroupProperties ( GridLayoutGroup fromGLG, ref GridLayoutGroup toGLG )
        {
            //toGLG = fromGLG;
            //Debug.Log ("I did reach here.\t" + fromGLG.cellSize.ToString ());

            toGLG.cellSize = fromGLG.cellSize;
            toGLG.spacing = fromGLG.spacing;
            toGLG.padding = fromGLG.padding;

            //Debug.Log ("I did reach here.\t" + toGLG.cellSize.ToString ());
        }


    }
}
