using System;
using System.Collections;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public partial class Entity
    {
        private new Rigidbody2D rigidbody;

        private IEnumerator action;
        public bool Busy => action != null;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            if (World == null || World.Paused)
            {
                return;
            }

            if (action != null)
            {
                if (!action.MoveNext())
                {
                    action = null;
                }
            }
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
            action = Movement(distance);
        }

        public void SetRotate(float angle, bool to = false)
        {
            action = Rotation(angle, to);
        }

        

        public bool UseItem()
        {
            if (HandItem != null)
            {
                action = Other();
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
                action = Other();

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
                action = Other();
                //TODO: Item drop
                return true;
            }
            return false;
        }

        public bool Interact(string[] command)
        {
            var block = LookingAtBlock;
            if (block != null)
            {
                action = Other();
                block.Interact(this, command);
                return true;
            }
            return false;
        }

        private IEnumerator Movement(float distance)
        {
            var ended = false;
            var directedSpeed = MathF.Sign(distance) * moveSpeed;
            while (!ended)
            {
                var directed = Time.deltaTime * directedSpeed;
                float move;
                if (distance / directed <= 1)
                {
                    move = distance;
                    ended = true;
                }
                else
                {
                    distance -= directed;
                    move = directed;
                }
                rigidbody.position += move * (Vector2)transform.up;
                yield return null;
            }
        }

        private IEnumerator Rotation(float angle, bool to)
        {
            angle = EscapeAngle(angle);
            if (to)
            {
                var current = rigidbody.rotation;
                var delta = EscapeAngle(angle - current);
                var sideChange = delta - Math.Sign(delta) * 360;
                if (Math.Abs(delta) < Math.Abs(sideChange))
                {
                    angle = delta;
                }
                else
                {
                    angle = sideChange;
                }
            }

            var ended = false;
            var directedRotate = MathF.Sign(angle) * rotationSpeed;
            while (!ended)
            {
                var directed = Time.deltaTime * directedRotate;
                float rotate;
                if (angle / directed <= 1)
                {
                    rotate = angle;
                    ended = true;
                }
                else
                {
                    angle -= directed;
                    rotate = directed;
                }
                rigidbody.rotation = EscapeAngle(rigidbody.rotation + rotate);
                yield return null;
            }
        }

        private IEnumerator Other()
        {
            yield return null;
        }

        private float EscapeAngle(float angle)
        {
            var rotations = (int)angle / 360;
            return angle - rotations * 360;
        }
    }
}
