using Grid = Quorridor.Model.Grid;

namespace Quoridorgame.Controllers
{
    using Quorridor.Model;
    using UnityEngine;
    using View;
    using Grid = Grid;
    using Pawn = View.Pawn;

    public abstract class UnityPlayerControllerBase : MonoBehaviour, IPlayerController
    {
        [SerializeField] protected Pawn _pawn;

        protected WallDeck _wallDeck;

        protected Player _playerModel;
        protected Grid _grid;
        protected Game _gameModel;

        public Pawn Pawn => _pawn;
        public abstract void SetModelsGameAndGrid(Game game, Grid grid);
        public abstract void SubscribeToModel(Player playerModel);
        public abstract void SetPawnView(Pawn pawn);
        public abstract void SetWallDeck(WallDeck deck);

        public void Init(Game game, Grid grid, Player playerModel)
            => (_grid, _gameModel, _playerModel) = (grid, game, playerModel);

        public abstract bool IsActiveNow { get; set; }
    }
}