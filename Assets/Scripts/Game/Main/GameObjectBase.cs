using System;
using Uninstructed.Game.Saving.Interfaces;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public abstract class GameObjectBase<TEnum, TMemento> : MonoBehaviour, ISaveable<TMemento>
        where TEnum : Enum where TMemento : GameObjectData<TEnum>, new()
    {
        [SerializeField]
        private TEnum type;
        public TEnum Type { get => type; }

        [SerializeField]
        private string defaultName;
        public string DefaultName => defaultName;

        public string ShowName { get; set; }

        public virtual void Reset()
        {
            type = (TEnum)Enum.ToObject(typeof(TEnum), ushort.MaxValue); ;
            defaultName = "Default name";
        }

        public void InitDefault(GameObjectFactory factory)
        {
            ShowName = defaultName;

            InitDefaultSub(factory);
        }
        protected abstract void InitDefaultSub(GameObjectFactory factory);

        public void Load(TMemento memento, GameObjectFactory factory)
        {
            type = memento.Type;
            ShowName = memento.ShowName ?? defaultName;

            LoadSub(memento, factory);

            var additions = GetComponents<ISaveableAddition>();
            foreach (var addition in additions)
            {
                addition.Load(memento.Additionals, factory);
            }
        }
        protected abstract void LoadSub(TMemento memento, GameObjectFactory factory);

        public TMemento Save()
        {
            var memento = new TMemento();
            if (ShowName != defaultName)
            {
                memento.ShowName = ShowName;
            }

            SaveSub(memento);

            var additions = GetComponents<ISaveableAddition>();
            foreach (var addition in additions)
            {
                addition.Save(memento.Additionals);
            }
            return memento;
        }
        protected abstract void SaveSub(TMemento memento);
    }
}
