namespace Quorridor.Model
{
    public interface IGameController
    {
        public void Init(IPlayerController[] playerControllers, Player[] players);
        public Game _gameModel { get; }
        public Grid _grid{ get; }
    }
}