using Uninstructed.Game.Main;
using Uninstructed.Game.Player.IO;

namespace Uninstructed.Game.Player
{
    public class Player : GameObjectAdditionBase<Entity>
    {
        public PlayerProgram Program;

        public void Start()
        {
            Program = new PlayerProgram();
        }
    }
}
