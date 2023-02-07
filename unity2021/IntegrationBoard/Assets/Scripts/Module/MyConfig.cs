
using Grpc.Core;
using System.Xml.Serialization;

namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class MyConfig : MyConfigBase
    {
        public class AlignGrid
        {
            [XmlAttribute("row")]
            public int row { get; set; } = 0;
            [XmlAttribute("column")]
            public int column { get; set; } = 0;
            [XmlAttribute("stickH")]
            public bool stickH { get; set; } = false;
            [XmlAttribute("stickV")]
            public bool stickV { get; set; } = false;
        }

        public class Border
        {
            [XmlAttribute("top")]
            public int top { get; set; } = 0;
            [XmlAttribute("bottom")]
            public int bottom { get; set; } = 0;
            [XmlAttribute("left")]
            public int left { get; set; } = 0;
            [XmlAttribute("right")]
            public int right { get; set; } = 0;
        }

        public class VisualUiElement : UiElement
        {
            [XmlAttribute("color")]
            public string color = "";

            [XmlElement("Border")]
            public Border border { get; set; } = new Border();
        }

        public class PageSlot
        {
            [XmlAttribute("page")]
            public string page { get; set; } = "";
            [XmlElement("InlaySubject")]
            public Subject inlaySubject { get; set; } = new Subject();
            [XmlElement("RefreshSubject")]
            public Subject refreshSubject { get; set; } = new Subject();
        }

        public class TabButton
        {
            [XmlAttribute("name")]
            public string name { get; set; } = "";
            [XmlAttribute("contentKey")]
            public string contentKey { get; set; } = "";
            [XmlAttribute("checkedIcon")]
            public string checkedIcon { get; set; } = "";
            [XmlAttribute("checkedWidth")]
            public int checkedWidth { get; set; } = 0;
            [XmlAttribute("checkedHeight")]
            public int checkedHeight { get; set; } = 0;
            [XmlAttribute("uncheckedIcon")]
            public string uncheckedIcon { get; set; } = "";
            [XmlAttribute("uncheckedWidth")]
            public int uncheckedWidth { get; set; } = 0;
            [XmlAttribute("uncheckedHeight")]
            public int uncheckedHeight { get; set; } = 0;
            [XmlArray("Subjects"), XmlArrayItem("Subject")]
            public Subject[] subjects { get; set; } = new Subject[0];
        }

        public class CloseButton : UiElement
        {
            [XmlArray("Subjects"), XmlArrayItem("Subject")]
            public Subject[] subjects { get; set; } = new Subject[0];
        }

        public class TabBar
        {
            [XmlAttribute]
            public int space { get; set; } = 0;
            [XmlElement("Background")]
            public VisualUiElement background { get; set; } = new VisualUiElement();
            [XmlElement("CloseButton")]
            public CloseButton closeButton { get; set; } = new CloseButton();
            [XmlArray("TabButtons"), XmlArrayItem("TabButton")]
            public TabButton[] tabButtons { get; set; } = new TabButton[0];
        }

        public class ImageToolBox : VisualUiElement
        {
            [XmlElement("ZoomInButton")]
            public UiElement btnZoomIn { get; set; } = new UiElement();
            [XmlElement("ZoomOutButton")]
            public UiElement btnZoomOut { get; set; } = new UiElement();
            [XmlElement("BackButton")]
            public UiElement btnBack { get; set; } = new UiElement();
        }

        public class CoverPicture
        {
            [XmlAttribute("fitmode")]
            public string fitmode { get; set; } = "none";
            [XmlAttribute("maxZoomIn")]
            public float maxZoomIn { get; set; } = 0f;
        }

        public class SizeRange
        {
            [XmlAttribute("descriptionMaxHeight")]
            public int descriptionMaxHeight { get; set; } = 0;
        }

        public class Style
        {
            [XmlAttribute("name")]
            public string name { get; set; } = "";
            [XmlAttribute("width")]
            public int width { get; set; } = 0;
            [XmlAttribute("height")]
            public int height { get; set; } = 0;
            [XmlElement("AlignGrid")]
            public AlignGrid alignGrid { get; set; } = new AlignGrid();
            [XmlElement("TitleBarBackground")]
            public VisualUiElement titleBarBackground { get; set; } = new VisualUiElement();
            [XmlElement("TopicBackground")]
            public VisualUiElement topicBackground { get; set; } = new VisualUiElement();
            [XmlElement("DescriptionBackground")]
            public VisualUiElement descriptionBackground { get; set; } = new VisualUiElement();
            [XmlElement("PanelBackground")]
            public VisualUiElement panelBackground { get; set; } = new VisualUiElement();
            [XmlElement("LikeBackground")]
            public VisualUiElement likeBackground { get; set; } = new VisualUiElement();
            [XmlElement("ToolBoxBackground")]
            public VisualUiElement toolBoxBackground { get; set; } = new VisualUiElement();
            [XmlElement("TopicSwitchIcon")]
            public UiElement topicSwitchIcon { get; set; } = new UiElement();
            [XmlElement("DescriptionSwitchIcon")]
            public UiElement descriptionSwitchIcon { get; set; } = new UiElement();
            [XmlElement("LikedIcon")]
            public UiElement likedIcon { get; set; } = new UiElement();
            [XmlElement("UnlikedIcon")]
            public UiElement unlikedIcon { get; set; } = new UiElement();
            [XmlElement("SizeRange")]
            public SizeRange sizeRange { get; set; } = new SizeRange();
            [XmlArray("PageSlots"), XmlArrayItem("PageSlot")]
            public PageSlot[] pageSlotS { get; set; } = new PageSlot[0];
            [XmlElement("TabBar")]
            public TabBar tabBar { get; set; } = new TabBar();
            [XmlElement("ImageToolBox")]
            public ImageToolBox imageToolBox { get; set; } = new ImageToolBox();
            [XmlElement("CoverPicture")]
            public CoverPicture coverPicture { get; set; } = new CoverPicture();
        }



        [XmlArray("Styles"), XmlArrayItem("Style")]
        public Style[] styles { get; set; } = new Style[0];
    }
}

