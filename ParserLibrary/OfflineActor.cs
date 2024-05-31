/******************************************************************
 * File: OfflineActor.cs
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

using RabbitMQ.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public abstract class OfflineActor
    {

        public bool isPresetted = false;

        public static Dictionary<string,OfflineActor> allActors = new Dictionary<string, OfflineActor>();
        public static Dictionary<string,List<string>> ancestors ;

       
//        public static fillAncestors(MXGraphDoc)
        public OfflineActor(string id)
        {
            this.id = id;
            allActors.Add(id, this);


        }
        public  class ContextItem
        {
            //public bool RecordFinished = false;

            ActorContext context;
            public OfflineActor owner;
            public ContextItem(OfflineActor owner,ActorContext context)
            {
                this.owner = owner;
                this.context = context;
             //   RecordFinished = false;
                //context.
            }
            public async  Task exec()
            {
                var res = await owner.execute(this);
                if (res)
                {

                    List<string> readyChilds= new List<string>();
                    //                this.context
                    foreach (var link in owner.outboundLink)
                    {
                        AddOutboundLinkValue(link,readyChilds);
                    }
                   // RecordFinished = true;
                    foreach(var child in readyChilds)
                    {
                        await context.getContext(child).exec();
                    }
                } /*else
                    RecordFinished = true;*/
              /*  foreach (var cont in outboundLink.Select(ii => ii.linkContext).Distinct())
                {
                    if (cont.readyForExec(this))
                        cont.fillOutputLinks();
                }
               */
            }

            private void AddOutboundLinkValue(KeyValuePair<string, (OutLink[] childIds, LinkGetter valueGetter)> link,List<string> readyChilds)
            {
                var getValue = link.Value.valueGetter.getValue(this);
                foreach (var it in link.Value.childIds)
                {
                    OfflineActor.allActors[it.box_id].inboundLink[it.link_id].setValue(getValue, this);
                    if (ancestors[it.box_id].Contains(owner.id))
                    {
                        ancestors[it.box_id].Remove(owner.id);
                        if(ancestors[it.box_id].Count==0)
                            readyChilds.Add(it.box_id);
                    }
                }
            }
        }

        public abstract ContextItem createContext(ActorContext owner);

        public class OutLink
        {
            public OutLink(string fullLink) 
            {
                int pos = fullLink.IndexOf(":");
                box_id = fullLink.Substring(0, pos);
                link_id=fullLink.Substring(pos+1);
            }
            public string box_id;
            public string link_id;
        }

        //link_id desctination,(box_id destination,setter)
        public Dictionary<string, (OutLink[] childIds , LinkGetter valueGetter)> outboundLink = new Dictionary<string, (OutLink[], LinkGetter)> ();
        public Dictionary<string, LinkSetter> inboundLink = new Dictionary<string, LinkSetter>();
        public interface LinkGetter
        {
            IEnumerable<string> getValue(ContextItem context);
        }
        public interface LinkSetter
        {
            void setValue(IEnumerable<string> ret,ContextItem context);
        }


        public abstract  Task<bool> execute(ContextItem context);

        public string id;



    }


    public class ActorContext
    {
        //id box,context
       Dictionary<string,OfflineActor.ContextItem> contextItems = new Dictionary<string, OfflineActor.ContextItem> ();
        public static Dictionary<string, List<string>> ancestors;

        public ActorContext()
        {
            ancestors = OfflineActor.ancestors.ToDictionary(x => x.Key, y => y.Value.ToList());
           // new KeyValuePair(ii.Key,ii.Value));
            foreach (var actor in OfflineActor.allActors.Where(ii => ii.Value.isPresetted))
                getContext(actor.Value).exec().GetAwaiter().GetResult();
        }

        public OfflineActor.ContextItem getContext(OfflineActor actor)
        {
            string id = actor.id;
            return getContext(actor, id);
        }
        public OfflineActor.ContextItem getContext(string id)
        { 
            var actor = OfflineActor.allActors[id];
            return getContext(actor, id); 
        }
         OfflineActor.ContextItem getContext(OfflineActor actor, string id)
        {
            OfflineActor.ContextItem context;
            if (!contextItems.TryGetValue(id, out context))
            {
                context = actor.createContext(this);
                contextItems.Add(id, context);
            }
            return context;
        }
     
    }
}
