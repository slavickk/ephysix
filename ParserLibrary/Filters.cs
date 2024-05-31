/******************************************************************
 * File: Filters.cs
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

using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace ParserLibrary;

public class ArrFilter : List<Filter>
{

}

public class ConditionFilter : Filter
{
    public string conditionPath { get; set; }
    [YamlIgnore] public string[] tokens = null;

    /*public ConditionFilter()
        {

        }*/
    //                                  Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name
    //Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name
    //       [JsonInclude]
    public ComparerV conditionCalcer { get; set; } = new ScriptCompaper();
    public static bool isNew = true;

    public override IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list,
        ref AbstrParser.UniEl rootElement)
    {
        if (isNew)
        {
            if (tokens == null)
                tokens = conditionPath.Split("/");
            int index = 0;
            if (rootElement == null)
            {
                rootElement = list[0];
                index = 0;
            }
            else
            {
                rootElement = AbstrParser.getLocalRoot(rootElement, tokens);
                index = rootElement.rootIndex;
            }

            return rootElement.getAllDescentants(tokens, index).Where(ii => conditionCalcer.Compare(ii));
        }
        else
            return list.Where(ii => ii.path == conditionPath && conditionCalcer.Compare(ii));
    }
}

public class AndOrFilter : Filter
{
    public enum Action
    {
        OR,
        AND,
        DEL
    };

    public Action action { get; set; } = Action.AND;
    public Filter[] filters = new Filter[] { new ConditionFilter() };

    //public bool isRelativePathFind = true;


    IEnumerable<AbstrParser.UniEl> filterForFilterAnd(List<AbstrParser.UniEl> list, int index, AbstrParser.UniEl el)
    {
        AbstrParser.UniEl rootEl = el;
        foreach (var it in filters[index].filter(list, ref rootEl))
            if (index >= filters.Length - 1)
                yield return it;
            else
                foreach (var it1 in filterForFilterAnd(list, index + 1, it))
                    yield return it1;
    }

    public override IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list,
        ref AbstrParser.UniEl rootElement)
    {
        return filt(list);
        //            List<AbstrParser.UniEl> answers = new List<AbstrParser.UniEl>();
        /*  if (action == Action.OR)
            {
                foreach (var flt in filters)
                {
                    AbstrParser.UniEl rEl = null;
                    foreach (var res in flt.filter(list, ref rEl))
                        yield return res;

                }
            }
            else
                foreach(var res in  filterForFilterAnd(list, 0, null))
                    yield return res;

            */
    }

    IEnumerable<AbstrParser.UniEl> filt(List<AbstrParser.UniEl> list)
    {
        //            List<AbstrParser.UniEl> answers = new List<AbstrParser.UniEl>();
        if (action == Action.OR)
        {
            foreach (var flt in filters)
            {
                AbstrParser.UniEl rEl = null;
                foreach (var res in flt.filter(list, ref rEl))
                    yield return res;
            }
        }
        else
            foreach (var res in filterForFilterAnd(list, 0, null))
                yield return res;
    }
}

public abstract class Filter
{
    public abstract IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list,
        ref AbstrParser.UniEl rootElement);
}