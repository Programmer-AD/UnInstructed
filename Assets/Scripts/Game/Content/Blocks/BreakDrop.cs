using System.Collections.Generic;
using Uninstructed.Game.Main;
using UnityEngine;

namespace Uninstructed.Game.Content.Blocks
{
    public class BreakDrop : MonoBehaviour
    {
        [SerializeField]
        private List<ItemMiniInfo> drops;

        public void Reset()
        {
            drops = new();
        }

        public void Start()
        {
            var block = GetComponent<Block>();
            block.Break += OnBlockBreak;
        }

        private void OnBlockBreak(Block block)
        {
            var position = block.transform.position;
            foreach (var (itemType, count) in drops)
            {
                var item = block.Director.Factory.Create(itemType);
                item.Count = count;
                item.transform.position = position;
            }
        }
    }
}
