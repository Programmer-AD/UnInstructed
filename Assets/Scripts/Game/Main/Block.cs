using System;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public class Block : GameObjectBase<BlockType, BlockData>
    {
        [SerializeField, Min(1)]
        private float maxDurability;
        public float MaxDurabilty => maxDurability;

        [SerializeField]
        private bool canBreak;
        public bool CanBreak => canBreak;

        public bool CanGoThrough;

        [SerializeField]
        private ParticleSystem BreakParticles;

        private float durability;
        public float Durability
        {
            get => durability;
            set
            {
                durability = Math.Clamp(value, 0, maxDurability);
                if (durability == 0)
                {
                    OnBreak();
                }
            }
        }
        public bool Broken { get; private set; }

        public override void Reset()
        {
            base.Reset();

            canBreak = false;
            maxDurability = 1;
        }

        public event Action<Entity, Item, Block> UsedItem;
        public void Use(Entity user, Item item)
        {
            UsedItem?.Invoke(user, item, this);
        }

        public event Action<Block> Break;
        private void OnBreak()
        {
            if (canBreak && !Broken)
            {
                if (BreakParticles != null)
                {
                    var breakParticles = Instantiate(BreakParticles, transform.position, transform.rotation);
                    Destroy(breakParticles, breakParticles.main.duration);
                }

                Break?.Invoke(this);
                Broken = true;
                Destroy(gameObject);
            }
        }

        protected override void LoadSub(BlockData memento, GameObjectFactory factory)
        {
            Durability = memento.Durability;
            CanGoThrough = memento.CanGoThrough;
        }

        protected override void SaveSub(BlockData memento)
        {
            memento.Durability = durability;
            memento.CanGoThrough = CanGoThrough;
        }

        protected override void InitDefaultSub(GameObjectFactory factory)
        {
            durability = maxDurability;
            Broken = false;
        }
    }
}
