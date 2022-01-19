using UnityEngine;

namespace Uninstructed.UI.Components.Dialogs
{
    public abstract class DialogBase : MonoBehaviour
    {
        public bool Opened
        {
            get => gameObject.activeSelf;
            protected set => gameObject.SetActive(value);
        }

        public virtual void Start()
        {
            Opened = false;
        }

        public virtual void Reset()
        {

        }
    }
}
