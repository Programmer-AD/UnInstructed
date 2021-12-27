using System;
using System.Collections;
using System.Collections.Generic;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Saving;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public class Entity : GameObjectBase<EntityType, EntityData>
    {
        [SerializeField, Min(1)]
        private float maxHealth;
        public float MaxHealth => maxHealth;

        [SerializeField, Min(0)]
        private int inventorySize;
        public int InventorySize => inventorySize;

        [SerializeField]
        private bool canDie;
        public bool CanDie => canDie;

        [SerializeField]
        private ParticleSystem DeathParticles;

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
        public int SelectedInventorySlot
        {
            get => selectedInventorySlot;
            set
            {
                selectedInventorySlot = Math.Clamp(value, 0, inventorySize);
            }
        }

        public Inventory Inventory;
        public Item HandItem => Inventory[selectedInventorySlot];

        public override void Reset()
        {
            base.Reset();

            maxHealth = 100;
            inventorySize = 1;
            canDie = true;
        }

        public override void Start()
        {
            base.Start();

            health = maxHealth;
            selectedInventorySlot = 0;
            Dead = false;
            if (inventorySize > 0)
            {
                Inventory = new Inventory(inventorySize);
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
                var item = HandItem;
                item.UseOnBlock(this, block);

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

        protected override void LoadSub(EntityData memento)
        {
            transform.SetPositionAndRotation(
                new Vector2(memento.X, memento.Y), 
                Quaternion.AngleAxis(memento.Rotation, Vector3.forward));

            Health = memento.Health;
            SelectedInventorySlot = memento.SelectedInventorySlot;
            Dead = memento.Dead;
        }

        protected override void SaveSub(EntityData memento)
        {
            memento.X = transform.position.x;
            memento.Y = transform.position.y;
            memento.Rotation = transform.rotation.eulerAngles.z;

            memento.Health=Health;
            memento.SelectedInventorySlot=SelectedInventorySlot;
            memento.Dead=Dead;
        }
    }
}
