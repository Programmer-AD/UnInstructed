using System.Text;
using Uninstructed.Game.Main;
using UnityEngine;

namespace Uninstructed.Game.Player.Commands
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendPosition(this StringBuilder builder, Vector2 position)
        {
            return builder.Append(position.x).Append(' ').Append(position.y);
        }
        public static StringBuilder AppendShortInfo(this StringBuilder builder, Block block)
        {
            if (builder.AppendOnNull(block))
            {
                return builder;
            }
            return builder.Append((int)block.Type);
        }
        public static StringBuilder AppendShortInfo(this StringBuilder builder, Item item, bool withPosition = false)
        {
            if (builder.AppendOnNull(item))
            {
                return builder;
            }
            builder.Append((int)item.Type)
                .Append(' ').Append(item.Count);
            if (withPosition)
            {
                var position = item.transform.position;
                builder.Append(' ')
                    .AppendPosition(position);
            }
            return builder;
        }
        public static StringBuilder AppendShortInfo(this StringBuilder builder, Entity entity)
        {
            if (builder.AppendOnNull(entity))
            {
                return builder;
            }
            var position = entity.transform.position;
            return builder.Append((int)entity.Type)
                .Append(' ').AppendPosition(position);
        }
        public static StringBuilder AppendLongInfo(this StringBuilder builder, Block block)
        {
            if (builder.AppendOnNull(block))
            {
                return builder;
            }
            return builder.AppendShortInfo(block)
                .Append(' ').Append(block.CanGoThrough ? 1 : 0)
                .Append(' ').Append(block.CanBreak ? 1 : 0)
                .Append(' ').Append(block.Durability);
        }
        public static StringBuilder AppendLongInfo(this StringBuilder builder, Item item, bool withPosition = false)
        {
            if (builder.AppendOnNull(item))
            {
                return builder;
            }
            return builder.AppendShortInfo(item, withPosition);
        }
        public static StringBuilder AppendLongInfo(this StringBuilder builder, Entity entity)
        {
            if (builder.AppendOnNull(entity))
            {
                return builder;
            }
            return builder.AppendShortInfo(entity)
                .Append(' ').Append(entity.transform.eulerAngles.z)
                .Append(' ').Append(entity.Health);
        }

        private static bool AppendOnNull(this StringBuilder builder, MonoBehaviour obj)
        {
            if (obj == null)
            {
                builder.Append(0);
                return true;
            }
            return false;
        }
    }
}
