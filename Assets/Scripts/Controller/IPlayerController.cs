namespace Quoridorgame.Controllers
{
    using graph_sandbox;
    using UnityEngine;
    using View;
    using Grid = Grid;
    using Pawn = View.Pawn;

    public abstract class IPlayerController : MonoBehaviour
    {
        public abstract void SetModelsGameAndGrid(Game game, Grid grid);
        public abstract void SubscribeToModel(Player playerModel);
        public abstract void SetPawnView(Pawn pawn);
        public abstract void SetWallDeck(WallDeck deck);
        public abstract bool IsActiveNow { get; set; }
    }
}