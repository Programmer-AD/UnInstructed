namespace Uninstructed.Game.Saving.Interfaces
{
    public interface ISaveable<T> where T : class
    {
        T Save();
        void Load(T memento, GameObjectFactory factory);
    }
}
