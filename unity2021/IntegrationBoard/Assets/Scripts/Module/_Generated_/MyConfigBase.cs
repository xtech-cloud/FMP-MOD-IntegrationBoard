
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.70.0.  DO NOT EDIT!
//*************************************************************************************

using System.Xml.Serialization;

namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    public class MyConfigBase
    {
        public class GRPC
        {
            [XmlAttribute("address")]
            public string address { get; set; } = "";
        }

        public class Anchor
        {
            [XmlAttribute("horizontal")]
            public string horizontal { get; set; } = "center";
            [XmlAttribute("vertical")]
            public string vertical { get; set; } = "center";
            [XmlAttribute("marginH")]
            public string marginH { get; set; } = "0";
            [XmlAttribute("marginV")]
            public string marginV { get; set; } = "0";
            [XmlAttribute("width")]
            public int width { get; set; } = 0;
            [XmlAttribute("height")]
            public int height { get; set; } = 0;
        }

        public class UiElement
        {
            [XmlAttribute("image")]
            public string image { get; set; } = "";

            [XmlElement("Anchor")]
            public Anchor anchor { get; set; } = new Anchor();
        }

        public class UI
        {
            [XmlAttribute("visible")]
            public bool visible { get; set; } = false;

            [XmlAttribute("slot")]
            public string slot { get; set; } = "[root]";
        }

        public class World
        {
            [XmlAttribute("visible")]
            public bool visible { get; set; } = false;

            [XmlAttribute("slot")]
            public string slot { get; set; } = "[root]";
        }

        public class Instance
        {
            [XmlAttribute("uid")]
            public string uid { get; set; } = "";
            [XmlAttribute("style")]
            public string style { get; set; } = "";
            [XmlAttribute("uiSlot")]
            public string uiSlot { get; set; } = "";
            [XmlAttribute("worldSlot")]
            public string worldSlot { get; set; } = "";
        }

        public class Parameter
        {
            [XmlAttribute("key")]
            public string key { get; set; } = "";
            [XmlAttribute("value")]
            public string value { get; set; } = "";
            [XmlAttribute("type")]
            public string type { get; set; } = "";
        }


        public class Subject
        {
            [XmlAttribute("message")]
            public string message { get; set; } = "";
            [XmlArray("Parameters"), XmlArrayItem("Parameter")]
            public Parameter[] parameters { get; set; } = new Parameter[0];
        }

        public class Preload
        {
            [XmlArray("Subjects"), XmlArrayItem("Subject")]
            public Subject[] subjects { get; set; } = new Subject[0];
        }

        [XmlAttribute("version")]
        public string version { get; set; } = "";

        [XmlElement("GRPC")]
        public GRPC grpc { get; set; } = new GRPC();

        [XmlElement("UI")]
        public UI ui { get; set; } = new UI();

        [XmlElement("World")]
        public World world { get; set; } = new World();

        [XmlArray("Instances"), XmlArrayItem("Instance")]
        public Instance[] instances { get; set; } = new Instance[0];
        [XmlElement("Preload")]
        public Preload preload { get; set; } = new Preload();
    }
}

