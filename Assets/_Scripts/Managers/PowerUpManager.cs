#define TESTING

using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using System.Text;

using SO.Levels;
using SO.Money;
using Managers;

namespace PowerUps
{
    public class PowerUpManager : MonoBehaviour
    {
        [System.Serializable]
        class LetterStruct
        {
            public char Letter;

            public bool usable = true;

            public List <int> LetterAvailableAtOptionIndices;

            public List <int> LetterAtAnswerIndices;

                        //public List <int> AvailableOptionIndices;
            //
                        //public void CreateAvailableOptionIndices ()
                        //{
            //
                        //}

            public LetterStruct ( char lttr, bool option, int index )
            {
                Letter = lttr;

                LetterAvailableAtOptionIndices = new List<int> ();
                LetterAtAnswerIndices = new List<int> ();
                if ( option )
                {
                    LetterAtAnswerIndices.Add ( index );
                }
                else
                {
                    LetterAvailableAtOptionIndices.Add ( index );
                }

            }

            override public string ToString ()
            {
                StringBuilder LetterAvailableAtOptionIndicesString = new StringBuilder ( "Letter Available At Option Indices:\t [ " );
                StringBuilder LetterAtAnswerIndicesString = new StringBuilder ( "Letter At Answer Indices:\t [ " );

                foreach ( int item in LetterAtAnswerIndices )
                {
                    LetterAtAnswerIndicesString.Append ( item );
                    LetterAtAnswerIndicesString.Append ( ", " );
                }
                LetterAtAnswerIndicesString.Append ( " ]" );

                foreach ( int item in LetterAvailableAtOptionIndices )
                {
                    LetterAvailableAtOptionIndicesString.Append ( item );
                    LetterAvailableAtOptionIndicesString.Append ( ", " );
                }
                LetterAvailableAtOptionIndicesString.Append ( " ]" );

                return string.Format ( "char:\t{0}\t{1}\t{2}\tUsable:\t{3}", Letter, LetterAtAnswerIndicesString, LetterAvailableAtOptionIndicesString, usable );

            }

            public bool Evaluate ()
            {
                if ( LetterAtAnswerIndices.Count < LetterAvailableAtOptionIndices.Count )
                {
                    foreach ( char item in LetterAtAnswerIndices )
                    {
                        LetterAvailableAtOptionIndices.RemoveAt ( Random.Range ( 0, LetterAvailableAtOptionIndices.Count - 1 ) );
                    }

                    return usable = true;
                }
                else
                {
                    LetterAvailableAtOptionIndices.Clear ();
                }

            //    usable = false;
                return usable = false;
            }

        }

        [SerializeField]
        private LevelScriptableObject LSO;

        [SerializeField]
        private MoneyScriptableObject MSO;

        [SerializeField]
        private Spawner SpawnerReference;


        void OnEnable ()
        {
            UIManager.OnPowerUp1 += AddCorrectLetter;

            UIManager.OnPowerUp2 += RemoveIncorrectLetters;
        }

        void OnDisable ()
        {
            UIManager.OnPowerUp1 -= AddCorrectLetter;

            UIManager.OnPowerUp2 -= RemoveIncorrectLetters;
        }

        void AddCorrectLetter ( Level currentLevel )
        {
            int selectedCharIndexInAnswer = 0;
            int selectedCharIndexInOptions = 0;
            char selectedChar;
            List <int> IndicesOfSellectedOptionLetter = new List<int> ( 0 );
            List <int> IndicesOfAvailableAnswerLetters = new List<int> ( 0 );

            for ( int index = 0; index < currentLevel.AnsweredLetters.Count; index++ )
            {
                if ( !currentLevel.AnsweredLetters [ index ] )
                {
                    IndicesOfAvailableAnswerLetters.Add ( index );
                }
            }

            //do
            //{
                selectedCharIndexInAnswer = IndicesOfAvailableAnswerLetters [ Random.Range ( 0, ( IndicesOfAvailableAnswerLetters.Count - 1 ) ) ];
            //}
            //while ( !currentLevel.AnsweredLetters [ selectedCharIndexInAnswer ] );

            currentLevel.AnsweredLetters [ selectedCharIndexInAnswer ] = true;

            selectedChar = currentLevel.Word [ selectedCharIndexInAnswer ];

            for ( int index = 0; index < currentLevel.OtherChars.Count; index++ )
            {
                if ( currentLevel.OtherChars [ index ].Equals ( selectedChar ) && !currentLevel.RemovedLetters [ index ] )
                {
                    IndicesOfSellectedOptionLetter.Add ( index );
                }
            }

            //selectedCharIndexInOptions = 0;
            if ( IndicesOfSellectedOptionLetter.Count >= 1 )
            {
                selectedCharIndexInOptions = Random.Range ( 0, ( IndicesOfSellectedOptionLetter.Count - 1 ) );
            }

            currentLevel.RemovedLetters [ IndicesOfSellectedOptionLetter [ selectedCharIndexInOptions ] ] = true;

            #if UNITY_EDITOR && TESTING && false
            Debug.Log ( IndicesOfAvailableAnswerLetters.Count.ToString () + "\t" + selectedCharIndexInAnswer.ToString () + "\t" + selectedChar.ToString () + "\t"
                + IndicesOfSellectedOptionLetter.Count.ToString () + "\t" + selectedCharIndexInOptions.ToString () );
            #endif

            #if UNITY_EDITOR && TESTING && false
            Debug.Log ( ( IndicesOfSellectedOptionLetter.Count >= 1 ).ToString () );
            #endif

            SpawnerReference.UpdateLevel ();

        }

        void RemoveIncorrectLetters ( Level currentLevel )
        {
            //int selectedCharIndexInAnswer = 0;
            //int selectedCharIndexInOptions = 0;
            //char selectedChar;
            //List <int> IndicesOfSellectedOptionLetter = new List<int> ( 0 );
            //List <int> IndicesOfAvailableAnswerLetters = new List<int> ( 0 );

            Dictionary <char, LetterStruct> LetterTrack = new Dictionary<char, LetterStruct> ();

            Dictionary <int, char> Indexer = new Dictionary<int, char> ();

            int indexOfDict = 0, CountOfLettersThatCanBeRemoved = 0;

            //Dictionary <char, int> CountedLettersInOption = new Dictionary<char, int> ();
            //
            //Dictionary <char, int> SelectedLettersAvailableForRemoval = new Dictionary<char, int> ();

            int countOfLettersToSelectForm = 0;

            for ( int index = 0, WordLenght = currentLevel.Word.Length; index < WordLenght; index++ )
            {
                char letter = currentLevel.Word [ index ];
                if ( LetterTrack.ContainsKey ( letter ) )
                {
                    LetterTrack [ letter ].LetterAtAnswerIndices.Add ( index );
                }
                else
                {
                    LetterTrack.Add ( letter, new LetterStruct ( letter, true, index ) );
                    //Indexer.Add ( indexOfDict++, letter );
                }
                //Debug.Log ( LetterTrack [ letter ].ToString () );
            }

            for ( int index = 0, LevelOtherCharsCount = currentLevel.OtherChars.Count; index < LevelOtherCharsCount; index++ )
            {
                char letter = currentLevel.OtherChars [ index ];
                if ( !currentLevel.RemovedLetters [ index ] )//&& !CountedLettersInAnswer.ContainsKey (letter) )
                {
                    if ( LetterTrack.ContainsKey ( letter ) )
                    {
                        LetterTrack [ letter ].LetterAvailableAtOptionIndices.Add ( index );
                        //Debug.Log ("Ever Here");
                    }
                    else
                    {
                        LetterTrack.Add ( letter, new LetterStruct ( letter, false, index ) );
                        //Indexer.Add ( indexOfDict++, letter );
                    }
                }
            }

            #if UNITY_EDITOR && TESTING && false
            Debug.Log ( "Before:" );
            foreach ( KeyValuePair <char, LetterStruct> item in LetterTrack )
            {
                Debug.Log ( string.Format ( "Key:\t{0}\t{1}", item.Key, item.Value.ToString () ) );
            }
            #endif

            foreach ( KeyValuePair<char, LetterStruct> item in LetterTrack )
            {
                //KeyValuePair<char, LetterStruct> item = LetterTrack [ LetterTrack [ index ] ];
                if ( !item.Value.Evaluate () )
                {
                    //LetterTrack.Remove ( item.Key );
                    //Indexer.;
                }
                else
                {
                    CountOfLettersThatCanBeRemoved += item.Value.LetterAvailableAtOptionIndices.Count;
                    Indexer.Add ( indexOfDict++, item.Key );
                }
            }

            int lettersToRemove = Random.Range ( 1, ( Constant.Constants.maxLettersToRemove > CountOfLettersThatCanBeRemoved ? Constant.Constants.maxLettersToRemove : CountOfLettersThatCanBeRemoved ) );

            #if UNITY_EDITOR && TESTING && false
            Debug.Log ( "After:" );
            foreach ( KeyValuePair <char, LetterStruct> item in LetterTrack )
            {
                Debug.Log ( string.Format ( "Key:\t{0}\t{1}", item.Key, item.Value.ToString () ) );
            }
            #endif

            if ( Indexer.Count > 0 )
            {
                for ( int lettersRemoved = 0; lettersRemoved < lettersToRemove; lettersRemoved++ )
                {
                    int indexToDelete = Random.Range ( 0, Indexer.Count - 1 ), maxTrys = 4;
                    bool deleted = false;
                    while ( !deleted )
                    {
                        maxTrys--;
                        if ( LetterTrack [ Indexer [ indexToDelete ] ].usable )
                        {
                            deleted = true;
                            LetterTrack [ Indexer [ indexToDelete ] ].usable = false;
                            currentLevel.RemovedLetters [ LetterTrack [ Indexer [ indexToDelete ] ].LetterAvailableAtOptionIndices [ Random.Range ( 0, LetterTrack [ Indexer [ indexToDelete ] ].LetterAvailableAtOptionIndices.Count - 1 ) ] ] = true;

                            //Debug.Log ( "Deleted Letter At index:\t" + indexToDelete.ToString () );

                            break;
                        }
                        else
                        if ( maxTrys > 0 )
                        {
                            indexToDelete = Random.Range ( 0, Indexer.Count - 1 );
                        }
                        else
                        {
                            int ind = 0;
                            while ( ind < Indexer.Count )
                            {
                                if ( LetterTrack [ Indexer [ indexToDelete ] ].usable )
                                {
                                    deleted = true;
                                    LetterTrack [ Indexer [ indexToDelete ] ].usable = false;
                                    currentLevel.RemovedLetters [ LetterTrack [ Indexer [ indexToDelete ] ].LetterAvailableAtOptionIndices [ Random.Range ( 0, LetterTrack [ Indexer [ indexToDelete ] ].LetterAvailableAtOptionIndices.Count - 1 ) ] ] = true;

                                    //Debug.Log ( "Deleted Letter At index:\t" + indexToDelete.ToString () );

                                    break;
                                }
                                else
                                {
                                    ind++;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                MSO.RefundMoneyOnPowerUp_2 ();
            }

            //for ( int index = 0, OtherCharsCount = currentLevel.OtherChars.Count; index < OtherCharsCount; index++ )
            //{
            //    char letter = currentLevel.OtherChars [ index ];
            //    if ( !currentLevel.RemovedLetters [ index ] && !CountedLettersInAnswer.ContainsKey ( letter ) )
            //    {
            //        if ( CountedLettersInOption.ContainsKey ( letter ) )
            //        {
            //            CountedLettersInOption [ letter ] += 1;
            //     //            Debug.Log ( "Ever Here" );
            //        }
            //        else
            //        {
            //            CountedLettersInOption.Add ( letter, 1 );
            //        }
            //    }
            //}

            //currentLevel.OtherChars.ToString ().IndexOf ()

            #if UNITY_EDITOR && TESTING && false

            foreach ( KeyValuePair <char, int> item in CountedLettersInAnswer )
            {
            Debug.Log ( string.Format("{0}\tcount:\t{1}", item.Key, item.Value) );
            }

            #endif

            #if UNITY_EDITOR && TESTING && false

            //foreach ( KeyValuePair <char, int> item in CountedLettersInOption )
            //{
            //    countOfLettersToSelectForm += item.Value;
            //    if ( item.Value > 0 )
            //    {
            //        Debug.Log ( string.Format ( "{0}\tcount:\t{1}", item.Key, item.Value ) );
            //    }
            //    else
            //    {
            //        Debug.Log ( string.Format ( "NOT SELECTED:\t{0}\tcount:\t{1}", item.Key, item.Value ) );
            //    }
            //}

            Debug.Log ( countOfLettersToSelectForm );
            #endif


            #if false
            for ( int answerIndex = 0; answerIndex < currentLevel.Word.Length; answerIndex++ )
            {
                currentLevel.Word.Substring ( answerIndex + 1 ).Contains ( currentLevel.Word [ answerIndex ].ToString () );
            }
            #endif

            #if UNITY_EDITOR && TESTING && false
            Debug.Log ( IndicesOfAvailableAnswerLetters.Count.ToString () + "\t" + selectedCharIndexInAnswer.ToString () + "\t" + selectedChar.ToString () + "\t"
                + IndicesOfSellectedOptionLetter.Count.ToString () + "\t" + selectedCharIndexInOptions.ToString () );
            #endif

            #if UNITY_EDITOR && TESTING && false
            Debug.Log ( ( IndicesOfSellectedOptionLetter.Count >= 1 ).ToString () );
            #endif

            #if false
            SpawnerReference.UpdateLevel ();
            #endif

            SpawnerReference.UpdateLevel ();
        }

    }
}
