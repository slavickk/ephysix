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
using System.Text.RegularExpressions;
using System.Globalization;

namespace UniElLib;

public class ExtractFromInputValueWithScript: ExtractFromInputValue
{
    MethodDelegate checker = null;
    string body = @"using System;
using System.Linq;
using ParserLibrary;
AbstrParser.UniEl  ConvObject(AbstrParser.UniEl el)
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
   /* public override object getValue(AbstrParser.UniEl rootEl)
    {
        return (getNode(rootEl).Value.ToString());
        //return base.getValue(rootEl);
    }*/
    public override AbstrParser.UniEl getFinalNode(AbstrParser.UniEl el)
    {
        return  new AbstrParser.UniEl() { Value = checker(el.Value.ToString()) } as AbstrParser.UniEl;
//            return base.getNode(rootEl);
    }
}

public abstract class OutputValue:ILiquidizable
{
    public bool alwaysInArray { get; set; } = false;
    public bool viewAsJsonString = false;
    public string outputType;
    public string outputPath;
    public bool isUniqOutputPath = true;
    public List<OutputValue> outputChilds { get; set; } = new List<OutputValue>();
    public enum TypeCopy
    {
        Value,
        Structure
    };

    public bool isExported { get; set; } = false;
    public TypeCopy typeCopy = TypeCopy.Value;

    public enum OnEmptyAction
    {
        Skip,
        FillEmpty
    };

    public OnEmptyAction onEmptyValueAction = OnEmptyAction.Skip;

    public string[] PreferableParsers;

    public ConverterOutput converter = null;
    [YamlIgnore] public virtual bool canReturnObject => true;

    public abstract object getValue(AbstrParser.UniEl rootEl);
    public abstract AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl);
    public abstract IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl,ContextItem context);

    string[] outs = null;

    protected virtual AbstrParser.UniEl createOutPath(AbstrParser.UniEl outputRoot)
    {
        if (outs == null && outputPath != "")
        {
            outs = outputPath.Split("/");
        }
        /*if(outputPath.Contains("Operator/Param/"))
        {
            int yy = 0;
        }*/
        //if(("root/"+outputPath).IndexOf(outputRoot.path)
        if (outs == null)
            return outputRoot;
        var rootEl = outputRoot;
        if (outs[outs.Length - 1] == "Param")
        {
            int yy = 0;
        }
        int ibeg1 = outs.Length - 2;
        int ibeg = -1;
        var currRoot = outputRoot;

        for(int i=ibeg1; i>=0;i-- )
        {
            if(currRoot.Name == outs[i])
            {
                ibeg = i;
                break;
            }
        }
        for(int i=ibeg; i>=0;i--)
        {
            if(currRoot.Name != outs[i])
            {
                ibeg = -1; 
                break;
            }
            //ibeg = i;
            currRoot = currRoot.ancestor;
        }
        for (int i = ibeg+1; i < outs.Length; i++)
        {
            // It is not a root
            //if (!(outs[i] == outputRoot.Name && i == 0))
            {

                var el = rootEl.childs.LastOrDefault(ii => ii.Name == outs[i]);
                if (el == null || (!isUniqOutputPath && i == outs.Length - 1))
                    el = new AbstrParser.UniEl(rootEl) { Name = outs[i] };
                rootEl = el;
            }
        }
        if(PreferableParsers != null )
        {
            foreach(var el in PreferableParsers)
                AbstrParser.AddAvalParser(rootEl, AbstrParser.availParser.First(ii=>ii.GetType().Name==el));
        }
        if (viewAsJsonString)
            rootEl.packToJsonString = true;
        return rootEl;
    }

    public bool getNodeNameOnly = false;
    public bool returnOnlyFirstRow = true;


    public IEnumerable<object> getAllObject(AbstrParser.UniEl inputRoot,ContextItem context)
    {
        bool found = false;
        foreach (var el1 in getNodes(inputRoot,context))
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
                        elV = ToOutputType(el1?.Value); //el1?.Value?.ToString()??"";
                    else
                        elV = ToOutputType(getValue(inputRoot));
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
    public virtual bool addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot, ContextItem context)
    {
       if(outputPath=="Operator/Group")
        {
            int yy = 0;
        }
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
            el.alwaysArray = this.alwaysInArray;
     //           el.alwaysArray
            //                AbstrParser.UniEl el = new AbstrParser.UniEl(outputRoot);
            //el.Name = outputPath;
            //                if(el.)
            //                if(el1.)
            if (outputChilds.Count == 0)
            {
                if (this is ExtractFromInputValue && !string.IsNullOrEmpty((this as ExtractFromInputValue).functionCall))
                {
                   var  elV =ToOutputType( GetValueSimple(el1));
                    if (elV != null)
                    {
                        if (converter != null)
                            el = converter.Convert(elV.ToString(), inputRoot, el);
                        else
                            el.Value = elV;
                    }


                    //Scalar only
                }
                else
                {
                    if (!this.canReturnObject || el1.childs.Count == 0)
                    {
                        object elV;
                        if (getNodeNameOnly && el1 != null)
                            elV = el1.Name;
                        else
                        {
                            if (canReturnObject)
                                elV =ToOutputType(GetValueSimple(el1));
                            else
                                elV = ToOutputType(getValue(inputRoot));
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
                }
            }

            /*   else
                   {
                       var el = createOutPath(outputRoot);

                       el.childs.Add(inputRoot.copy(outputRoot));
                   }*/
            var el_root = el1;
            foreach(var el_child in outputChilds)
                el_child.addToOutput(el1,ref el/* outputRoot*//*el_root*//*???*/, context);

            if (returnOnlyFirstRow)
                return true;
        }

        return found;
    }

    public object ToOutputType(object val)
    {
        if(string.IsNullOrEmpty(outputType))
            return (val ?? "").ToString();
        switch (outputType)
            {
            case "string":
                return (val??"").ToString();
            case "boolean":
                return Boolean.Parse(val.ToString());
            case "integer":
                return Int32.Parse(val.ToString());
            case "double":
                return Double.Parse(val.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                //return double.Parse(val.ToString());
            default:
                return val.ToString();


        }
        if (outputType == "string" || string.IsNullOrEmpty(outputType))
            return val;

    }

    protected  virtual string GetValueSimple(AbstrParser.UniEl? el1)
    {
        return el1?.Value?.ToString() ?? "";
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
                    if (pars.canRazbor("", templ, rootElement, list))
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

    public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl, ContextItem context)
    {
        return new AbstrParser.UniEl[] { null };
    }

    public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
    {
        return null;
    }
}
public class ExtractFromInputValueWithSwitch:ExtractFromInputValue
{
    public override bool canReturnObject => false;
    public class SwitchItem
    {
        public bool overwise { get; set; } = false;
        public string Key { get; set; }
        public string Value { get; set; }   
    }
    public List<SwitchItem> SwitchItems { get; set; } = new List<SwitchItem>();


    public override object getValue(AbstrParser.UniEl rootEl)
    {
        var val= base.getValue(rootEl).ToString();
        var retValue = SwitchItems.FirstOrDefault(ii => ii.Key == val);
        if(retValue== null)
        {
            retValue = SwitchItems.FirstOrDefault(ii => ii.overwise);
        }
        return retValue.Value;
    }
}
/*
public class ScriptFromInputValue : ExtractFromInputValue
{
    public override object getValue(AbstrParser.UniEl rootEl)
    {
        return (getNode(rootEl).Value.ToString());
        //return base.getValue(rootEl);
    }
}
*/
public class ExtractFromInputValue : OutputValue
{
    public override bool canReturnObject => string.IsNullOrEmpty(transformRegularExpression);
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


    string funcText;
    protected Func<AbstrParser.UniEl,object> func=null;
    public string functionCall {
        get => funcText;
       
        set
        {
            funcText= value;
            if(!string.IsNullOrEmpty(value))
            {
                if(!(funcText.Substring(0,1)=="$" && funcText.Substring(funcText.Length-1) == "$"))
                {
                    func = EmbeddedFunctions.Parse(funcText);

                }
            }
        }
    }

  
    public string conditionPath { get; set; }
    [YamlIgnore] public string[] conditionPathToken = null;
    public ComparerV conditionCalcer { get; set; }
    public string valuePath { get; set; } = "";
    [YamlIgnore] public string[] valuePathToken = null;

    public string transformRegularExpression { get; set; } = "";
    Regex regex = null;
    string calcRegEx(string input)
    {
        if (regex == null)
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

    protected override string GetValueSimple(AbstrParser.UniEl? el1)
    {
        var retValue= base.GetValueSimple(el1);
        if (!string.IsNullOrEmpty(transformRegularExpression))
            retValue = calcRegEx(retValue);
        if (!string.IsNullOrEmpty(functionCall))
        {
            if (func != null)
            {
                retValue=func(el1)?.ToString();
            }
            else
            {
                var arr = new List<AbstrParser.UniEl>() { (AbstrParser.UniEl)el1 };

                retValue = EmbeddedFunctions.exec(functionCall, arr).ToString();
            }
        }
        return retValue;

    }
    public override object getValue(AbstrParser.UniEl rootEl)
    {
        object retValue;
        var nod = getNode(rootEl);
        if (!string.IsNullOrEmpty(transformRegularExpression))
            retValue= calcRegEx(nod.Value.ToString());
        else
        retValue=nod.Value;
        if(!string.IsNullOrEmpty(functionCall))
            if (func != null)
            {
                retValue = func(nod)?.ToString();
            }
            else
                retValue = EmbeddedFunctions.exec(functionCall,new List<AbstrParser.UniEl> { nod });
        return retValue;
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

    public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl, ContextItem context)
    {
        if(conditionPath =="Step_0/Rec/Fields/MiscellaneousTransactionAttributes2/Value/AED/Value/Value/Value/BrowserInfo/Value/browserIp/Address")
        {
            int yy = 0;
        }
        if (conditionPathToken == null)
            conditionPathToken = conditionPath.Split("/");

        var rootEl1 = AbstrParser.getLocalRoot(rootEl, conditionPathToken);

        foreach (var item in rootEl1.getAllDescentants(conditionPathToken, rootEl1.rootIndex,context)
                     .Where(ii => ((conditionCalcer == null) ? true : conditionCalcer.Compare(ii))))
        {
            var item1 = item;
            if (valuePath != "")
            {
                if (valuePathToken == null)
                    valuePathToken = valuePath.Split("/");
                item1 = AbstrParser.getLocalRoot(item1, valuePathToken);
                foreach (var item2 in item1.getAllDescentants(valuePathToken, item1.rootIndex,context))
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

            foreach (var item in rootEl1.getAllDescentants(conditionPathToken, rootEl1.rootIndex,null)
                         .Where(ii => ((conditionCalcer == null) ? true : conditionCalcer.Compare(ii))))
            {
                var item1 = item;
                if (valuePath != "")
                {
                    if (valuePathToken == null)
                        valuePathToken = valuePath.Split("/");
                    item1 = AbstrParser.getLocalRoot(item1, valuePathToken);
                    foreach (var item2 in item1.getAllDescentants(valuePathToken, item1.rootIndex,null))
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
        var valFunc = EmbeddedFunctions.exec(Value?.ToString()).ToString();
        if (!string.IsNullOrEmpty(valFunc))
            return valFunc;
        if (Value?.GetType() == typeof(string) && typeConvert != TypeObject.String)
            return ConvertFromType(Value.ToString(), typeConvert);
        return Value;
    }

    public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
    {
        return null;
    }

    public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl, ContextItem context)
    {
        return new AbstrParser.UniEl[] { null };
    }
}