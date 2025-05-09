public interface IItem
{
    public void AddItem<T>(T type);
    public bool RemoveItem<T>(T type);
}
