using Uninstructed.Game.Main;
using UnityEngine;

namespace Uninstructed.Game.Content.Items
{
    public class BlockBreaker : MonoBehaviour
    {
        [SerializeField, Min(1)]
        private float damage;

        public void Reset()
        {
            damage = 1;
        }

        public void Awake()
        {
            var item = GetComponent<Item>();
            item.UsedOnBlock += UsedOnBlock;
        }

        private void UsedOnBlock(Entity entity, Item item, Block block)
        {
            block.Durability -= damage;
        }
    }
}
