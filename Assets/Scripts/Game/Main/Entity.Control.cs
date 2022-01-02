using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving.Models;

using UnityEngine;

namespace Uninstructed.Game.Main
{
    public partial class Entity
    {
        private new Rigidbody2D rigidbody;

        private Movement action = new();
        public bool Busy => action.Type != MovementType.None;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            switch (action.Type)
            {
                case MovementType.None:
                    return;
                case MovementType.Move:
                    TickMove();
                    break;
                case MovementType.Rotate:
                    TickRotate();
                    break;
            }
        }

        private void TickMove()
        {
            var maxFrameMove = Time.deltaTime * moveSpeed;
            var directed = MathF.Sign(action.Value) * maxFrameMove;
            float move;
            if (action.Value / directed <= 1)
            {
                move = action.Value;
                action.Value = 0;
                action.Type = MovementType.None;
            }
            else
            {
                action.Value -= directed;
                move = directed;
            }
            rigidbody.position += move * (Vector2)transform.up;
        }

        private void TickRotate()
        {
            var maxFrameRotate = Time.deltaTime * rotationSpeed;
            var directed = MathF.Sign(action.Value) * maxFrameRotate;
            float rotate;
            if (action.Value / directed <= 1)
            {
                rotate = action.Value;
                action.Value = 0;
                action.Type = MovementType.None;
            }
            else
            {
                action.Value -= directed;
                rotate = directed;
            }
            rigidbody.rotation = EscapeAngle(rigidbody.rotation + rotate);
        }

        public Vector2 LookAt => transform.up;
        public Vector2Int LookAtBlock => new((int)MathF.Round(LookAt.x), (int)MathF.Round(LookAt.y));


        public void SetMove(float distance)
        {
            action.Type = MovementType.Move;
            action.Value = distance;
        }

        public void SetRotate(float angle, bool to = false)
        {
            angle = EscapeAngle(angle);
            action.Type = MovementType.Rotate;
            if (to)
            {
                var current = rigidbody.rotation;
                var delta = EscapeAngle(current - angle);
                var sideChange = delta - Math.Sign(delta) * 360;
                angle = Math.Min(Math.Abs(delta), Math.Abs(sideChange));
            }
            action.Value = angle;
        }

        private float EscapeAngle(float angle)
        {
            var rotations = (int)angle / 360;
            return angle - rotations * 360;
        }

        public void UseItem()
        {
            if (HandItem != null)
            {
                HandItem.Use(this);
            }
        }

        public void UseOnBlock()
        {
            var lookAtBlock = LookAtBlock;
            var block = World.Map[lookAtBlock.x, lookAtBlock.y];

            var item = HandItem;
            if (item != null)
            {
                item.UseOnBlock(this, block);
            }

            block.Use(this, item);
        }

        public void Drop()
        {
            //TODO: Item drop
        }

        public void Interact()
        {
            //TODO: entity-block interact mechanics
        }



        struct Movement
        {
            public MovementType Type;
            public float Value;
        }

        enum MovementType
        {
            None, Move, Rotate
        }
    }
}
