
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

        public class MainBackground : VisualUiElement
        {
            [XmlElement("Margin")]
            public Border margin { get; set; } = new Border();
        }

        public class MainMask : VisualUiElement
        {
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

        public class LabelButton
        {
            [XmlAttribute("fontSize")]
            public int fontSize { get; set; } = 12;
            [XmlAttribute("fontColor")]
            public string fontColor { get; set; } = "#FFFFFFFF";
            [XmlAttribute("height")]
            public int height { get; set; } = 24;
            [XmlAttribute("image")]
            public string image { get; set; } = "";
            [XmlElement("Border")]
            public Border border { get; set; } = new Border();
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
            [XmlAttribute]
            public string layout { get; set; } = "";
            [XmlAttribute]
            public int offset { get; set; } = 0;
            [XmlAttribute]
            public bool useMask { get; set; } = true;
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

        public class SwitchButton
        {
            [XmlAttribute("background")]
            public string background { get; set; } = "";
            [XmlElement("Border")]
            public Border border { get; set; } = new Border();
            [XmlAttribute("icon")]
            public string icon { get; set; } = "";
            [XmlAttribute("iconSize")]
            public int iconSize { get; set; } = 16;
        }

        public class Like
        {
            [XmlElement("Anchor")]
            public Anchor anchor { get; set; } = new Anchor();
            [XmlElement("Background")]
            public VisualUiElement background { get; set; } = new VisualUiElement();
            [XmlElement("SelectedIcon")]
            public UiElement selectedIcon { get; set; } = new UiElement();
            [XmlElement("UnselectedIcon")]
            public UiElement unselectedIcon { get; set; } = new UiElement();
        }

        public class Topic
        {
            [XmlElement("Anchor")]
            public Anchor anchor { get; set; } = new Anchor();
            [XmlElement("Background")]
            public VisualUiElement background { get; set; } = new VisualUiElement();
            [XmlElement("SwitchButton")]
            public SwitchButton switchButton { get; set; } = new SwitchButton();
        }

        public class Description
        {
            [XmlAttribute("maxHeight")]
            public int maxHeight { get; set; } = 110;
            [XmlElement("Anchor")]
            public Anchor anchor { get; set; } = new Anchor();
            [XmlElement("Background")]
            public VisualUiElement background { get; set; } = new VisualUiElement();
            [XmlElement("SwitchButton")]
            public SwitchButton switchButton { get; set; } = new SwitchButton();
        }

        public class Style
        {
            [XmlAttribute("name")]
            public string name { get; set; } = "";
            [XmlAttribute("width")]
            public int width { get; set; } = 0;
            [XmlAttribute("height")]
            public int height { get; set; } = 0;
            [XmlAttribute("autoCloseTimeout")]
            public int autoCloseTimeout { get; set; } = 0;
            [XmlAttribute("effect")]
            public string effect { get; set; } = "";
            [XmlElement("AlignGrid")]
            public AlignGrid alignGrid { get; set; } = new AlignGrid();
            [XmlElement("MainMask")]
            public MainMask mainMask { get; set; } = new MainMask();
            [XmlElement("MainBackground")]
            public MainBackground mainBackground { get; set; } = new MainBackground();
            [XmlElement("Panel")]
            public VisualUiElement panel { get; set; } = new VisualUiElement();
            [XmlElement("Like")]
            public Like like { get; set; } = new Like();
            [XmlElement("Topic")]
            public Topic topic { get; set; } = new Topic();
            [XmlElement("Description")]
            public Description description { get; set; } = new Description();
            [XmlElement("TitleBarBackground")]
            public VisualUiElement titleBarBackground { get; set; } = new VisualUiElement();
            [XmlElement("ToolBoxBackground")]
            public VisualUiElement toolBoxBackground { get; set; } = new VisualUiElement();
            [XmlArray("PageSlots"), XmlArrayItem("PageSlot")]
            public PageSlot[] pageSlotS { get; set; } = new PageSlot[0];
            [XmlElement("TabBar")]
            public TabBar tabBar { get; set; } = new TabBar();
            [XmlElement("ImageToolBox")]
            public ImageToolBox imageToolBox { get; set; } = new ImageToolBox();
            [XmlElement("CoverPicture")]
            public CoverPicture coverPicture { get; set; } = new CoverPicture();
            [XmlElement("LabelButton")]
            public LabelButton labelButton { get; set; } = new LabelButton();
            [XmlElement("EventHandler")]
            public EventHandler eventHandler { get; set; } = new EventHandler();
        }

        public class EventHandler
        {
            [XmlArray("OnOpenS"), XmlArrayItem("Subject")]
            public Subject[] onOpenSubjectS { get; set; } = new Subject[0];
            [XmlArray("OnLikeS"), XmlArrayItem("Subject")]
            public Subject[] onLikeSubjectS { get; set; } = new Subject[0];
            [XmlArray("OnRefreshS"), XmlArrayItem("Subject")]
            public Subject[] onRefreshSubjectS { get; set; } = new Subject[0];
            [XmlArray("OnCloseS"), XmlArrayItem("Subject")]
            public Subject[] onCloseSubjectS { get; set; } = new Subject[0];
        }



        [XmlArray("Styles"), XmlArrayItem("Style")]
        public Style[] styles { get; set; } = new Style[0];
    }
}

