using UnityEngine;
using System.Collections;

using System.Collections.Generic;

namespace SO.Levels
{

    namespace SavingUtility
    {
        [System.Serializable]
        public class PowerUpUseSaver
        {
            public string Word;

            public List<bool> AnsweredLetters = new List<bool>();

            public List<bool> RemovedLetters = new List<bool>(12);

            public PowerUpUseSaver()
            {
            }

            public PowerUpUseSaver(string word, List<bool> answeredLetters, List<bool> removedLetters)
            {
                Word = word;
                AnsweredLetters = answeredLetters;
                RemovedLetters = removedLetters;
            }
        }
    }

    [System.Serializable]
    public class Level
    {

#if UNITY_EDITOR
        public bool show = false;

        public bool locked = false;
#endif

        [SerializeField]
        private bool completionStatus = false;

        public bool CompletionStatus
        {
            get { return completionStatus; }
        }

        public List<Sprite> Pics = new List<Sprite>(4);

        public string Word = "train";

        public string Word2 = "";

        public List<bool> AnsweredLetters = new List<bool>(5);

        public List<bool> RemovedLetters = new List<bool>(12);

        public List<char> OtherChars = new List<char>(12);

        public void SetCompletionStatus()
        {
            completionStatus = true;
        }

        public void RsetCompletionStatus()
        {
            completionStatus = false;
        }

        public void ClearWord()
        {
            Word = "";
            ClearSuffix();
            ClearAnswered();

        }

        public void ClearSuffix()
        {
            Word2 = "";
        }

        public void ClearOptions()
        {
            OtherChars.Clear();

            for (int i = 0; i < 12; i++)
            {
                OtherChars.Add(default(char));

            }

        }

        public void ClearRemoved()
        {
            RemovedLetters.Clear();

            for (int i = 0; i < 12; i++)
            {
                RemovedLetters.Add(default(bool));

            }

        }

        public void ClearAnswered()
        {
            AnsweredLetters.Clear();

            for (int i = 0; i < Word.Length; i++)
            {
                AnsweredLetters.Add(default(bool));
            }

        }

        public void ClearSelectedPictures()
        {
            Pics.Clear();
            for (int i = 0; i < 04; i++)
            {
                Pics.Add(default(Sprite));
            }

        }

        public SavingUtility.PowerUpUseSaver GetAnswerAndRemovedLetters()
        {
            return new SavingUtility.PowerUpUseSaver(Word, AnsweredLetters, RemovedLetters);
        }

        public void SetAnswerAndRemovedLetters(SavingUtility.PowerUpUseSaver ansAndRemLetters)
        {
            Word = ansAndRemLetters.Word;
            AnsweredLetters = ansAndRemLetters.AnsweredLetters;
            RemovedLetters = ansAndRemLetters.RemovedLetters;
        }

        public Level()
        {
#if UNITY_EDITOR
            show = false;
#endif
            completionStatus = false;
            this.ClearWord();
            this.ClearOptions();
            this.ClearRemoved();
            //this.ClearAnswered ();
            this.ClearSelectedPictures();
        }

    }

    [System.Serializable]
    public class LevelBatch
    {
#if UNITY_EDITOR
        public bool show = false;

        public bool locked = false;
#endif

        public List<Level> Levels = new List<Level>(10);

        public void AddLevel()
        {
            Levels.Add(new Level());
        }

        public LevelBatch()
        {
#if UNITY_EDITOR
            show = false;
#endif

            AddLevel();
        }

        public void DeleteLevel(int levelIndex)
        {
            Levels.RemoveAt(levelIndex);
        }

        public void ResetLevel(int levelIndex)
        {
            Levels[levelIndex] = new Level();
        }
    }


    public class LevelScriptableObject : ScriptableObject
    {
        public List<LevelBatch> LevelBatches;

        public static int MaxLevels = 10;

        public int MaxBatches = 10;

        public int MaxLevelsInBatches = 10;

        public int CurrentLevelCount()
        {
            int totalLevels = 0;
            foreach (LevelBatch LB in LevelBatches)
            {
                totalLevels += LB.Levels.Count;
            }
            return totalLevels;
        }

        public void AddBatch()
        {
            LevelBatches.Add(new LevelBatch());
        }

        public void DeleteBatch(int batchIndex)
        {
            LevelBatches.RemoveAt(batchIndex);
        }

        public void ResetBatch(int batchIndex)
        {
            LevelBatches[batchIndex] = new LevelBatch();
        }
    }
}
