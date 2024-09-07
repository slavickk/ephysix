using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public class PlantUMLItem
    {

        public PlantUMLItem getLastChild(string MyServiceName="Сервис платежей")
        {
            if (this.Name == MyServiceName)
                return null;
            if (this.links == null || this.links.Count == 0)
                return this;
            foreach(Link link in this.links)
            {
                return link.children.getLastChild(MyServiceName);
            }
            return null;//????
        }

        public enum TypeItem { participant, actor };
        public TypeItem type { get; set; } = TypeItem.participant;
        public class Link
        {
            public bool isError = false;
            public string NameRq { get; set; }
            public string NameRp { get; set; }
            public PlantUMLItem children { get; set; }
        }
        public string Name { get; set; }
        public string color { get; set; }
        public string shortName { get; set; }
        public List<Link> links { get; set;}


        public static string getUML(string title,IEnumerable<PlantUMLItem> items)
        {
            string retValue = $"@startuml\r\ntitle = {title}\r\n";
            List<PlantUMLItem> distinctItems = new List<PlantUMLItem>();
            foreach (var item in items)
            {
                AddToDistinct(distinctItems, item);
            }
            retValue += getParticipantDefinitions(distinctItems);
            retValue += getUMLLinks(items);
            retValue += "@enduml\r\n";
            return retValue ;
        }

        private static void AddToDistinct(List<PlantUMLItem> distinctItems, PlantUMLItem item)
        {
            if (!distinctItems.Contains(item))
            {
                distinctItems.Add(item);
                if(item.links != null)
                foreach (var item2 in item.links.Select(ii => ii.children))
                {
                    AddToDistinct(distinctItems, item2);
                }
            }
        }

        public static string getParticipantDefinitions(IEnumerable<PlantUMLItem> items)
        {
            string retValue = "";
            int index = 0;
            foreach (var item in items)
            {
                index++;
                if (string.IsNullOrEmpty(item.shortName))
                    item.shortName = $"Item{index}";
                retValue += $"{Enum.GetName<TypeItem>(item.type)} \"{item.Name.Replace("\n", "\\n")}\"              as {item.shortName}  order {index * 10} {item.color}\r\n";
            }
            return retValue;
        }

        public static string getUMLLinks(IEnumerable<PlantUMLItem> items)
        {
            string retValue = "";
            foreach(var item in items)
            {
                retValue += $"  activate {item.shortName} {item.color}\r\n";
                retValue = GetChildsLinks(retValue, item ,"",out string lastRet,out bool force);
                retValue += $"  deactivate {item.shortName}\r\n";
            }
            return retValue;
        }

        private static string GetChildsLinks(string retValue, PlantUMLItem item,string lastLinkName,out string lastRetLinkName,out bool force)
        {
            force = false;
            lastRetLinkName = "";
            if (item.links?.Count > 0)
            {
                foreach (var item2 in item.links)
                {
                    retValue += $"  activate {item2.children.shortName} {item2.children.color}\r\n";
                    string NameLinkRq=(string.IsNullOrEmpty(item2.NameRq)?lastLinkName:item2.NameRq).Replace("\n", "\\n");
                    if (item2.isError)
                    { 

                        force=true;
                        lastRetLinkName = item2.NameRp;
                        retValue += $"  {item.shortName} -[#red]>x {item2.children.shortName} : {NameLinkRq}\r\n";
                    }
                    else

                        retValue += $"  {item.shortName} -> {item2.children.shortName} : {NameLinkRq}\r\n";
                    if(!force)
                        retValue = GetChildsLinks(retValue, item2.children,NameLinkRq,out lastRetLinkName,out force);
                    if(!force)                        
                        lastRetLinkName = (string.IsNullOrEmpty(item2.NameRp) ? (lastRetLinkName) : item2.NameRp).Replace("\n", "\\n");
                    if (!item2.isError)
                        retValue += $"  {item2.children.shortName} -{((item2.isError|| force)?"[#red]":"")}-> {item.shortName} : {lastRetLinkName}\r\n";

                    retValue += $"  deactivate {item2.children.shortName}\r\n";

                }
            }
            else
                retValue += $"  {item.shortName} -> {item.shortName}\r\n";

            return retValue;
        }
    }
}
