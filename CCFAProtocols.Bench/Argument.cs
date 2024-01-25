namespace CCFAProtocols.Bench;

public class Argument<T>
{
    private readonly string _name;
    private T obj;

    public Argument(T val, string? name = null)
    {
        _name = name ?? val.ToString();
        obj = val;
    }

    public T Get() => obj;

    public static implicit operator T(Argument<T> arg) => arg.obj;

    public override string ToString()
    {
        return _name;
    }
}