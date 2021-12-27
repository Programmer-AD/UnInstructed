using System;
using System.Collections;
using System.Collections.Generic;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Saving;
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

        public override void Start()
        {
            base.Start();

            durability = maxDurability;
            Broken = false;
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

        protected override void LoadSub(BlockData memento)
        {
            Durability = memento.Durability;
            Broken = memento.Broken;
        }

        protected override void SaveSub(BlockData memento)
        {
            memento.Durability = durability;
            memento.Broken = Broken;
        }
    }
}
