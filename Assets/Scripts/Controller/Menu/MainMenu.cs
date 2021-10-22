using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Quoridorgame.View
{
    using graph_sandbox;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        
        [SerializeField]
        private GameController gameController;


        private void Start()
        {
            gameController.OnPlayerWins += winnerId =>
            {
                _label.SetText($"{(winnerId == 0 ? "First" : "Second")} player wins!");
                gameObject.SetActive(true);
            };
        }
        public void PlayAgainstPlayer()
        {
            var players = new Player[2];
            players[0] = new HotSeatPlayer(gameController.MapSizeY - 1, true, 1);
            players[1] = new HotSeatPlayer(0, false, 2);

            gameController.SetPlayers(players);
            gameController.Init();
            gameObject.SetActive(false);
        }

        public void PlayAgainstAI()
        {
            var players = new Player[2];
            players[0] = new HotSeatPlayer(gameController.MapSizeY - 1, true, 1);
            players[1] = new HotSeatPlayer(0, false, 2);

            gameController.SetPlayers(players);
            gameController.Init();
            gameObject.SetActive(false);
        }

        public void QuitGame() => Application.Quit();
    }
}