using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScoreSystem;
using GameControllers;
using LevelManagement.Data;

namespace LevelManagement
{
    // shown when player completes the level
    public class LeaderboardMenu : Menu<LeaderboardMenu>
    {
        // reference to initially selected button 
        [SerializeField]
        private Button _selectedComponent;

        // reference to active status of leaderboard
        private bool _active = false;

        [Header("Leaderboard UI Components")]
        [SerializeField]
        private Text _songNameText;

        [SerializeField]
        private Dropdown _songNameDropdown;

        [SerializeField]
        private LeaderboardSlot[] _activeSlots; 

        [SerializeField]
        private LeaderboardSlot _activePlayerSlot; 

        private List<string> _dropdowns; 
        private string activePlayerName = "Jeff Kaplan";
        public string ActivePlayerName { set { activePlayerName = value ; } }        
        private string _activeSongName = "Escape - Jaroslav Beck";
        
        private void Update()
        {
            if (!_active)
            {
                _active = true;
                _selectedComponent.Select();
                _selectedComponent.OnSelect(null);
            }
        }

        public void SetDropdown()
        {
            _dropdowns = new List<string>();
            DataManager data = FindObjectOfType<DataManager>();
            Debug.Log("Highscores Keys: " + data.Highscores.Keys.Count);
            foreach(string val in data.Highscores.Keys)
                _dropdowns.Add(val);
            
            if(_dropdowns.Capacity != 0)
            {
                _songNameDropdown.ClearOptions();
                _songNameDropdown.AddOptions(_dropdowns);
                _songNameDropdown.value = 0;
                _songNameDropdown.RefreshShownValue();
            }
            else 
            {
                    Debug.LogError("LeaderboardMenu Error: No Leaderboards Available");
            }

        }

        public void SetupCurrentLeaderboard()
        {
            foreach(LeaderboardSlot slot in _activeSlots)
            {
                slot.GetComponent<CanvasGroup>().alpha = 0;
            }

            _activePlayerSlot.GetComponent<CanvasGroup>().alpha = 0;

            _activeSongName = _songNameDropdown.options[_songNameDropdown.value].text;

            Debug.Log("Setting up Leaderboard: " + _activeSongName);

            _songNameText.text = _activeSongName;

            DataManager data = FindObjectOfType<DataManager>();

            if(data.Highscores.ContainsKey(_activeSongName))
            {
                List<Score> scores = data.Highscores[_activeSongName];
                bool playerFound = false;
                for(int i = 1; i < 6; i++)
                {
                    foreach(Score s in scores)
                    {
                        if(s.scorePosition == i)
                        {
                            _activeSlots[i-1].GetComponent<CanvasGroup>().alpha = 1;
                            _activeSlots[i-1].AssignTexts(s);
                            
                            if(!playerFound && (s.playerID == activePlayerName))
                            {
                                _activePlayerSlot.GetComponent<CanvasGroup>().alpha = 1;
                                _activePlayerSlot.AssignTexts(s);
                                playerFound = true;
                            }
                            
                            break;
                        }
                    }
                }
            }
            else 
            {
                Debug.LogError("LeaderboardMenu Error: Dropdown Value Invalid");
            }
        }



        // custom back press function, sets menu to inactive
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            
            foreach(LeaderboardSlot slot in _activeSlots)
            {
                slot.GetComponent<CanvasGroup>().alpha = 0;
            }

            _activePlayerSlot.GetComponent<CanvasGroup>().alpha = 0;

            _active = false; 
        }

    }
}
