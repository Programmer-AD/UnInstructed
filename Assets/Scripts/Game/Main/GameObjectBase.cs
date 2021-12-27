using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Saving;
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

        public virtual void Start()
        {
            ShowName = defaultName;
        }

        public void Load(TMemento memento)
        {
            type = memento.Type;
            ShowName = memento.ShowName ?? defaultName;

            LoadSub(memento);

            var additions = GetComponents<ISaveableAddition>();
            foreach (var addition in additions)
            {
                addition.Load(memento.Additionals);
            }
        }
        protected abstract void LoadSub(TMemento memento);

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
