using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving.Interfaces;
using UnityEngine;

namespace Uninstructed.Game.Content.Blocks
{
    public class Launcher : MonoBehaviour, ISaveableAddition
    {
        [SerializeField]
        private Sprite activatedSprite;

        [SerializeField]
        private List<ItemMiniInfo> requiredItems;

        private new SpriteRenderer renderer;

        private bool activated = false;
        public bool Activated
        {
            get => activated;

            private set
            {
                if (!activated && value)
                {
                    activated = true;
                    renderer.sprite = activatedSprite;
                }
            }
        }

        public void Reset()
        {
            activatedSprite = null;
            requiredItems = new();
        }

        public void Start()
        {
            renderer = GetComponent<SpriteRenderer>();

            var block = GetComponent<Block>();
            block.Interacted += OnInteracted;
        }

        private void OnInteracted(Entity entity, Block block, string[] command)
        {
            if (!activated && command.Length > 0 && command[0] == "launch")
            {
                foreach (var (requiredType, requiredCount) in requiredItems)
                {
                    var count = entity.Inventory.TotalCount(requiredType);
                    if (count < requiredCount)
                    {
                        return;
                    }
                }
                foreach (var (requiredType, requiredCount) in requiredItems)
                {
                    entity.Inventory.Remove(requiredType, requiredCount);
                }
                activated = true;
            }
        }

        public void Load(Dictionary<string, object> data, GameObjectFactory factory)
        {
            activated = (bool)data["activated"];
        }

        public void Save(Dictionary<string, object> data)
        {
            data["activated"] = activated;
        }
    }
}
