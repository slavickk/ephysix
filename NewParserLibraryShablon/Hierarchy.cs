using ParserLibrary;
namespace NewParserLibraryShablon
{
    public interface CheckElementValue
    {
        public bool check(AbstrParser.UniEl el);

    }
    public interface IFilter
    {
        IEnumerable<AbstrParser.UniEl> filter();
    }

    public class GroupFilter : IFilter
    {
        public enum TypeSearch { fromCurrent,fromRoot};
        public TypeSearch typeSearch;
        public enum Operation { and, or };
        public Operation operation;
        public List<IFilter> members;


        public IEnumerable<AbstrParser.UniEl> filter()
        {
            throw new NotImplementedException();
        }
    }
    public class Filter:IFilter
    {
        public Filter(AbstrParser.UniEl rootEl,string path,CheckElementValue checker)
        {

        }

        public IFilter child;

      //  public NodeComparer? comparer;
        
        public IEnumerable<AbstrParser.UniEl> filter()
        {
            throw new NotImplementedException();
        }
    }

    public interface Sender
    {
        public bool IsAsync
        {
            get;
        }
        public Task<string> Send(string,object context);

        public Task SendRequest(SemaphoreSlim sem, string, object context);
        public Task<(string,object)> 

    }
    

}