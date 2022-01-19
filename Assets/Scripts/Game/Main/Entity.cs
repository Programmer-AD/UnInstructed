using System;
using System.Linq;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public partial class Entity : GameObjectBase<EntityType, EntityData>
    {
        [SerializeField, Min(1)]
        private float moveSpeed, rotationSpeed, maxHealth;
        public float MoveSpeed => moveSpeed;
        public float RotationSpeed => rotationSpeed;
        public float MaxHealth => maxHealth;

        [SerializeField, Min(0)]
        private int inventorySize;
        public int InventorySize => inventorySize;

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
            set => selectedInventorySlot = Math.Clamp(value, 0, inventorySize);
        }

        public Inventory Inventory { get; private set; }
        public Item HandItem => Inventory[selectedInventorySlot];

        public override void Reset()
        {
            base.Reset();

            moveSpeed = 1;
            rotationSpeed = 1;
            maxHealth = 100;
            inventorySize = 1;
            DeathParticles = null;
        }

        public event Action<Entity> Death;
        private void OnDeath()
        {
            if (!Dead)
            {
                if (DeathParticles != null)
                {
                    var deathParticles = Instantiate(DeathParticles, transform.position, transform.rotation);
                    Destroy(deathParticles, deathParticles.main.duration);
                }

                Death?.Invoke(this);
                Dead = true;
                Destroy(gameObject);

                Director.World.SetEntitiesNeedUpdate();
            }
        }

        protected override void SaveSub(EntityData memento)
        {
            memento.X = transform.position.x;
            memento.Y = transform.position.y;
            memento.Rotation = transform.rotation.eulerAngles.z;

            memento.Health = Health;
            memento.SelectedInventorySlot = SelectedInventorySlot;
            memento.Inventory = Inventory.Select(x => x != null ? x.Save() : null).ToArray();
        }

        protected override void LoadSub(EntityData memento, GameObjectFactory factory)
        {
            transform.SetPositionAndRotation(
                new Vector2(memento.X, memento.Y),
                Quaternion.AngleAxis(memento.Rotation, Vector3.forward));

            Health = memento.Health;
            selectedInventorySlot = memento.SelectedInventorySlot;
            Inventory = new Inventory(inventorySize);

            var position = 0;
            foreach (var itemData in memento.Inventory)
            {
                if (itemData != null)
                {
                    var item = factory.Load(itemData);
                    item.OnScene = false;
                    Inventory[position++] = item;
                }
            }
        }

        protected override void InitDefaultSub(GameObjectFactory factory)
        {
            health = maxHealth;
            selectedInventorySlot = 0;
            Dead = false;
            if (inventorySize > 0)
            {
                Inventory = new Inventory(inventorySize);
            }
        }
    }
}
