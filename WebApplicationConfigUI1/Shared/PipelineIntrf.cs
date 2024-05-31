/******************************************************************
 * File: PipelineIntrf.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationConfigUI1.Shared
{
    public class PipelineIntrf
    {
        public string pipelineDescription { get; set; } = "Pipeline example";
        public StepIntrf[] steps { get; set; } = new StepIntrf[] { };// new Step[] { new Step() };
        //public AbstrParser.UniEl rootElement = null;

       

    }
    public class StepIntrf
    {

    
        public string IDStep { get; set; } = "Example";


        public string IDPreviousStep { get; set; } = "";

        public string IDResponsedReceiverStep { get; set; } = "";


        public string description { get; set; } = "Some comments in this place";

//        public Receiver receiver { get; set; } = null;// new PacketBeatReceiver();
   /*     public class ItemFilter
        {
            public override string ToString()
            {
                return "filter";
            }
            public Filter filter { get; set; } = new ConditionFilter();
            public List<OutputValue> outputFields { get; set; } = new List<OutputValue> { new ConstantValue() { outputPath = "stream", Value = "CheckRegistration" }, new ExtractFromInputValue() { outputPath = "IP", conditionPath = "aa/bb/cc", conditionCalcer = new ComparerForValue() { value_for_compare = "tutu" }, valuePath = "cc/dd" } };
            public int exec(AbstrParser.UniEl rootElInput, ref AbstrParser.UniEl local_rootOutput)
            {
                int count = 0;
                if (local_rootOutput == null)
                    local_rootOutput = new AbstrParser.UniEl() { Name = "root" };

                foreach (var ff in outputFields)
                {
                    if (ff.addToOutput(rootElInput, ref local_rootOutput))
                        count++;
                }
                return count;
            }

        }
        public List<ItemFilter> converters { get; set; } = new List<ItemFilter>() { };
        public Sender sender { get; set; } = new LongLifeRepositorySender();
        public Pipeline owner { get; set; }

        public class ContextItem
        {
            public List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            public object context;
        }
   */
    }
}
