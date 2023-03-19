using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CamundaInterfaces
{

    public class Test
    {
		public static void test()
        {
			XmlSerializer serializer = new XmlSerializer(typeof(definitions));
			definitions test = new definitions();
            CamundaProcess process = new CamundaProcess();
			using (StreamReader reader = new StreamReader(@"C:\Camunda\Example.bpmn"))
			 {
			    test = (definitions)serializer.Deserialize(reader);
			}
            test.setCamundaProcess(process);

            using (StreamWriter sw = new StreamWriter(@"C:\Camunda\Example1.bpmn"))
			{
				serializer.Serialize(sw,test);
			}


		}
	}





    public class CamundaProcess
    {
        public string documentation;
        public void  save(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(definitions));
            definitions test = new definitions();
            CamundaProcess process = new CamundaProcess();
            using (StreamReader reader = new StreamReader(@"C:\Camunda\Example.bpmn"))
            {
                test = (definitions)serializer.Deserialize(reader);
            }

            test.setCamundaProcess(this);

            using (StreamWriter sw = new StreamWriter(path))
            {
                serializer.Serialize(sw, test);
            }

        }
        public string DefinitionID { get; set; } = "DefinitionTest";
        public string ProcessID { get; set; } = "CamundaTest";
        public string ProcessName { get; set; } = "It's camunda example";
        public string versionTag { get; set; } = "1.0.0";
        public List<ExternalTask> tasks { get; set; } = new List<ExternalTask>() { new ExternalTask() { id="Id1",name="Name1"}, new ExternalTask() { id = "Id2", name = "Name2" }, new ExternalTask() { id = "Id3", name = "Name3" }, new ExternalTask() { id = "Id4", name = "Name4" } };

        public class ExternalTask
        {
            public string Annotation = "It's example annotation";
            public class Parameter
            {
                public string Name { get; set; }
                public List<string> listValues = null;
                public string Value = "";
                public string description;
                public Parameter(string name, List<string> listValues, string description="")
                {
                    Name = name;
                    this.listValues = listValues;
                    this.description = description;
                }
                public Parameter(string name, string value, string description = "")
                {
                    Name = name;
                    Value = value;
                    this.description = description;
                }
            }
            public List<Parameter> parameters { get; set; } = new List<Parameter>() { new Parameter("PAN","457"), new Parameter("Val",new List<string>() { "aa","bb"})};
            public string id;
            public string name;
            public string topic="PythonExporter";
            public string description;
            public string author;
            public string service_location;
        }



    }



    public partial class definitions
    {

        public void setCamundaProcess(CamundaProcess proc)
        {
            id = proc.DefinitionID;
            process = new definitionsProcess() { id = proc.ProcessID, name = proc.ProcessName, versionTag = proc.versionTag, Items = getItems(proc) };
            process.documentation = proc.documentation;

            this.BPMNDiagram = new BPMNDiagram() { id = "BPMNDiagram_1", BPMNPlane = new BPMNDiagramBPMNPlane() { id = "BPMNPlane_1", bpmnElement = proc.ProcessID, BPMNShape = getShapes(proc), BPMNEdge = getEdges(proc) } };

        }

        string getIDEl(int index)
        {
            return "Item_" + index;
        }
        BPMNDiagramBPMNPlaneBPMNShape[] getShapes(CamundaProcess proc)
        {
            List<BPMNDiagramBPMNPlaneBPMNShape> retValue = new List<BPMNDiagramBPMNPlaneBPMNShape>();
            retValue.Add(new BPMNDiagramBPMNPlaneBPMNShape() { id = "Shape_0", bpmnElement = startID, Bounds = new Bounds() { x = startX, y = startY - radiusCircle, width = radiusCircle * 2, height = radiusCircle * 2 } });
            for (int i = 0; i < proc.tasks.Count; i++)
            {
                retValue.Add(new BPMNDiagramBPMNPlaneBPMNShape() { id = $"Shape_{i + 1}", bpmnElement = proc.tasks[i].id, Bounds = new Bounds() { x = calcX(i), y = startY - rectangleHeight / 2, width = rectangleWidth, height = rectangleHeight } });
                if (proc.tasks[i].Annotation != "")
                {
                    retValue.Add(new BPMNDiagramBPMNPlaneBPMNShape() { id = $"ShapeAnnotation_{i + 1}", bpmnElement = $"Annotation_{i}", Bounds= new Bounds() {  x=calcX(i),y= startY + rectangleWidth/2 + annotationIndent , height= calcAnnotationHeight(proc.tasks[i].Annotation), width=rectangleWidth} });
                }
            }
            retValue.Add(new BPMNDiagramBPMNPlaneBPMNShape() { id = "Shape_End", bpmnElement = EndID, Bounds = new Bounds() { x = calcX(proc.tasks.Count), y = startY - radiusCircle, width = radiusCircle * 2, height = radiusCircle * 2 } });
            return retValue.ToArray();
        }

        int calcAnnotationHeight(string annotation)
        {
            int len = annotation.Length;
            if (len < 5)
                return 20;
            return (int) (len * 1.5);
        }

        private static ushort calcX(int i)
        {
            return (ushort)(startX + radiusCircle * 2 + shapeLength + (rectangleWidth + shapeLength) * i);
        }

        BPMNDiagramBPMNPlaneBPMNEdge[] getEdges(CamundaProcess proc)
        {
            List<BPMNDiagramBPMNPlaneBPMNEdge> retValue = new List<BPMNDiagramBPMNPlaneBPMNEdge>();


            retValue.Add(new BPMNDiagramBPMNPlaneBPMNEdge() { id = "Edge_0", bpmnElement = getIDEl(0), waypoint = new waypoint[] { new waypoint() { x = startX + radiusCircle * 2, y = startY }, new waypoint() { x = startX + radiusCircle * 2 + shapeLength, y = startY } } });
            for (int i = 0; i < proc.tasks.Count; i++)
            {
                retValue.Add(new BPMNDiagramBPMNPlaneBPMNEdge() { id = $"Edge_{i + 1}", bpmnElement = getIDEl(i+1), waypoint = new waypoint[] { new waypoint() { x = (ushort)(calcX(i) + rectangleWidth), y = startY }, new waypoint() { x =(ushort)(calcX(i)+rectangleWidth+shapeLength), y = startY } } });
                if (proc.tasks[i].Annotation !="")
                {
                    retValue.Add(new BPMNDiagramBPMNPlaneBPMNEdge() { id = $"EdgeAssociation_{i + 1}", bpmnElement = $"Association_{i}", waypoint = new waypoint[] { new waypoint() { x = (ushort)(calcX(i)+rectangleWidth/2) , y = startY+ rectangleHeight/2 }, new waypoint() { x = (ushort)(calcX(i) + rectangleWidth / 2), y =(ushort)( startY +  rectangleHeight/2 + annotationIndent)} } });
                }

            }
            return retValue.ToArray();
        }

        const string startID = "Start_1";
        const string EndID = "End_1";
        const int startX = 170;
        const int startY = 100;
        const int radiusCircle = 18;
        const int rectangleWidth = 100;
        const int rectangleHeight = 80;
        const int shapeLength = 50;
        const int annotationIndent = 50;
        const int annotationHeight = 50;
//        const int annotationHeight = 50;

        object[] getItems(CamundaProcess proc)
        {
            List<object> items = new List<object>();
            List<object> add_items = new List<object>();
            items.Add(new definitionsProcessStartEvent() { id = startID, outgoing = getIDEl(0) });
            items.Add(new definitionsProcessSequenceFlow() { id = getIDEl(0), targetRef = proc.tasks.First().id, sourceRef = startID });
            for (int i = 0; i < proc.tasks.Count; i++)
            {
                var item7 = new definitionsProcessServiceTask() { id = proc.tasks[i].id, outgoing = getIDEl(i + 1), incoming = getIDEl(i), name = proc.tasks[i].name, type = "external", topic = proc.tasks[i].topic };
                if (proc.tasks[i].parameters.Count > 0)
                {

                    item7.extensionElements = new definitionsProcessServiceTaskExtensionElements() { inputOutput = proc.tasks[i].parameters.Select(ii => new inputOutputInputParameter() { name = ii.Name, Text= ((ii.listValues == null) ? ii.Value : ""), list = ((ii.listValues == null) ? null : ii.listValues.ToArray()) }).ToArray() };
                }
                if (proc.tasks[i].Annotation!= "")
                {
                    add_items.Add(new definitionsProcessTextAnnotation() { id=$"Annotation_{i}", text = proc.tasks[i].Annotation });
                    add_items.Add(new definitionsProcessAssociation() { id = $"Association_{i}" , sourceRef = proc.tasks[i].id, targetRef= $"Annotation_{i}" });
                }
                items.Add(item7);
                if (i < proc.tasks.Count - 1)
                    items.Add(new definitionsProcessSequenceFlow() { id = getIDEl(i + 1), targetRef = proc.tasks[i + 1].id, sourceRef = proc.tasks[i].id });
                else
                    items.Add(new definitionsProcessSequenceFlow() { id = getIDEl(i + 1), targetRef = EndID, sourceRef = proc.tasks[i].id });
            }

            items.Add(new definitionsProcessEndEvent() { id = EndID, incoming = getIDEl(proc.tasks.Count) });
            items.AddRange(add_items);
            return items.ToArray();
        }

    }

    /*

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public partial class definitions1
    {
        public void setCamundaProcess(CamundaProcess proc)
        {
            id = proc.DefinitionID;
            process = new definitionsProcess() {  id=proc.ProcessID, name=proc.ProcessName, versionTag=proc.versionTag, Items=getItems(proc) };
            this.BPMNDiagram = new BPMNDiagram() { id = "BPMNDiagram_1", BPMNPlane = new BPMNDiagramBPMNPlane() { id= "BPMNPlane_1", bpmnElement= proc.ProcessID, BPMNShape = getShapes(proc), BPMNEdge= getEdges(proc) } };
           
        }

        string getIDEl(int index)
        {
            return "Item_" + index;
        }
        BPMNDiagramBPMNPlaneBPMNShape[] getShapes(CamundaProcess proc)
        {
            List<BPMNDiagramBPMNPlaneBPMNShape> retValue = new List<BPMNDiagramBPMNPlaneBPMNShape>();
            retValue.Add(new BPMNDiagramBPMNPlaneBPMNShape() { id="Shape_0", bpmnElement=startID, Bounds= new Bounds() { x=startX, y=startY - radiusCircle, width=radiusCircle*2, height=radiusCircle*2 } });
            for(int i=0; i < proc.tasks.Count; i++)
            {
                retValue.Add(new BPMNDiagramBPMNPlaneBPMNShape() { id = $"Shape_{i}", bpmnElement = proc.tasks[i].id, Bounds = new Bounds() { x = (ushort)(startX + radiusCircle * 2 + (rectangleWidth + shapeLength) * i), y = startY - rectangleHeight/2, width = rectangleWidth, height = rectangleHeight } });
            }
            retValue.Add(new BPMNDiagramBPMNPlaneBPMNShape() { id = "Shape_End", bpmnElement = EndID, Bounds = new Bounds() { x =(ushort) (startX+radiusCircle*2+(rectangleWidth+shapeLength)*proc.tasks.Count) , y = startY - radiusCircle, width = radiusCircle * 2, height = radiusCircle * 2 } });
            return retValue.ToArray();
        }

        BPMNDiagramBPMNPlaneBPMNEdge[] getEdges(CamundaProcess proc)
        {
            List<BPMNDiagramBPMNPlaneBPMNEdge> retValue = new List<BPMNDiagramBPMNPlaneBPMNEdge>();


            retValue.Add(new BPMNDiagramBPMNPlaneBPMNEdge() { id = "Edge_0", bpmnElement = startID, waypoint= new waypoint[] { new waypoint() { x=startX+radiusCircle*2, y=startY }, new waypoint() { x= startX + radiusCircle * 2+ shapeLength, y=startY } } });
            for(int i=1; i < proc.tasks.Count;i++)
            {
                retValue.Add(new BPMNDiagramBPMNPlaneBPMNEdge() { id = $"Edge_{i + 1}", bpmnElement = (i < proc.tasks.Count - 1)?startID:EndID, waypoint = new waypoint[] { new waypoint() { x =(ushort) (startX + radiusCircle * 2+i*(rectangleWidth+shapeLength)), y = startY }, new waypoint() { x = (ushort)(startX + radiusCircle * 2 + i * (rectangleWidth + shapeLength) + shapeLength), y = startY } } });
            }
            return retValue.ToArray();
        }

        const string startID = "Start_1";
        const string EndID = "End_1";
        const int startX = 170;
        const int startY = 100;
        const int radiusCircle = 18;
        const int rectangleWidth = 100;
        const int rectangleHeight = 80;
        const int shapeLength = 50;

        object[] getItems(CamundaProcess proc)
        {
            List<object> items = new List<object>();
            items.Add(new definitionsProcessStartEvent() {  id=startID, outgoing= getIDEl(0) });
            items.Add(new definitionsProcessSequenceFlow() { id = getIDEl(0), targetRef = proc.tasks.First().id, sourceRef = startID });
            for(int i=0; i < proc.tasks.Count;i++)
            {
                items.Add(new definitionsProcessServiceTask() { id = proc.tasks[i].id, outgoing = getIDEl(i+1),incoming=getIDEl(i), name= proc.tasks[i].name, type= "external", topic = proc.tasks[i].topic });
                if(i< proc.tasks.Count-1)
                    items.Add(new definitionsProcessSequenceFlow() { id = getIDEl(i+1), targetRef = proc.tasks[i+1].id, sourceRef = proc.tasks[i].id });
                else
                    items.Add(new definitionsProcessSequenceFlow() { id = getIDEl(i + 1), targetRef = EndID, sourceRef = proc.tasks[i].id });
            }

            items.Add(new definitionsProcessEndEvent() { id = EndID, incoming = getIDEl(proc.tasks.Count) });

            return items.ToArray();
        }


//<bpmn:definitions xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:camunda="http://camunda.org/schema/1.0/bpmn" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:modeler="http://camunda.org/schema/modeler/1.0" id="Definitions_0ssama7" targetNamespace="http://bpmn.io/schema/bpmn" exporter="Camunda Modeler" exporterVersion="4.11.1" modeler:executionPlatform="Camunda Platform" modeler:executionPlatformVersion="7.15.0">


        private BPMNDiagram bPMNDiagramField= new BPMNDiagram();




        /// <remarks/>
        public definitionsProcess process;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
        public BPMNDiagram BPMNDiagram;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string targetNamespace = "http://bpmn.io/schema/bpmn";

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string exporter = "Camunda Modeler";

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string exporterVersion = "4.11.1";

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://camunda.org/schema/modeler/1.0")]
        public string executionPlatform = "Camunda Platform";

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://camunda.org/schema/modeler/1.0")]
        public string executionPlatformVersion = "7.15.0";
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public partial class definitionsProcess
    {
       


        private bool isExecutableField=true;


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("endEvent", typeof(definitionsProcessEndEvent))]
        [System.Xml.Serialization.XmlElementAttribute("sequenceFlow", typeof(definitionsProcessSequenceFlow))]
        [System.Xml.Serialization.XmlElementAttribute("serviceTask", typeof(definitionsProcessServiceTask))]
        [System.Xml.Serialization.XmlElementAttribute("startEvent", typeof(definitionsProcessStartEvent))]
        public object[] Items;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;

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
        public string versionTag;
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

        private string[] textField;

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
        public string[] Text
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

        /// <remarks/>
        public string outgoing;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id;
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

        private BPMNDiagramBPMNPlaneBPMNEdge[] bPMNEdgeField;

        private BPMNDiagramBPMNPlaneBPMNShape[] bPMNShapeField;

        private string idField;

        private string bpmnElementField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BPMNEdge")]
        public BPMNDiagramBPMNPlaneBPMNEdge[] BPMNEdge
        {
            get
            {
                return this.bPMNEdgeField;
            }
            set
            {
                this.bPMNEdgeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BPMNShape")]
        public BPMNDiagramBPMNPlaneBPMNShape[] BPMNShape
        {
            get
            {
                return this.bPMNShapeField;
            }
            set
            {
                this.bPMNShapeField = value;
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

        private byte yField;

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

        private byte widthField;

        private byte heightField;

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
        public byte width
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
        public byte height
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

*/


}
