using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public class Entity : MonoBehaviour
    {
        [SerializeField, Min(1)]
        private float maxHealth;

        [SerializeField, Min(0)]
        private int inventorySize;

        [SerializeField]
        private bool canDie;

        [SerializeField]
        private string entityName;

        public ParticleSystem DeathParticles;        

        public string EntityName { get => entityName; }
        public string ShowName { get; set; }

        private float health;
        public float Health
        {
            get => health;
            set
            {
                health = Math.Clamp(value, 0, maxHealth);
                if (health == 0)
                {
                    OnDeath();
                }
            }
        }
        public bool Dead { get; private set; }

        private int selectedInventorySlot;
        public Inventory Inventory;
        public Item HandItem => Inventory[selectedInventorySlot];

        public void Reset()
        {
            maxHealth = 100;
            inventorySize = 1;
            canDie = true;
            entityName = "Entity Name";
        }

        public void Start()
        {
            ShowName = EntityName;
            health = maxHealth;
            selectedInventorySlot = 0;
            Dead = false;
            if (inventorySize > 0)
            {
                Inventory = new Inventory(inventorySize);
            }
        }

        public void SelectInventorySlot(int slot)
        {
            if (slot is >= 0 && slot < inventorySize)
            {
                selectedInventorySlot = slot;
            }
        }

        public void UseItem()
        {
            if (HandItem != null)
            {
                HandItem.Use(this);
            }
        }

        public void UseOnBlock(Block block)
        {
            var positionDelta = block.transform.position - transform.position;
            if (MathF.Abs(positionDelta.x) <= 1 && MathF.Abs(positionDelta.y) <= 1)
            {

            }
        }

        public event Action<Entity> Death;
        private void OnDeath()
        {
            if (canDie && !Dead)
            {
                if (DeathParticles != null)
                {
                    var deathParticles = Instantiate(DeathParticles, transform.position, transform.rotation);
                    Destroy(deathParticles, deathParticles.main.duration);
                }

                Death?.Invoke(this);
                Dead = true;
                Destroy(gameObject);
            }
        }
    }
}
