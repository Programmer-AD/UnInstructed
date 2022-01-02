using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Saving.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components.Dialogs
{
    public abstract class DialogBase:MonoBehaviour
    {
        public bool Opened
        {
            get => gameObject.activeSelf;
            protected set
            {
                gameObject.SetActive(value);
            }
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
