namespace UniElLib;

public abstract class AliasProducer
{
    public abstract string getAlias(string originalValue);
}

[Sensitive("FIO")]
public class AliasFIO : AliasProducer
{

    public override string getAlias(string originalValue)
    {
        var tt =originalValue.Split(' ');
        return tt.Select(query => query.Substring(0,1)).Aggregate((a, b) => a + " " + b+"."); ;
    }
}

[Sensitive("PAN")]
public class AliasPan : AliasProducer
{
    public override string getAlias(string originalValue)
    {
        if (originalValue.Length >= 16)
            return $"{originalValue.Substring(0, 4)}***{originalValue.Substring(originalValue.Length - 3)}";
        return "*";
    }
}

[Sensitive("PHONE")]
public class AliasPhone : AliasProducer
{
    public override string getAlias(string originalValue)
    {
        if (originalValue.Length > 3)
            return $"{originalValue.Substring(0, 2)}*{originalValue.Substring(originalValue.Length - 3)}";
        return "*";
    }
}