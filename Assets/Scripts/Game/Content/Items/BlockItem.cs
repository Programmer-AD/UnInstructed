using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using UnityEngine;

namespace Uninstructed.Game.Content.Items
{
    public class BlockItem : MonoBehaviour
    {
        [SerializeField]
        private BlockType places;

        public void Reset()
        {
            places = BlockType.Unknown;
        }

        public void Awake()
        {
            var item = GetComponent<Item>();
            item.UsedSingle += OnUsedSingle;
        }

        private void OnUsedSingle(Entity entity, Item item)
        {
            var position = entity.LookDirectionInt;
            var x = position.x;
            var y = position.y;

            var map = entity.Director.World.Map;
            var block = map[x, y];
            if (block == null)
            {
                item.Count--;
                block = entity.Director.Factory.Create(places);
                block.transform.position = new Vector3(x, y);
                map[x, y] = block;
            }

        }
    }
}
