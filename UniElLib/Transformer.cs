/******************************************************************
 * File: Transformer.cs
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using CSScriptLib;
using DotLiquid.Util;
using static System.Net.Mime.MediaTypeNames;

namespace UniElLib
{
    public abstract  class Transformer
    {
        public string[] Inputs;
        public string[] Outputs;
        public Transformer(string body)
        {
        }

        protected void Init(string body)
        {
            var pars = Prepare(body);
            Inputs = pars.inputParameters;
            Outputs = pars.outputParameters;
        }

        protected abstract (string[] inputParameters, string[] outputParameters) Prepare(string body);
        public abstract string[] transform(string[] args);
    }

    public class RegexpTransformer : Transformer
    {
        Regex regex;

        public RegexpTransformer(string regularPattern= @"=(\d{2})(\d{2})"/* for Track2 parser*/) :base(regularPattern) 
        {
            Init(regularPattern);
        }
        public override string[] transform(string[] args)
        {
            Match match = regex.Match(args[0]);

            if (match.Success)
            {
                return match.Groups.Values.Select(ii => ii.Value).ToArray();//.Select).G  
            }
            else
            {
                return null;
            }
        }

        protected override (string[] inputParameters, string[] outputParameters) Prepare(string body)
        {
            regex = new Regex(body);
            var outputs = regex.GetGroupNames().Select(ii=> "Out" +ii).ToArray();
            return (new string[] { "inputString" },outputs);
        }
    }

    public class CSScriptTransformer : Transformer
    {
        bool isDouble;
        public CSScriptTransformer(string expression = "@P0/1000",bool isDouble =false) : base(expression) 
        {
            this.isDouble = isDouble;
            Init(expression);

        }
        public override string[] transform(string[] args)
        {
            return checker(new object[] { args });
        }
        MethodDelegate<string[]> checker;
        protected override (string[] inputParameters, string[] outputParameters) Prepare(string expression)
        {
            // Define the regular expression pattern
            string pattern = @"@Input(\d+)";

            // Replace using regular expression
            List<string> outs = new List<string>();
            string result = Regex.Replace(expression, pattern, isDouble?("Convert.ToDouble(params1[$1])"):"params1[$1]");
            Regex r = new Regex(pattern);
            Match m = r.Match(expression);
            while (m.Success)
            {
                Group g = m.Groups[1];
                outs.Add(g.Value);
                m = m.NextMatch();
            }

            var body = @"using System;
using System.Text;
using System.Linq;
string[]  ConvObject(string[] params1)
{                           
    return new string[] {(" + result + @").ToString() };
}
";
            checker = CSScript.RoslynEvaluator.CreateDelegate<string[]>(body);
            //            checker(new object[] { new string[] { "1000" } });
            int kolGroups = outs.Max(ii => Convert.ToInt32(ii)) + 1;
            return (Enumerable.Range(0,kolGroups).Select(ii => "Input" + ii).ToArray(), new string[] { "OutValue" });
        }
    }

}
