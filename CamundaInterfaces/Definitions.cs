using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaInterfaces
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public partial class definitions
    {


        private definitionsProcess processField;

        private BPMNDiagram bPMNDiagramField;

        private string idField;

        private string targetNamespaceField=  "http://bpmn.io/schema/bpmn";

        private string exporterField;

        private string exporterVersionField;

        private string executionPlatformField;

        private string executionPlatformVersionField;

        /// <remarks/>
        public definitionsProcess process
        {
            get
            {
                return this.processField;
            }
            set
            {
                this.processField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
        public BPMNDiagram BPMNDiagram
        {
            get
            {
                return this.bPMNDiagramField;
            }
            set
            {
                this.bPMNDiagramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string targetNamespace
        {
            get
            {
                return this.targetNamespaceField;
            }
            set
            {
                this.targetNamespaceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string exporter
        {
            get
            {
                return this.exporterField;
            }
            set
            {
                this.exporterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string exporterVersion
        {
            get
            {
                return this.exporterVersionField;
            }
            set
            {
                this.exporterVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://camunda.org/schema/modeler/1.0")]
        public string executionPlatform
        {
            get
            {
                return this.executionPlatformField;
            }
            set
            {
                this.executionPlatformField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://camunda.org/schema/modeler/1.0")]
        public string executionPlatformVersion
        {
            get
            {
                return this.executionPlatformVersionField;
            }
            set
            {
                this.executionPlatformVersionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public partial class definitionsProcess
    {

        [System.Xml.Serialization.XmlElementAttribute()]
        //"http://www.omg.org/spec/BPMN/20100524/DI"
        public string documentation { get; set; }


        private object[] itemsField;

        private string idField;

        private string nameField;

        private bool isExecutableField=true;

        private string versionTagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("association", typeof(definitionsProcessAssociation))]
        [System.Xml.Serialization.XmlElementAttribute("endEvent", typeof(definitionsProcessEndEvent))]
        [System.Xml.Serialization.XmlElementAttribute("sequenceFlow", typeof(definitionsProcessSequenceFlow))]
        [System.Xml.Serialization.XmlElementAttribute("serviceTask", typeof(definitionsProcessServiceTask))]
        [System.Xml.Serialization.XmlElementAttribute("startEvent", typeof(definitionsProcessStartEvent))]
        [System.Xml.Serialization.XmlElementAttribute("textAnnotation", typeof(definitionsProcessTextAnnotation))]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExecutable
        {
            get
            {
                return this.isExecutableField;
            }
            set
            {
                this.isExecutableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://camunda.org/schema/1.0/bpmn")]
        public string versionTag
        {
            get
            {
                return this.versionTagField;
            }
            set
            {
                this.versionTagField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public partial class definitionsProcessAssociation
    {

        private string idField;

        private string sourceRefField;

        private string targetRefField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sourceRef
        {
            get
            {
                return this.sourceRefField;
            }
            set
            {
                this.sourceRefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string targetRef
        {
            get
            {
                return this.targetRefField;
            }
            set
            {
                this.targetRefField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public partial class definitionsProcessEndEvent
    {

        private string incomingField;

        private string idField;

        /// <remarks/>
        public string incoming
        {
            get
            {
                return this.incomingField;
            }
            set
            {
                this.incomingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public partial class definitionsProcessSequenceFlow
    {

        private string idField;

        private string sourceRefField;

        private string targetRefField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sourceRef
        {
            get
            {
                return this.sourceRefField;
            }
            set
            {
                this.sourceRefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string targetRef
        {
            get
            {
                return this.targetRefField;
            }
            set
            {
                this.targetRefField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public partial class definitionsProcessServiceTask
    {

        private definitionsProcessServiceTaskExtensionElements extensionElementsField;

        private string incomingField;

        private string outgoingField;

        private string idField;

        private string nameField;

        private string typeField;

        private string topicField;

        /// <remarks/>
        public definitionsProcessServiceTaskExtensionElements extensionElements
        {
            get
            {
                return this.extensionElementsField;
            }
            set
            {
                this.extensionElementsField = value;
            }
        }

        /// <remarks/>
        public string incoming
        {
            get
            {
                return this.incomingField;
            }
            set
            {
                this.incomingField = value;
            }
        }

        /// <remarks/>
        public string outgoing
        {
            get
            {
                return this.outgoingField;
            }
            set
            {
                this.outgoingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://camunda.org/schema/1.0/bpmn")]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://camunda.org/schema/1.0/bpmn")]
        public string topic
        {
            get
            {
                return this.topicField;
            }
            set
            {
                this.topicField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public partial class definitionsProcessServiceTaskExtensionElements
    {

        private inputOutputInputParameter[] inputOutputField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://camunda.org/schema/1.0/bpmn")]
        [System.Xml.Serialization.XmlArrayItemAttribute("inputParameter", IsNullable = false)]
        public inputOutputInputParameter[] inputOutput
        {
            get
            {
                return this.inputOutputField;
            }
            set
            {
                this.inputOutputField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://camunda.org/schema/1.0/bpmn")]
    public partial class inputOutputInputParameter
    {

        private string[] listField;

        private string textField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("value", IsNullable = false)]
        public string[] list
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public partial class definitionsProcessStartEvent
    {

        private string outgoingField;

        private string idField;

        /// <remarks/>
        public string outgoing
        {
            get
            {
                return this.outgoingField;
            }
            set
            {
                this.outgoingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public partial class definitionsProcessTextAnnotation
    {

        private string textField;

        private string idField;

        /// <remarks/>
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.omg.org/spec/BPMN/20100524/DI", IsNullable = false)]
    public partial class BPMNDiagram
    {

        private BPMNDiagramBPMNPlane bPMNPlaneField;

        private string idField;

        /// <remarks/>
        public BPMNDiagramBPMNPlane BPMNPlane
        {
            get
            {
                return this.bPMNPlaneField;
            }
            set
            {
                this.bPMNPlaneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    public partial class BPMNDiagramBPMNPlane
    {

        private object[] itemsField;

        private string idField;

        private string bpmnElementField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BPMNEdge")]
        public BPMNDiagramBPMNPlaneBPMNEdge[] BPMNEdge;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BPMNShape")]
        public BPMNDiagramBPMNPlaneBPMNShape[] BPMNShape;
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bpmnElement
        {
            get
            {
                return this.bpmnElementField;
            }
            set
            {
                this.bpmnElementField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    public partial class BPMNDiagramBPMNPlaneBPMNEdge
    {

        private waypoint[] waypointField;

        private string idField;

        private string bpmnElementField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("waypoint", Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
        public waypoint[] waypoint
        {
            get
            {
                return this.waypointField;
            }
            set
            {
                this.waypointField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bpmnElement
        {
            get
            {
                return this.bpmnElementField;
            }
            set
            {
                this.bpmnElementField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
    public partial class waypoint
    {

        private ushort xField;

        private ushort yField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    public partial class BPMNDiagramBPMNPlaneBPMNShape
    {

        private Bounds boundsField;

        private string idField;

        private string bpmnElementField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.omg.org/spec/DD/20100524/DC")]
        public Bounds Bounds
        {
            get
            {
                return this.boundsField;
            }
            set
            {
                this.boundsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bpmnElement
        {
            get
            {
                return this.bpmnElementField;
            }
            set
            {
                this.bpmnElementField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/DD/20100524/DC")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.omg.org/spec/DD/20100524/DC", IsNullable = false)]
    public partial class Bounds
    {

        private ushort xField;

        private byte yField;

        private decimal widthField;

        private int heightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://camunda.org/schema/1.0/bpmn")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://camunda.org/schema/1.0/bpmn", IsNullable = false)]
    public partial class inputOutput
    {

        private inputOutputInputParameter[] inputParameterField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inputParameter")]
        public inputOutputInputParameter[] inputParameter
        {
            get
            {
                return this.inputParameterField;
            }
            set
            {
                this.inputParameterField = value;
            }
        }
    }


}
