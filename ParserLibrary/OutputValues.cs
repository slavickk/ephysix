/******************************************************************
 * File: OutputValues.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using CSScriptLib;
using YamlDotNet.Serialization;
using DotLiquid;
using UniElLib;
using System.Text.RegularExpressions;
namespace ParserLibrary;

public class ExtractFromInputValueWithScript: ExtractFromInputValue
{
    MethodDelegate checker = null;
    string body = @"using System;
using System.Linq;
using ParserLibrary;
string  ConvObject(AbstrParser.UniEl el)
{                                                           
            var sb = new StringBuilder();
            el.ancestor.childs.ForEach(s => sb.Append(s.Value));
            return new AbstrParser.UniEl() { Value = sb};
}
";
        
/*        AbstrParser.UniEl ConvObject(AbstrParser.UniEl el)
        {
            var sb = new StringBuilder();
            el.ancestor.childs.ForEach(s => sb.Append(s.Value+";"));
            return new AbstrParser.UniEl() { Value = sb };
        }*/
    public string ScriptBody
    {
        get
        {
            return body;
        }
        set
        {
            body = value;
            checker = CSScript.RoslynEvaluator
                .CreateDelegate(body);

        }
    }

    public override object getValue(AbstrParser.UniEl rootEl)
    {
        return (getNode(rootEl).Value.ToString());
        //return base.getValue(rootEl);
    }

/*    public override AbstrParser.UniEl getFinalNode(AbstrParser.UniEl el)
    {
        return checker(el.Value.ToString()) as AbstrParser.UniEl;
//            return base.getNode(rootEl);
    }*/
}

public abstract class OutputValue:ILiquidizable
{
    public bool viewAsJsonString = false;
    public string outputPath;
    public bool isUniqOutputPath = true;

    public enum TypeCopy
    {
        Value,
        Structure
    };

    public TypeCopy typeCopy = TypeCopy.Value;

    public enum OnEmptyAction
    {
        Skip,
        FillEmpty
    };

    public OnEmptyAction onEmptyValueAction = OnEmptyAction.Skip;

    public ConverterOutput converter = null;
    [YamlIgnore] public virtual bool canReturnObject => true;

    public abstract object getValue(AbstrParser.UniEl rootEl);
    public abstract AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl);
    public abstract IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl, ContextItem context);

    string[] outs = null;

    protected virtual AbstrParser.UniEl createOutPath(AbstrParser.UniEl outputRoot)
    {
        if (outs == null && outputPath != "")
        {
            outs = outputPath.Split("/");
        }

        if (outs == null)
            return outputRoot;
        var rootEl = outputRoot;
        for (int i = 0; i < outs.Length; i++)
        {
            var el = rootEl.childs.LastOrDefault(ii => ii.Name == outs[i]);
            if (el == null || (!isUniqOutputPath && i == outs.Length - 1))
                el = new AbstrParser.UniEl(rootEl) { Name = outs[i] };
            rootEl = el;
        }

        if (viewAsJsonString)
            rootEl.packToJsonString = true;
        return rootEl;
    }

    public bool getNodeNameOnly = false;
    public bool returnOnlyFirstRow = true;


    public IEnumerable<object> getAllObject(AbstrParser.UniEl inputRoot)
    {
        bool found = false;
        foreach (var el1 in getNodes(inputRoot))
        {
            AbstrParser.UniEl el = new AbstrParser.UniEl();
            found = true;
            if (!this.canReturnObject)
            {
            }

            if (el1 == null && onEmptyValueAction == OnEmptyAction.Skip && this.canReturnObject)
                yield break;
            if (!this.canReturnObject || el1.childs.Count == 0)
            {
                object elV;
                if (getNodeNameOnly && el1 != null)
                    elV = el1.Name;
                else
                {
                    if (canReturnObject)
                        elV = el1.Value.ToString();
                    else
                        elV = getValue(inputRoot);
                }

                if (elV != null)
                {
                    if (converter != null)
                        yield return converter.Convert(elV.ToString(), inputRoot, el).Value;
                    else
                        yield return elV;
                }
            }
            else
            {
                CopyNode(el1, el);
                yield return el;    
                //                    el.childs.Add(el1.copy(el));
            }

            /*   else
                   {
                       var el = createOutPath(outputRoot);

                       el.childs.Add(inputRoot.copy(outputRoot));
                   }*/
            if (returnOnlyFirstRow)
                yield break; 
        }

    }
    public virtual bool addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot,ContextItem context)
    {
       
        // skipped--------------------------- Пока поддерживается только линейная структура записи
        //     if (typeCopy == TypeCopy.Value)
        bool found = false;
        foreach (var el1 in getNodes(inputRoot,context))
        {
            found = true;
            if (!this.canReturnObject)
            {
            }

            if (el1 == null && onEmptyValueAction == OnEmptyAction.Skip && this.canReturnObject)
                return false;
            var el = createOutPath(outputRoot);
            //                AbstrParser.UniEl el = new AbstrParser.UniEl(outputRoot);
            //el.Name = outputPath;
            //                if(el.)
            //                if(el1.)
            if (!this.canReturnObject || el1.childs.Count == 0)
            {
                object elV;
                if (getNodeNameOnly && el1 != null)
                    elV = el1.Name;
                else
                {
                    if (canReturnObject)
                        elV = el1.Value.ToString();
                    else
                        elV = getValue(inputRoot);
                }

                if (elV != null)
                {
                    if (converter != null)
                        el = converter.Convert(elV.ToString(), inputRoot, el);
                    else
                        el.Value = elV;
                }
            }
            else
            {
                CopyNode(el1, el);
                //                    el.childs.Add(el1.copy(el));
            }

            /*   else
                   {
                       var el = createOutPath(outputRoot);

                       el.childs.Add(inputRoot.copy(outputRoot));
                   }*/
            if (returnOnlyFirstRow)
                return true;
        }

        return found;
    }

    protected virtual void CopyNode(AbstrParser.UniEl el1, AbstrParser.UniEl el)
    {
        el1.copy(el);
    }

    public virtual Dictionary<string, object> getLiquidDict()
    {
        return new Dictionary<string, object>();
    }
public object ToLiquid()
    {
        return getLiquidDict();
    }
}

public class TemplateSenderOutputValue : OutputValue
{
    string templ;
    AbstrParser.UniEl rootElement;

    public string templateBody
    {
        get { return ownerSender.getTemplate(key); }
        set
        {
            ownerSender.setTemplate(key, value);
            SetTemplate(value);
        }
    }

    private void SetTemplate(string value)
    {
        templ = value;
        rootElement = AbstrParser.ParseString(templ);
    }


    Sender ownerSender;
    string key;

    public TemplateSenderOutputValue(Sender sender, string key1)
    {
        key = key1;
        ownerSender = sender;
    }

    public override bool canReturnObject => false;

    public override bool addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot,ContextItem context)
    {
        foreach (var el in rootElement.childs)
            el.copy(outputRoot);
        return true;
        //            return base.addToOutput(inputRoot, ref outputRoot);
    }

    public override object getValue(AbstrParser.UniEl rootEl)
    {
        return null;
    }

    public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
    {
        return null;
    }

    public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl)
    {
        return new AbstrParser.UniEl[] { null };
    }
}

public class TemplateOutputValue : OutputValue
{
    string templ;
    AbstrParser.UniEl rootElement;

    public string templateBody
    {
        get { return templ; }
        set
        {
            templ = value;
            List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            rootElement = AbstrParser.CreateNode(null, list, "TT");
            try
            {
                //            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };
                foreach (var pars in AbstrParser.availParser)
                    if (pars.canRazbor(templ, rootElement, list))
                    {
                    }
            }
            catch
            {
            }
        }
    }

    public override bool canReturnObject => false;

    public override bool addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot, ContextItem context)
    {
        foreach (var el in rootElement.childs)
            el.copy(outputRoot);
        return true;
//            return base.addToOutput(inputRoot, ref outputRoot);
    }

    public override object getValue(AbstrParser.UniEl rootEl)
    {
        return null;
    }

    public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl)
    {
        return new AbstrParser.UniEl[] { null };
    }

    public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
    {
        return null;
    }
}

public class ExtractFromInputValue : OutputValue,ILiquidizable
{
    public string preoPath(string path)
    {
        var path1 = path.Replace("/#text", "").Replace("@","");

        int pos=path1.LastIndexOf('/');
        if (pos > 0)
        {
            if(path1.Substring(pos + 1)=="*")
            {
                path1= path1.Substring(0, pos);
                pos=path1.LastIndexOf("/");
                path1 += "_All";
            }
            path1 = path1.Substring(pos + 1).Replace("-", "");
            if (string.IsNullOrEmpty(path1))
            {
                int yy = 0;
            }
            return path1;
        }
        if (string.IsNullOrEmpty(path1))
        {
            int yy = 0;
        }
        return path1;
    }
    public override Dictionary<string, object> getLiquidDict()
    {
        if (!string.IsNullOrEmpty(this.conditionPath) && !string.IsNullOrEmpty(this.outputPath))
            return new Dictionary<string, object>() { { "Input", preoPath(this.conditionPath) }, { "Output", preoPath(this.outputPath) } };
        else
            return new Dictionary<string, object>();
    }
    protected override void CopyNode(AbstrParser.UniEl el1, AbstrParser.UniEl el)
    {
        if (this.copyChildsOnly)
        {
            foreach (var it in el1.childs)
                base.CopyNode(it, el);
        }
        else
            base.CopyNode(el1, el);
    }

    public override string ToString()
    {
        return outputPath + "; from " + conditionPath;
    }

    public bool copyChildsOnly = false;


    public string conditionPath { get; set; }
    [YamlIgnore] public string[] conditionPathToken = null;
    public ComparerV conditionCalcer { get; set; }
    public string valuePath { get; set; } = "";
    [YamlIgnore] public string[] valuePathToken = null;

    public string transformRegularExpression { get; set; } = "";
    Regex regex = null;
    string calcRegEx(string input)
    {
        if (regex== null)
        {
            regex = new Regex(transformRegularExpression);
        }
        Match match = regex.Match(input);

        if (match.Success)
        {
          return match.Value;
     //       Console.WriteLine($"First {n} characters: {result}");
        }
        else
        {
            return null;
           // Console.WriteLine("No match found.");
        }
    }

    public override object getValue(AbstrParser.UniEl rootEl)
    {
        if (!string.IsNullOrEmpty(transformRegularExpression))
            return calcRegEx(getNode(rootEl).Value);
        return getNode(rootEl).Value;
    }

    private AbstrParser.UniEl getLocalRoot(string[] patts, int indexF, AbstrParser.UniEl item1)
    {
        var nodes = patts[indexF].Split("/");
        var index = nodes.Length - 1;
        while (index >= 0 && AbstrParser.isEqual(item1.Name, nodes[index]))
        {
            item1 = item1.ancestor;
            index--;
        }

        return item1;
    }


    public virtual AbstrParser.UniEl getFinalNode(AbstrParser.UniEl node)
    {
        return node;
    }

    public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl)
    {
        if (conditionPathToken == null)
            conditionPathToken = conditionPath.Split("/");

        var rootEl1 = AbstrParser.getLocalRoot(rootEl, conditionPathToken);

        foreach (var item in rootEl1.getAllDescentants(conditionPathToken, rootEl1.rootIndex)
                     .Where(ii => ((conditionCalcer == null) ? true : conditionCalcer.Compare(ii))))
        {
            var item1 = item;
            if (valuePath != "")
            {
                if (valuePathToken == null)
                    valuePathToken = valuePath.Split("/");
                item1 = AbstrParser.getLocalRoot(item1, valuePathToken);
                foreach (var item2 in item1.getAllDescentants(valuePathToken, item1.rootIndex))
                    yield return getFinalNode(item2);
            }
            else
                yield return getFinalNode(item);
        }
    }

    public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
    {
        if (ConditionFilter.isNew)
        {
            if (conditionPathToken == null)
                conditionPathToken = conditionPath.Split("/");

            var rootEl1 = AbstrParser.getLocalRoot(rootEl, conditionPathToken);

            foreach (var item in rootEl1.getAllDescentants(conditionPathToken, rootEl1.rootIndex)
                         .Where(ii => ((conditionCalcer == null) ? true : conditionCalcer.Compare(ii))))
            {
                var item1 = item;
                if (valuePath != "")
                {
                    if (valuePathToken == null)
                        valuePathToken = valuePath.Split("/");
                    item1 = AbstrParser.getLocalRoot(item1, valuePathToken);
                    foreach (var item2 in item1.getAllDescentants(valuePathToken, item1.rootIndex))
                        return getFinalNode(item2);
                }
                else
                    return getFinalNode(item);
            }
        }
        else
        {
            var pathOwn = rootEl.path;
            var patts1 = AbstrParser.PathBuilder(new string[] { pathOwn, conditionPath });


            var patts = AbstrParser.PathBuilder(new string[] { conditionPath, valuePath });

            var rootEl1 = getLocalRoot(patts1, 0, rootEl);

            foreach (var item in rootEl1.getAllDescentants().Where(ii =>
                         ii.path == conditionPath && ((conditionCalcer == null) ? true : conditionCalcer.Compare(ii))))
            {
                var item1 = item;
                if (valuePath != "")
                {
                    item1 = getLocalRoot(patts, 0, item1);
                    foreach (var item2 in item1.getAllDescentants().Where(ii => ii.path == valuePath))
                        return getFinalNode(item2);
                }
                else
                    return getFinalNode(item);
            }
        }

        return null;
    }
}

public class ConstantValue : OutputValue
{
    public static object ConvertFromType(string value, TypeObject tObject)
    {
        switch (tObject)
        {
            case TypeObject.String:
                return value;
                break;

            case TypeObject.Number:
                return Convert.ToDouble(value);
                break;
            case TypeObject.Boolean:
                return Convert.ToBoolean(value);
                break;
        }

        return value;
    }

    public enum TypeObject
    {
        String,
        Number,
        Boolean
    };

    public TypeObject typeConvert = TypeObject.String;

    public override string ToString()
    {
        return outputPath + ";" + Value;
    }

    public object Value { get; set; }

    public override bool canReturnObject => false;

    public override object getValue(AbstrParser.UniEl rootEl)
    {
        var valFunc = EmbeddedFunctions.exec(Value?.ToString());
        if (!string.IsNullOrEmpty(valFunc))
            return valFunc;
        if (Value.GetType() == typeof(string) && typeConvert != TypeObject.String)
            return ConvertFromType(Value.ToString(), typeConvert);
        return Value;
    }

    public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
    {
        return null;
    }

    public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl)
    {
        return new AbstrParser.UniEl[] { null };
    }
}