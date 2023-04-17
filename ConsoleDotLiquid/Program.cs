using DotLiquid;
internal class Program
{

    public class Product : ILiquidizable
    {
        Dictionary<string,object> data;
        public Product(int val)
        {
            data = new Dictionary<string, object>() { { "val", val },{ "val1", val + 1 } };
        }
        public object ToLiquid()
        {
            return data;
        }
    }
    private static void Main(string[] args)
    {

        string TemplateBody1 = @"
<ul id=""products"">
  {% for product in products %}
    <li>
      <h2>{{product.name}}</h2>
      Only {{product.price | price }}

      {{product.description | prettyprint | paragraph }}
    </li>
  {% endfor %}
</ul>";
        string TemplateBody = @"
<ul id=""products"">
  {% for product in products %}
    <li>
      <h2>val={{product.val}}</h2>
      <h2>val1={{product.val1}}</h2>
    </li>
  {% endfor %}
</ul>";
        //        RenderParameters param1= new RenderParameters()
        Template template = Template.Parse(TemplateBody); // Parses and compiles the template
                                                          //        Template template = Template.Parse("hi {{name}}"); // Parses and compiles the template
        var res = template.Render((Hash.FromDictionary(new Dictionary<string, object>() { { "products", new Product[] {new Product(1),new Product(2) } }, { "products1", 2 } }))); // => "hi tobi"
       // var res =template.Render((Hash.FromAnonymousObject(new { name = "tobi" }))); // => "hi tobi"
        Console.WriteLine($"Hello, World! {res}");
    }
}