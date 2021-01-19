namespace Pizzeria_API.Data.Factory
{
    public abstract class ItemFactory<T>
    {
        public T GetItem(string name)
        {
            var item = CreateItem(name);
            return item;
        }

        protected abstract T CreateItem(string name);
    }
}
