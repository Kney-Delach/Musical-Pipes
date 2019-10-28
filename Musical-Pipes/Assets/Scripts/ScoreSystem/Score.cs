using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace ScoreSystem{

    [Serializable]
    public class Score
    {
        public string playerID; 

        public int score;

        public float timeScore;

        public int scorePosition;
        
        // constructor
        public Score(string playerID, int score, float timeScore)
        {
            this.playerID = playerID;
            this.score = score;
            this.timeScore = timeScore;
            this.scorePosition = 1000;
        }

        [JsonConstructor]
        public Score(string playerID, int score, float timeScore, int scorePosition)
        {
            this.playerID = playerID;
            this.score = score;
            this.timeScore = timeScore;
            this.scorePosition = scorePosition;
        }
    }
}