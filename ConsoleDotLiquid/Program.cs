/******************************************************************
 * File: Program.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using DotLiquid;
internal class Program
{
    public class Class : ILiquidizable
    {
        public Class(List<Member> members)
        {
            this.members = members;
            foreach (Member m in members) { m.owner = this; }
        }
        public string Name { get; set; }    
        public string Type { get; set; }
   
        public class Member : ILiquidizable
        {
            public Class owner;
            public string Name { get; set; }
            public List<Member> destinators=new List<Member>();
   
            public object ToLiquid()
            {
                return new Dictionary<string, object> { { "Name",Name }, { "Owner",owner?.Name},{ "destinators", destinators } };
            }

        }
        public List<Member> members;
        public object ToLiquid()
        {
            Dictionary<string,object> dicMember = new Dictionary<string,object>();
            foreach( var member in members ) 
                dicMember.Add(member.Name, member.ToLiquid());
            return new Dictionary<string, object>() { { "Name", Name }, { "Type", Type }, { "members", dicMember } };
        }

    }
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
        Class[] classes = new Class[] { new Class(new List<Class.Member>() {  new Class.Member() { Name = "m1" }, new Class.Member() { Name = "m3" } }) { Name = "a1", Type = "s" }, new Class(new List<Class.Member>() { new Class.Member() { Name = "m1" }, new Class.Member() { Name = "m3" } }) { Name = "a2",Type = "d"} };
        classes[0].members[0].destinators.Add(classes[1].members[0]);
        classes[0].members[1].destinators.Add(classes[1].members[1]);
        string TemplateBody3 = @"
@startuml
title =ACS+DummySystem1(Event processing)+TWO/TX
box ""PCI DSS zone"" #EEEEFF
participant ""ACS""     as ACS  order 10  #FF9999
participant ""DMN""      as IU   order 20 #99FF99
participant ""TWO""  as TWO  order 30  #FF9999
participant ""PGSQL""  as PG  order 30  #FF9999
end box
participant ""Event\nProcessing"" as DummySystem1 order 40 #99FF99
legend left
<#FFFFFF,#FFFFFF>|<#99FF99>   | DummySystem1 components|   |<#FFFFFF,#FFFFFF>|<#FF9999>   | Other C+ components|
endlegend
autonumber
  == AReq ==
  [o->ACS:Http
  ACS -[#FF3333]> DummySystem1 : <b>TECAP: TdsPrepareAuth</b>/Req
  DummySystem1 -[#00FF00]> IU: <b>OPEN API: TdsPrepareAuth</b>/Req
  IU -[#00FF00]> TWO: <b>OPEN API: TdsPrepareAuth</b>/Req
  TWO -[#00FF00]>ACS : <b>OPEN API: TdsPrepareAuth+Extensions(DummySystem1Result)</b>/Resp
  ACS->[:Http
  TWO -[#FF3333]> PG : <b>TECAP: TdsPrepareAuth</b>/Resp
@enduml";
        string TemplateBody2 = @"
@startuml
!pragma svginterface true
title =ACS->TWO transform{% for object in objects %}
class {{object.Name}} << ({{object.Type}},orchid) >>
{
{% for member in object.members %}
+{{member.Name}}{% endfor %}
}{% endfor %}{% for object in objects %}{% for member in object.members %}{% for dest in member.destinators %}
{{object.Name}}::{{member.Name}}->{{dest.Owner}}::{{dest.Name}}{% endfor %}{% endfor %}{% endfor %}
@enduml
";
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
        Template template = Template.Parse(TemplateBody2); // Parses and compiles the template
                                                          //        Template template = Template.Parse("hi {{name}}"); // Parses and compiles the template
        var res = template.Render((Hash.FromDictionary(new Dictionary<string, object>() { { "objects", classes } }))); // => "hi tobi"
//        var res = template.Render((Hash.FromDictionary(new Dictionary<string, object>() { { "products", new Product[] {new Product(1),new Product(2) } }, { "products1", 2 } }))); // => "hi tobi"
       // var res =template.Render((Hash.FromAnonymousObject(new { name = "tobi" }))); // => "hi tobi"
        Console.WriteLine($"Hello, World! {res}");
    }
}