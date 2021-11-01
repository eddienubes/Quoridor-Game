namespace Quorridor.Model
{
    public interface IPlayerController
    {
        public void SetModelsGameAndGrid(Game game, Grid grid);
        public void SubscribeToModel(Player playerModel);
        public bool IsActiveNow { get; set; }
    }
}