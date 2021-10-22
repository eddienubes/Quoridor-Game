namespace Quoridorgame.Controllers
{
    using graph_sandbox;
    using View;
    using Pawn = View.Pawn;

    public interface IPlayerController
    {
        void TrySetVerticalWall(Cell wallPlaceHolder);
        void TrySetHorizontalWall(Cell wallPlaceHolder);
        void TryMovePawn(Cell clickedCell);

        void SetModelsGameAndGrid(Game game, Grid grid);
        public void SubscribeToModel(Player playerModel);
        public void SetPawnView(Pawn pawn);
        public void SetWallDeck(WallDeck deck);

        bool IsActiveNow { get; set; }
    }
}