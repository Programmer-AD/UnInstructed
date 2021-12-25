using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public class Block:MonoBehaviour
    {
        [SerializeField, Min(1)]
        private float maxDurability;
        
        [SerializeField]
        private bool canBreak;

        [SerializeField]
        private string blockName;

        public ParticleSystem BreakParticles;

        public string BlockName { get => blockName; }
        public string ShowName { get; set; }

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

        public void Reset()
        {
            canBreak = false;
            blockName = "Block Name";
            maxDurability = 1;
        }

        public void Start()
        {
            durability = maxDurability;
            ShowName = BlockName;
            Broken = false;
        }

        public bool CanUseItem => UsedItem != null;
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
                    var deathParticles = Instantiate(BreakParticles, transform.position, transform.rotation);
                    Destroy(deathParticles, deathParticles.main.duration);
                }

                Break?.Invoke(this);
                Broken = true;
                Destroy(gameObject);
            }
        }
    }
}
