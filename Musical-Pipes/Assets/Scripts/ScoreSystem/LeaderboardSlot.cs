using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LevelManagement;
using ScoreSystem;

namespace ScoreSystem {
    public class LeaderboardSlot : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField]
        private Text rankText;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text scoreText;

        [SerializeField]
        private Text scoreTimeText;

        private Score score;

        public void AssignTexts(Score score)
        {
            this.score = score;

            rankText.text = this.score.scorePosition.ToString();
            nameText.text = this.score.playerID;
            scoreText.text = this.score.score.ToString();
            scoreTimeText.text = this.score.timeScore.ToString("F2");
        }

    }

}