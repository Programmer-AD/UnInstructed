using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public abstract class GameObjectAdditionBase<TPrimary>:MonoBehaviour
        where TPrimary : MonoBehaviour
    {
        protected TPrimary primary;

        public GameObjectAdditionBase()
        {
            primary = GetComponent<TPrimary>();
        }
    }
}
