using System;

namespace Uninstructed.Game
{
    internal class LazyUpdateable<T>
    {
        public static implicit operator T(LazyUpdateable<T> updateable)
        {
            return updateable.Value;
        }

        private readonly Func<T, T> updater;
        private bool needUpdate;
        private T value;

        public LazyUpdateable(Func<T, T> updater)
        {
            this.updater = updater;
            needUpdate = true;
        }

        public bool NeedUpdate => needUpdate;
        public T Value
        {
            get
            {
                if (needUpdate)
                {
                    Update();
                }
                return value;
            }
        }

        public void SetNeedUpdate()
        {
            needUpdate = true;
        }
        public void ForceUpdate()
        {
            Update();
        }

        private void Update()
        {
            value = updater(value);
            needUpdate = false;
        }
    }
}
