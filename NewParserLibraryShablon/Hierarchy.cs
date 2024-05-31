/******************************************************************
 * File: Hierarchy.cs
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

using ParserLibrary;
using System.Runtime.CompilerServices;
using UniElLib;

namespace NewParserLibraryShablon
{

    /// <summary>
    /// Record store functionality implementation
    /// </summary>
    public interface IRecordPusher
    {
        bool pushRecord(object[] record);
        bool endRequest();
    }
    /// <summary>
    /// Class implementing extract data from hierarchy structure with call record store handler(IRecordPusher)
    /// </summary>
    public class RecordExtractor
    {

        public enum OnAbsentFieldBehaviour { skipRecord, ignoreAbsent };
        public OnAbsentFieldBehaviour onAbsentFieldBehaviour;
        public IFilter formCondition;
        public List<Extractor> extractors;
        public IRecordPusher pusher;
        public async Task<int> toRecord(AbstrParser.UniEl root)
        {
            int kolRecord = 0;
            foreach (var item in formCondition.filter(root))
            {
                object[] record = new object[extractors.Count];
                bool recordFound = false;
                for (int i = 0; i < extractors.Count; i++)
                {
                    var ext = extractors[i];
                    var el = ext.search.filter(item).FirstOrDefault();
                    if (el != null)
                    {
                        record[i] = ext.extractor.extract(el);
                        recordFound = true;
                    }
                    else
                        if (onAbsentFieldBehaviour == OnAbsentFieldBehaviour.skipRecord)
                        goto skip;

                }
                if (recordFound)
                {
                    pusher.pushRecord(record);
                    kolRecord++;
                }
            skip:;
            }
            if (kolRecord > 0)
                pusher.endRequest();
            return kolRecord;
        }

    }
    public interface IExtractorValue
    {
        object extract(AbstrParser.UniEl el);
    }
    /// <summary>
    /// Extract value of filtered node
    /// </summary>
    public class Extractor
    {
        public IFilter search;
        public IExtractorValue extractor;
    }

    /// <summary>
    /// Base interface  of a filter functionality
    /// </summary>
    public interface IFilter
    {
        IEnumerable<AbstrParser.UniEl> filter(AbstrParser.UniEl el);
    }
    /// <summary>
    /// Group filter functionality
    /// </summary>

    public class GroupFilter : IFilter
    {
        public enum TypeSearch { fromCurrent, fromRoot };
        public TypeSearch typeSearch;
        public enum Operation { and, or };
        public Operation operation;
        public List<IFilter> members;


        public IEnumerable<AbstrParser.UniEl> filter(AbstrParser.UniEl el)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Implementation of a node value check with various conditions (check name, node value, entry into the range of values) in filter
    /// </summary>
    public interface ICheckElementValue
    {

        public bool check(AbstrParser.UniEl el);

    }

    /// <summary>
    /// Implementing filter functionality with path and check value conditions
    /// </summary>
    public class Filter : IFilter
    {
        public Filter(AbstrParser.UniEl rootEl, string path, ICheckElementValue checker)
        {

        }
        //  child filter - if the main filter returns nodes - they are filtered in the child filter
        public IFilter child;


        public IEnumerable<AbstrParser.UniEl> filter(AbstrParser.UniEl el)
        {
            throw new NotImplementedException();
        }
    }

    /*    public interface Sender
        {
            public bool IsAsync
            {
                get;
            }
            public Task<string> Send(string a,object context);

            public Task SendRequest(SemaphoreSlim sem, string, object context);
          //  public Task<(string,object)> 

        }
       */

}
