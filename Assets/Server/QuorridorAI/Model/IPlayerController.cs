namespace Quorridor.Model
{
    public interface IPlayerController
    {
        public void Init(Game game, Grid grid, Player playerModel);
        public bool IsActiveNow { get; set; }
    }
}