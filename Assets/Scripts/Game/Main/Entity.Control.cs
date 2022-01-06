using System;

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

        public Vector2 LookDirection => transform.up;
        public Vector2Int LookDirectionInt => new((int)MathF.Round(LookDirection.x), (int)MathF.Round(LookDirection.y));
        public Block LookingAtBlock
        {
            get
            {
                var coord = LookDirectionInt;
                return World.Map[coord.x, coord.y];
            }
        }

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

        public bool UseItem()
        {
            if (HandItem != null)
            {
                HandItem.Use(this);
                return true;
            }
            return false;
        }

        public bool UseOnBlock()
        {
            var block = LookingAtBlock;
            if (block != null)
            {
                var item = HandItem;
                if (item != null)
                {
                    item.UseOnBlock(this, block);
                }

                block.Use(this, item);
                return true;
            }
            return false;
        }

        public bool Drop()
        {
            var item = HandItem;
            if (item != null)
            {
                //TODO: Item drop
            }
            return false;
        }

        public bool Interact(string[] command)
        {
            var block = LookingAtBlock;
            if (block != null)
            {
                block.Interact(this, command);
                return true;
            }
            return false;
        }

        private struct Movement
        {
            public MovementType Type;
            public float Value;
        }

        private enum MovementType
        {
            None, Move, Rotate
        }
    }
}
