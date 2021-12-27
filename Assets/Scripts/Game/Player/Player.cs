using System.Collections;
using System.Collections.Generic;
using Uninstructed.Game.Main;
using Uninstructed.Game.Player.IO;
using UnityEngine;

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
