using System.Collections;
using System.Collections.Generic;
using Uninstructed.Game.Main;
using Uninstructed.Game.Player.IO;
using UnityEngine;

namespace Uninstructed.Game.Player
{
    public class Player : MonoBehaviour
    {
        private Entity entity;

        public PlayerProgram Program;

        public void Start()
        {
            entity = GetComponent<Entity>();
            Program = new PlayerProgram();
        }
    }
}
