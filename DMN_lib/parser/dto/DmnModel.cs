/******************************************************************
 * File: DmnModel.cs
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
using System.Xml.Serialization;

namespace net.adamec.lib.common.dmn.engine.parser.dto
{
    /// <summary>
    /// Root of DMN definition XML - DMN Model
    /// </summary>
    public class DmnModel
    {
        /// <summary>
        /// List of input data parameters to be used by <see cref="Decisions"/> for DMN Model
        /// </summary>
        [XmlElement("inputData")]
        public List<InputData> InputData { get; set; }

        /// <summary>
        /// List of <see cref="Decision">decisions</see> within DMN Model
        /// </summary>
        [XmlElement("decision")]
        public List<Decision> Decisions { get; set; }



        [XmlRoot(ElementName = "script", Namespace = "http://scripti.com/modeler7654")]
        public class Script
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "extensionElements")]
        public class ExtensionElements
        {
            [XmlElement(ElementName = "script", Namespace = "http://scripti.com/modeler7654")]
            public List<Script> Script { get; set; }
        }

        [XmlElement(ElementName = "extensionElements")]
        public ExtensionElements extensionElements { get; set; }

    }
}
