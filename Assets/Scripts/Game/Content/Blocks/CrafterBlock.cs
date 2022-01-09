using System;
using System.Collections.Generic;
using System.Linq;
using Uninstructed.Game.Main;
using UnityEngine;

namespace Uninstructed.Game.Content.Blocks
{
    public class CrafterBlock : MonoBehaviour
    {
        [SerializeField]
        private List<CraftRecipe> recipes;

        private Dictionary<string, CraftRecipe> indexedRecipes;

        public void Reset()
        {
            recipes = new();
        }

        public void Start()
        {
            indexedRecipes = recipes.ToDictionary(x => x.Name.ToLower(), x => x);

            var block = GetComponent<Block>();
            block.Interacted += OnInteract;
        }

        private void OnInteract(Entity entity, Block block, string[] args)
        {
            if (args.Length < 2 || args[0] != "craft")
            {
                return;
            }

            if (indexedRecipes.TryGetValue(args[1], out var recipe))
            {
                foreach (var (requiredType, requiredCount) in recipe.Required)
                {
                    var count = entity.Inventory.TotalCount(requiredType);
                    if (count < requiredCount)
                    {
                        return;
                    }
                }
                foreach (var (requiredType, requiredCount) in recipe.Required)
                {
                    entity.Inventory.Remove(requiredType, requiredCount);
                }
                var factory = entity.Director.Factory;
                var placePosition = entity.transform.position;
                foreach (var (resultType, resultCount) in recipe.Result)
                {
                    var item = factory.Create(resultType);
                    item.Count = resultCount;
                    item.transform.position = placePosition;
                }
            }
        }

        [Serializable]
        private struct CraftRecipe
        {
            public string Name;
            public List<ItemMiniInfo> Required;
            public List<ItemMiniInfo> Result;
        }
    }
}
