namespace Quorridor.Model
{
    public interface IGameController
    {
        public void Init(params IPlayerController[] playerControllers);
        public void SetPlayers(params Player[] players);
        
        public Game _gameModel { get; }
        public Grid _grid{ get; }
    }
}