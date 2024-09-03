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

namespace UniElLib;

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
        ref AbstrParser.UniEl rootElement,ContextItem context)
    {
        if (isNew)
        {
            if (tokens == null)
                if(conditionPath!= null)
                    tokens = conditionPath.Split("/");
                else
                    tokens = new string[0];
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

            return rootElement.getAllDescentants(tokens, index, context).Where(ii => conditionCalcer.Compare(ii));
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
        DEL,
        EQ,
        NOT_EQ
    };

    public Action action { get; set; } = Action.AND;
    public Filter[] filters = new Filter[] { new ConditionFilter() };

    //public bool isRelativePathFind = true;


    IEnumerable<AbstrParser.UniEl> filterForFilterAnd(List<AbstrParser.UniEl> list, int index, AbstrParser.UniEl el,ContextItem context)
    {
        AbstrParser.UniEl rootEl = el;
        foreach (var it in filters[index].filter(list, ref rootEl,context))
            if (index >= filters.Length - 1)
                yield return it;
            else
                foreach (var it1 in filterForFilterAnd(list, index + 1, it,context))
                    yield return it1;
    }

    public override IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list,
        ref AbstrParser.UniEl rootElement, ContextItem context)
    {
        return filt(list, context);
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

    IEnumerable<AbstrParser.UniEl> filt(List<AbstrParser.UniEl> list,ContextItem context)
    {
        if (action == Action.EQ)
        {
            string val = null;
            foreach (var flt in filters)
            {
                AbstrParser.UniEl rEl = null;
                bool found = false;
                foreach (var res in flt.filter(list, ref rEl, context))
                {
                    found = true;
                    if (val != null && val != res?.Value?.ToString())
                        yield break;
                    val = res?.Value?.ToString();
                    break;
                }
                if (!found)
                    yield break;

            }
            yield return list[0];
        }
        else
        {
            if (action == Action.NOT_EQ)
            {
                string val = null;
                foreach (var flt in filters)
                {
                    AbstrParser.UniEl rEl = null;
                    bool found = false;
                    foreach (var res in flt.filter(list, ref rEl, context))
                    {
                        found = true;
                        if (val != null && val == res?.Value?.ToString())
                            yield break;
                        val = res?.Value?.ToString();
                        break;
                    }
                    if (!found)
                        yield break;

                }
                yield return list[0];
            }
            else
            {
                //            List<AbstrParser.UniEl> answers = new List<AbstrParser.UniEl>();
                if (action == Action.OR)
                {
                    foreach (var flt in filters)
                    {
                        AbstrParser.UniEl rEl = null;
                        foreach (var res in flt.filter(list, ref rEl, context))
                            yield return res;
                    }
                }
                else
                    foreach (var res in filterForFilterAnd(list, 0, null, context))
                        yield return res;
            }
        }
    }
}

public abstract class Filter
{
    public abstract IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list,
        ref UniElLib.AbstrParser.UniEl rootElement,ContextItem context);
}