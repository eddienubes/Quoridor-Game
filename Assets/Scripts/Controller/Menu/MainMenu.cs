
using TMPro;
using UnityEngine;
using Quorridor.AI;

namespace Quoridorgame.View
{
    using Controllers;
    using Quorridor.Model;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private GameController gameController;


        private void Start()
        {
            gameController.OnPlayerWins += winnerId =>
            {
                _label.SetText($"{(winnerId == 0 ? "First" : "Second")} player wins!");
                gameObject.SetActive(true);
            };
            PlayAgainstPlayer();
            gameObject.SetActive(true);
        }

        public void PlayAgainstPlayer()
        {
            var players = new Player[2];
            players[0] = new HotSeatPlayer(gameController.MapSizeY - 1, true, 1);
            players[1] = new HotSeatPlayer(0, false, 2);

            gameController.SetPlayers(players);
            var p1 = gameController.gameObject.AddComponent<UnityPlayerController>();
            var p2 = gameController.gameObject.AddComponent<UnityPlayerController>();
            gameController.Init(p1, p2);
            gameObject.SetActive(false);
        }

        public void PlayAgainstAI()
        {
            var players = new Player[2];
            players[0] = new HotSeatPlayer(gameController.MapSizeY - 1, true, 1);
            players[1] = new DummyAiPlayer(0, false, 2);

            gameController.SetPlayers(players);
            var bot = gameController.gameObject.AddComponent<AiController>();
            var player = gameController.gameObject.AddComponent<UnityPlayerController>();
            gameController.Init(player, bot);
            gameObject.SetActive(false);
        }

        public void QuitGame() => Application.Quit();
    }
}