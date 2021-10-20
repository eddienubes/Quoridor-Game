using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quoridorgame.View
{
    using graph_sandbox;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        public void PlayAgainstPlayer()
        {
            var players = new Player[2];
            players[0] = new HotSeatPlayer(gameController.MapSizeY - 1, true, 1);
            players[1] = new HotSeatPlayer(0, false, 2);

            gameController.SetPlayers(players);
            gameController.Init();
            Destroy(gameObject);
        }

        public void PlayAgainstAI()
        {
            var players = new Player[2];
            players[0] = new HotSeatPlayer(gameController.MapSizeY - 1, true, 1);
            players[1] = new HotSeatPlayer(0, false, 2);

            gameController.SetPlayers(players);
            gameController.Init();
            Destroy(this);
        }

        public void QuitGame() => Application.Quit();
    }
}