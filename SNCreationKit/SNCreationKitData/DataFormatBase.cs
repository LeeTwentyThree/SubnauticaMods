namespace SNCreationKitData;

public abstract record DataFormatBase<T>
{
    public abstract void AssignFieldsToObject(T obj);
}