using UnityEngine;

namespace Uninstructed.Game.Main
{
    public abstract class GameObjectAdditionBase<TPrimary> : MonoBehaviour
        where TPrimary : MonoBehaviour
    {
        protected TPrimary primary;

        public GameObjectAdditionBase()
        {
            primary = GetComponent<TPrimary>();
        }
    }
}
