using System.Collections.Generic;
using System.Text;
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

        public void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();

            var block = GetComponent<Block>();
            block.Interacted += OnInteracted;
        }

        private void OnInteracted(Entity entity, Block block, string[] command)
        {
            if (command.Length > 0)
            {
                switch (command[0])
                {
                    case "activate":
                        if (!activated)
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
                        break;
                    case "status":
                        entity.Director.PlayerController.OuterResult = activated ? "1" : "0";
                        break;
                }
            }
        }

        public void Load(Dictionary<string, object> data, GameObjectFactory factory)
        {
            Activated = (bool)data["activated"];
        }

        public void Save(Dictionary<string, object> data)
        {
            data["activated"] = activated;
        }
    }
}
