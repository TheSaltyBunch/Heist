using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;

namespace Game
{
    public class VictoryScene : MonoBehaviour
    {
        [SerializeField] private List<PlayerModel> playerModels;

        [SerializeField] private Transform[] _places;

        [SerializeField] private PlaceText[] _placeTexts;

        private void Start()
        {
            var order = GameManager.GameManagerRef.Scores.OrderByDescending(x => x.PlayerScore).ToList();
            //get places from game manager
            for (int i = 0; i < GameManager.NumPlayers; i++)
            {
                var playerModel = Instantiate(playerModels[(int) GameManager.PlayerChoice[order[i].PlayerNumber]],_places[i]);
                playerModel.SetColor(GameManager.PlayerColour[order[i].PlayerNumber]);
                _placeTexts[i].ScoreText.text = order[i].PlayerScore.ToString();
            }
        }
    }
}