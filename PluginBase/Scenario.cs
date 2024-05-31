/******************************************************************
 * File: Scenario.cs
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

namespace PluginBase;

/// <summary>
///  Scenario item for Pipeline
/// </summary>
public  class Scenario
{
    public string Description { get; set; }
    public List<Item> mocs { get; set; }=new List<Item>();
    public override string ToString()
    {
        return Description;
    }
    public Item getStepItem(string idStep)
    {
        var stepItem = mocs.FirstOrDefault(ii => ii.IDStep == idStep);
        if (stepItem == null) 
        {
            stepItem = new Item() { IDStep = idStep };
            mocs.Add(stepItem);
        }
        return stepItem;
    }
    public class Item
    { 
        public string IDStep { get; set; }
//            public bool isMocReceiverEnabled { get; set; }
        public string MocFileReceiver { get; set; }
//            public bool isMocSenderEnabled { get; set; }
        public string MocFileResponce { get; set; }
    }
}
