using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.IntegrationBoard.LIB.Proto;
using XTC.FMP.MOD.IntegrationBoard.LIB.MVCS;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.Rendering;

namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    /// <summary>
    /// 实例类
    /// </summary>
    public class MyInstance : MyInstanceBase
    {
        public class AlignGrid
        {
            public AlignGrid(Vector2 _center, int _row, int _column)
            {
                Center = _center;
                Row = _row;
                Column = _column;
            }
            public Vector2 Center { get; private set; }
            public int Row { get; private set; }
            public int Column { get; private set; }
        }

        public class UiReference
        {
            public Transform tfPanel;
            public Transform tfTabBar;
            public Transform tfTitleBar;
            public Text txtTitle;
            public Text txtCaption;
            public Text txtTopic;
            public Text txtDescription;
            public Transform frameTopic;
            public Transform frameDescription;
            public RawImage imgPicture;
            public Toggle tgLike;
            public Toggle tgTabTemplate;
            public Button btnTabClose;
            public Button btnTopicSwitch;
            public Button btnDescriptionSwitch;
            public Transform tfImageToolBox;
            public List<Toggle> toggleTabS = new List<Toggle>();
            public Transform pageTemplate;
            public Dictionary<string, GameObject> pageS = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// 吸附后的对齐网格
        /// </summary>
        public AlignGrid stickedAlignGrid { get; private set; }
        /// <summary>
        /// 吸附后的x坐标
        /// </summary>
        public float stickedX { get; private set; }
        /// <summary>
        /// 吸附后的y坐标
        /// </summary>
        public float stickedY { get; private set; }

        private UiReference uiReference_ = new UiReference();
        private ContentReader contentReader_ = null;

        /// <summary>
        /// 封面图片适配后的大小
        /// </summary>
        private Vector2 pictureFitedSize_;
        private float pictureScale_ = 1.0f;

        private bool topicVisible_;
        private bool descriptionVisible_;
        private AlignGrid[] alignGridS_;
        private string activeContentUri_;

        private LibMVCS.Signal signalAddLike_;


        public MyInstance(string _uid, string _style, MyConfig _config, MyCatalog _catalog, LibMVCS.Logger _logger, Dictionary<string, LibMVCS.Any> _settings, MyEntryBase _entry, MonoBehaviour _mono, GameObject _rootAttachments)
            : base(_uid, _style, _config, _catalog, _logger, _settings, _entry, _mono, _rootAttachments)
        {
            contentReader_ = new ContentReader(contentObjectsPool);
            signalAddLike_ = new LibMVCS.Signal((entry_ as MyEntry).getDummyModel());
        }

        /// <summary>
        /// 当被创建时
        /// </summary>
        /// <remarks>
        /// 可用于加载主题目录的数据
        /// </remarks>
        public void HandleCreated()
        {
            uiReference_.tfPanel = rootUI.transform.Find("Board/Panel");
            uiReference_.tfTabBar = rootUI.transform.Find("Board/TabBar");
            uiReference_.tfTitleBar = rootUI.transform.Find("Board/Panel/__home__/TitleBar");
            uiReference_.txtTitle = rootUI.transform.Find("Board/Panel/__home__/TitleBar/txtTitle").GetComponent<Text>();
            uiReference_.txtCaption = rootUI.transform.Find("Board/Panel/__home__/TitleBar/txtCaption").GetComponent<Text>();
            uiReference_.txtTopic = rootUI.transform.Find("Board/Panel/__home__/frameTopic/txtTopic").GetComponent<Text>();
            uiReference_.txtDescription = rootUI.transform.Find("Board/Panel/__home__/frameDescription/ScrollView/Viewport/Content/txtDescription").GetComponent<Text>();
            uiReference_.frameTopic = rootUI.transform.Find("Board/Panel/__home__/frameTopic");
            uiReference_.frameDescription = rootUI.transform.Find("Board/Panel/__home__/frameDescription");
            uiReference_.btnTopicSwitch = rootUI.transform.Find("Board/Panel/__home__/frameTopic/btnSwitch").GetComponent<Button>();
            uiReference_.btnDescriptionSwitch = rootUI.transform.Find("Board/Panel/__home__/frameDescription/btnSwitch").GetComponent<Button>();
            uiReference_.imgPicture = rootUI.transform.Find("Board/Panel/__home__/svPicture/Viewport/Content").GetComponent<RawImage>();
            uiReference_.tgLike = rootUI.transform.Find("Board/Panel/__home__/tgLike").GetComponent<Toggle>();
            uiReference_.tgTabTemplate = rootUI.transform.Find("Board/TabBar/tgTemplate").GetComponent<Toggle>();
            uiReference_.tgTabTemplate.gameObject.SetActive(false);
            uiReference_.btnTabClose = rootUI.transform.Find("Board/TabBar/btnClose").GetComponent<Button>();
            uiReference_.pageTemplate = rootUI.transform.Find("Board/Panel/pageTemplate");
            uiReference_.tfImageToolBox = rootUI.transform.Find("Board/Panel/__home__/imageToolBox");
            uiReference_.pageTemplate.gameObject.SetActive(false);
            uiReference_.pageS.Add("__home__", rootUI.transform.Find("Board/Panel/__home__").gameObject);
            applyStyle();
            createTabs();
            bindEvents();
            signalAddLike_.Connect(handleLikeChanged);

            if (null == alignGridS_)
            {
                var instanceSize = rootUI.GetComponent<RectTransform>().rect.size;
                GenerateAlignGrid(instanceSize);
            }
        }

        /// <summary>
        /// 当被删除时
        /// </summary>
        public void HandleDeleted()
        {
            //TODO Upgrade oelMVCS
            //signalAddLike_.Disconnect(handleLikeChanged);
        }

        /// <summary>
        /// 当被打开时
        /// </summary>
        /// <remarks>
        /// 可用于加载内容目录的数据
        /// </remarks>
        public void HandleOpened(string _source, string _uri)
        {
            refreshContent(_source, _uri);
            rootUI.gameObject.SetActive(true);
        }

        /// <summary>
        /// 当被关闭时
        /// </summary>
        public void HandleClosed()
        {
            rootUI.gameObject.SetActive(false);
        }

        /// <summary>
        /// 生成对齐网格
        /// </summary>
        /// <remarks>
        /// 此函数可以在HandleCreated之前调用
        /// </remarks>
        public void GenerateAlignGrid(Vector2 _viewportSize)
        {
            // 创建对齐网格
            alignGridS_ = new AlignGrid[style_.alignGrid.row * style_.alignGrid.column];
            int height = (int)(_viewportSize.y / style_.alignGrid.row);
            int width = (int)(_viewportSize.x / style_.alignGrid.column);
            for (int i = 0; i < style_.alignGrid.row; i++)
            {
                for (int j = 0; j < style_.alignGrid.column; j++)
                {
                    float x = -_viewportSize.x / 2 + j * width + width / 2;
                    float y = -_viewportSize.y / 2 + i * height + height / 2;
                    alignGridS_[i * style_.alignGrid.row + j] = new AlignGrid(new Vector2(x, y), i, j);
                }
            }
        }

        public void ActivatePage(string _page)
        {
            foreach (var pair in uiReference_.pageS)
            {
                pair.Value.SetActive(pair.Key == _page);
            }
        }

        /// <summary>
        /// 吸附对齐网格
        /// </summary>
        /// <param name="_x">x坐标</param>
        /// <param name="_y">y坐标</param>
        /// <param name="_stickedX">吸附后的x坐标</param>
        /// <param name="_stickedY">吸附后的y坐标</param>
        public void StickAlignGrid(float _x, float _y)
        {
            stickedX = _x;
            stickedY = _y;
            Vector2 position = new Vector2(_x, _y);
            float minDistance = float.MaxValue;
            foreach (var grid in alignGridS_)
            {
                float offset = Vector2.Distance(position, grid.Center);
                if (offset < minDistance)
                {
                    minDistance = offset;
                    if (style_.alignGrid.stickH)
                        stickedX = grid.Center.x;
                    if (style_.alignGrid.stickV)
                        stickedY = grid.Center.y;
                    stickedAlignGrid = grid;
                }
            }
        }

        private void applyStyle()
        {
            var rtTemplate = rootUI.transform.Find("Board").GetComponent<RectTransform>();
            rtTemplate.sizeDelta = new Vector2(style_.width, style_.height);

            // 加载面板相关图片
            loadTextureFromTheme(style_.panelBackground.image, (_texture) =>
            {
                Vector4 border = new Vector4(style_.panelBackground.border.left, style_.panelBackground.border.bottom, style_.panelBackground.border.right, style_.panelBackground.border.top);
                Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, border);
                var image = uiReference_.tfPanel.GetComponent<Image>();
                image.sprite = sprite;
                Color color;
                if (ColorUtility.TryParseHtmlString(style_.panelBackground.color, out color))
                    image.color = color;
                image = uiReference_.tfPanel.Find("bg").GetComponent<Image>();
                image.sprite = sprite;
                image.color = color;
            }, () =>
            {

            });


            // 加载标题栏相关图片
            loadTextureFromTheme(style_.titleBarBackground.image, (_texture) =>
            {
                Vector4 border = new Vector4(style_.titleBarBackground.border.left, style_.titleBarBackground.border.bottom, style_.titleBarBackground.border.right, style_.titleBarBackground.border.top);
                Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, border);
                var image = uiReference_.tfTitleBar.GetComponent<Image>();
                image.sprite = sprite;
                Color color;
                if (ColorUtility.TryParseHtmlString(style_.titleBarBackground.color, out color))
                    image.color = color;
                image = uiReference_.tfTitleBar.Find("bg").GetComponent<Image>();
                image.sprite = sprite;
                image.color = color;
            }, () =>
            {

            });

            // 更改点赞按钮样式
            loadTextureFromTheme(style_.likeBackground.image, (_texture) =>
            {
                Vector4 border = new Vector4(style_.likeBackground.border.left, style_.likeBackground.border.bottom, style_.likeBackground.border.right, style_.likeBackground.border.top);
                Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, border);
                var image = uiReference_.tgLike.transform.Find("Background").GetComponent<Image>();
                image.sprite = sprite;
                Color color;
                if (ColorUtility.TryParseHtmlString(style_.likeBackground.color, out color))
                    image.color = color;
            }, () =>
            {

            });

            // 加载工具栏相关图片
            loadTextureFromTheme(style_.tabBar.background.image, (_texture) =>
            {
                Vector4 border = new Vector4(style_.tabBar.background.border.left, style_.tabBar.background.border.bottom, style_.tabBar.background.border.right, style_.tabBar.background.border.top);
                Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, border);
                var image = uiReference_.tfTabBar.GetComponent<Image>();
                image.sprite = sprite;
                Color color;
                if (ColorUtility.TryParseHtmlString(style_.tabBar.background.color, out color))
                    image.color = color;

                image = uiReference_.tfTabBar.Find("bg").GetComponent<Image>();
                image.sprite = sprite;
                image.color = color;
            }, () =>
            {

            });

            // 加载标语面板背景
            loadTextureFromTheme(style_.topicBackground.image, (_texture) =>
            {
                Vector4 border = new Vector4(style_.topicBackground.border.left, style_.topicBackground.border.bottom, style_.topicBackground.border.right, style_.topicBackground.border.top);
                Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, border);
                var image = uiReference_.frameTopic.GetComponent<Image>();
                image.sprite = sprite;
                Color color;
                if (ColorUtility.TryParseHtmlString(style_.topicBackground.color, out color))
                    image.color = color;
            }, () =>
            {

            });

            // 加载标语切换图标
            loadTextureFromTheme(style_.topicSwitchIcon.image, (_texture) =>
            {
                uiReference_.btnTopicSwitch.GetComponent<RawImage>().texture = _texture;
            }, () =>
            {

            });

            // 加载描述面板背景
            loadTextureFromTheme(style_.descriptionBackground.image, (_texture) =>
            {
                Vector4 border = new Vector4(style_.descriptionBackground.border.left, style_.descriptionBackground.border.bottom, style_.descriptionBackground.border.right, style_.descriptionBackground.border.top);
                Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, border);
                var image = uiReference_.frameDescription.GetComponent<Image>();
                image.sprite = sprite;
                Color color;
                if (ColorUtility.TryParseHtmlString(style_.descriptionBackground.color, out color))
                    image.color = color;
            }, () =>
            {

            });

            // 加载点赞后图标
            loadTextureFromTheme(style_.likedIcon.image, (_texture) =>
            {
                var icon = uiReference_.tgLike.transform.Find("Background/icon_checked");
                icon.GetComponent<RawImage>().texture = _texture;
                alignByAncor(icon, style_.likedIcon.anchor);
            }, () =>
            {

            });

            // 加载点赞前图标
            loadTextureFromTheme(style_.unlikedIcon.image, (_texture) =>
            {
                var icon = uiReference_.tgLike.transform.Find("Background/icon_normal");
                icon.GetComponent<RawImage>().texture = _texture;
                alignByAncor(icon, style_.unlikedIcon.anchor);
            }, () =>
            {

            });

            // 加载描述切换图标
            loadTextureFromTheme(style_.descriptionSwitchIcon.image, (_texture) =>
            {
                uiReference_.btnDescriptionSwitch.GetComponent<RawImage>().texture = _texture;
            }, () =>
            {

            });

            // 加载关闭按钮图标
            loadTextureFromTheme(style_.tabBar.closeButton.image, (_texture) =>
            {
                uiReference_.btnTabClose.GetComponent<RawImage>().texture = _texture;
                alignByAncor(uiReference_.btnTabClose.transform, style_.tabBar.closeButton.anchor);
            }, () =>
            {

            });

            // 加载图片工具栏背景
            loadTextureFromTheme(style_.imageToolBox.image, (_texture) =>
            {
                Vector4 border = new Vector4(style_.imageToolBox.border.left, style_.imageToolBox.border.bottom, style_.imageToolBox.border.right, style_.imageToolBox.border.top);
                Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, border);
                var image = uiReference_.tfImageToolBox.GetComponent<Image>();
                image.sprite = sprite;
                Color color;
                if (ColorUtility.TryParseHtmlString(style_.imageToolBox.color, out color))
                    image.color = color;
                alignByAncor(uiReference_.tfImageToolBox, style_.imageToolBox.anchor);
            }, () =>
            {

            });

            // 加载图片工具栏缩小按钮
            loadTextureFromTheme(style_.imageToolBox.btnZoomOut.image, (_texture) =>
            {
                var btn = uiReference_.tfImageToolBox.Find("btnZoomOut");
                btn.GetComponent<RawImage>().texture = _texture;
                alignByAncor(btn, style_.imageToolBox.btnZoomOut.anchor);
            }, () =>
            {

            });

            // 加载图片工具栏放大按钮
            loadTextureFromTheme(style_.imageToolBox.btnZoomIn.image, (_texture) =>
            {
                var btn = uiReference_.tfImageToolBox.Find("btnZoomIn");
                btn.GetComponent<RawImage>().texture = _texture;
                alignByAncor(btn, style_.imageToolBox.btnZoomIn.anchor);
            }, () =>
            {

            });

            // 加载图片关闭按钮
            loadTextureFromTheme(style_.imageToolBox.btnBack.image, (_texture) =>
            {
                var btn = uiReference_.tfImageToolBox.Find("btnBack");
                btn.GetComponent<RawImage>().texture = _texture;
                alignByAncor(btn, style_.imageToolBox.btnBack.anchor);
            }, () =>
            {

            });

        }

        private void bindEvents()
        {
            uiReference_.btnTopicSwitch.onClick.AddListener(() =>
            {
                uiReference_.frameTopic.gameObject.SetActive(false);
                uiReference_.frameDescription.gameObject.SetActive(true);
                mono_.StartCoroutine(fitDescription());
            });
            uiReference_.btnDescriptionSwitch.onClick.AddListener(() =>
            {
                uiReference_.frameTopic.gameObject.SetActive(true);
                uiReference_.frameDescription.gameObject.SetActive(false);
            });
            uiReference_.btnTabClose.onClick.AddListener(() =>
            {
                Dictionary<string, object> variableS = new Dictionary<string, object>();
                variableS["{{uid}}"] = this.uid;
                foreach (var subject in style_.tabBar.closeButton.subjects)
                {
                    publishSubject(subject, variableS);
                }
            });
            uiReference_.imgPicture.GetComponent<Button>().onClick.AddListener(() =>
            {
                uiReference_.tgLike.gameObject.SetActive(false);
                uiReference_.frameTopic.gameObject.SetActive(false);
                uiReference_.frameDescription.gameObject.SetActive(false);
                uiReference_.tfTitleBar.gameObject.SetActive(false);
                uiReference_.tfImageToolBox.gameObject.SetActive(true);
            });
            uiReference_.tfImageToolBox.Find("btnZoomIn").GetComponent<Button>().onClick.AddListener(() =>
            {
                pictureScale_ += 0.2f;
                uiReference_.imgPicture.rectTransform.sizeDelta = pictureFitedSize_ * pictureScale_;
            });
            uiReference_.tfImageToolBox.Find("btnZoomOut").GetComponent<Button>().onClick.AddListener(() =>
            {
                pictureScale_ -= 0.2f;
                if (pictureScale_ < 1.0f)
                    pictureScale_ = 1.0f;
                uiReference_.imgPicture.rectTransform.sizeDelta = pictureFitedSize_ * pictureScale_;
            });
            uiReference_.tfImageToolBox.Find("btnBack").GetComponent<Button>().onClick.AddListener(() =>
            {
                uiReference_.imgPicture.rectTransform.sizeDelta = pictureFitedSize_;
                uiReference_.imgPicture.rectTransform.anchoredPosition = Vector2.zero;
                uiReference_.tgLike.gameObject.SetActive(true);
                uiReference_.frameTopic.gameObject.SetActive(topicVisible_);
                uiReference_.frameDescription.gameObject.SetActive(descriptionVisible_);
                if (topicVisible_)
                    uiReference_.frameDescription.gameObject.SetActive(false);
                uiReference_.tfTitleBar.gameObject.SetActive(true);
                uiReference_.tfImageToolBox.gameObject.SetActive(false);
                pictureScale_ = 1.0f;
            });
            uiReference_.tgLike.onValueChanged.AddListener((_toggled) =>
            {
                if (_toggled)
                {
                    uiReference_.tgLike.interactable = false;
                    (entry_ as MyEntry).getDummyModel().SaveAddLike(activeContentUri_);
                    signalAddLike_.Emit(null);
                }
            });
        }

        private void refreshContent(string _source, string _uri)
        {
            activeContentUri_ = _uri;
            // 在刷新内容前先隐藏相关组件
            uiReference_.imgPicture.gameObject.SetActive(false);
            uiReference_.tgLike.gameObject.SetActive(false);
            uiReference_.txtTitle.text = "";
            uiReference_.txtCaption.text = "";
            uiReference_.txtTopic.text = "";
            uiReference_.txtDescription.text = "";
            uiReference_.btnTopicSwitch.gameObject.SetActive(false);
            uiReference_.btnDescriptionSwitch.gameObject.SetActive(false);
            uiReference_.frameDescription.gameObject.SetActive(false);
            uiReference_.frameTopic.gameObject.SetActive(false);
            uiReference_.tfImageToolBox.gameObject.SetActive(false);
            // 刷新Tab后允许全部关闭
            uiReference_.tgTabTemplate.group.allowSwitchOff = true;

            System.Action<ContentMetaSchema> onLoadContentSuccess = (_content) =>
            {
                if (null == _content)
                    return;
                topicVisible_ = !string.IsNullOrEmpty(_content.topic);
                descriptionVisible_ = !string.IsNullOrEmpty(_content.description);
                uiReference_.txtTitle.text = _content.title;
                uiReference_.txtCaption.text = _content.caption;
                uiReference_.txtTopic.text = _content.topic;
                uiReference_.frameTopic.gameObject.SetActive(topicVisible_);
                uiReference_.txtDescription.text = _content.description;
                uiReference_.frameDescription.gameObject.SetActive(descriptionVisible_);
                // topic和description均有值的时候，显示切换方式
                if (topicVisible_ && descriptionVisible_)
                {
                    uiReference_.btnTopicSwitch.gameObject.SetActive(true);
                    uiReference_.btnDescriptionSwitch.gameObject.SetActive(true);
                    uiReference_.frameDescription.gameObject.SetActive(false);
                }
                uiReference_.tgLike.gameObject.SetActive(true);
                contentReader_.LoadTexture("cover.png", (_texture) =>
                {
                    uiReference_.imgPicture.texture = _texture;
                    uiReference_.imgPicture.SetNativeSize();
                    uiReference_.imgPicture.gameObject.SetActive(true);
                    fitImage();
                }, () =>
                {

                });
                signalAddLike_.Emit(null);
                uiReference_.tgLike.interactable = true;
                uiReference_.toggleTabS.First().isOn = true;
                // 刷新所有tab对应的页面
                uiReference_.toggleTabS.ForEach((_toggle) =>
                {
                    bool visible = false;
                    foreach (var tab in style_.tabBar.tabButtons)
                    {
                        if (tab.name == _toggle.name)
                        {
                            string contentKV_value;
                            _content.kvS.TryGetValue(tab.contentKey, out contentKV_value);
                            visible = tab.contentKey == "_" || !string.IsNullOrEmpty(contentKV_value);
                            if (!string.IsNullOrEmpty(contentKV_value))
                            {
                                foreach (var pageSlot in style_.pageSlotS)
                                {
                                    if (pageSlot.page == tab.name)
                                    {
                                        Dictionary<string, object> variableS = new Dictionary<string, object>();
                                        variableS["{{uid}}"] = this.uid;
                                        // assloud中的相对路径
                                        variableS["{{uri}}"] = string.Format("{0}/{1}", _content.foreign_bundle_uuid, contentKV_value);
                                        // content的路径，bundle_uuid/content_uuid
                                        variableS["{{content_path}}"] = string.Format("{0}/{1}", _content.foreign_bundle_uuid, _content.Uuid);
                                        publishSubject(pageSlot.refreshSubject, variableS);
                                    }
                                }
                            }
                            break;
                        }
                    }
                    _toggle.gameObject.SetActive(visible);
                });
                // 刷新Tab后不允许全部关闭
                uiReference_.tgTabTemplate.group.allowSwitchOff = false;
            };

            if (_source == "assloud://")
            {

                contentReader_.AssetRootPath = settings_["path.assets"].AsString();
                contentReader_.ContentUri = _uri;
                contentReader_.LoadText("meta.json", (_data) =>
                {
                    ContentMetaSchema content = null;
                    try
                    {
                        content = JsonConvert.DeserializeObject<ContentMetaSchema>(System.Text.Encoding.UTF8.GetString(_data));
                    }
                    catch (System.Exception ex)
                    {
                        logger_.Exception(ex);
                    }
                    onLoadContentSuccess(content);
                }, () =>
                {
                });
            }
        }

        private void createTabs()
        {
            uiReference_.toggleTabS.Clear();
            foreach (var tab in style_.tabBar.tabButtons)
            {
                var clone = GameObject.Instantiate(uiReference_.tgTabTemplate, uiReference_.tgTabTemplate.transform.parent);
                clone.gameObject.SetActive(false);
                clone.name = tab.name;
                var imgUnchecked = clone.transform.Find("Unchecked").GetComponent<RawImage>();
                imgUnchecked.GetComponent<RectTransform>().sizeDelta = new Vector2(tab.uncheckedWidth, tab.uncheckedHeight);
                loadTextureFromTheme(tab.uncheckedIcon, (_texture) =>
                {
                    imgUnchecked.texture = _texture;
                }, () =>
                {
                });
                var imgChecked = clone.transform.Find("Checked").GetComponent<RawImage>();
                imgChecked.GetComponent<RectTransform>().sizeDelta = new Vector2(tab.checkedWidth, tab.checkedHeight);
                loadTextureFromTheme(tab.checkedIcon, (_texture) =>
                {
                    imgChecked.texture = _texture;
                }, () =>
                {
                });
                var toggle = clone.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener((_toggle) =>
                {
                    imgChecked.gameObject.SetActive(_toggle);
                    imgUnchecked.gameObject.SetActive(!_toggle);
                    Vector2 sizeDelta = _toggle ? imgChecked.GetComponent<RectTransform>().sizeDelta : imgUnchecked.GetComponent<RectTransform>().sizeDelta;
                    clone.GetComponent<RectTransform>().sizeDelta = sizeDelta;
                    if (!_toggle)
                        return;
                    mono_.StartCoroutine(relayoutTabBar());
                    Dictionary<string, object> variableS = new Dictionary<string, object>();
                    variableS["{{uid}}"] = this.uid;
                    foreach (var subject in tab.subjects)
                    {
                        publishSubject(subject, variableS);
                    }
                });
                clone.GetComponent<RectTransform>().sizeDelta = new Vector2(tab.uncheckedWidth, tab.uncheckedHeight);
                uiReference_.toggleTabS.Add(toggle);

                if (!uiReference_.pageS.ContainsKey(tab.name))
                {
                    // 创建对应的page
                    var pageClone = GameObject.Instantiate(uiReference_.pageTemplate.gameObject, uiReference_.pageTemplate.parent);
                    uiReference_.pageS[tab.name] = pageClone;
                    pageClone.name = tab.name;
                    foreach (var pageSlot in style_.pageSlotS)
                    {
                        if (pageSlot.page == tab.name)
                        {
                            Dictionary<string, object> variableS = new Dictionary<string, object>();
                            variableS["{{uid}}"] = this.uid;
                            variableS["{{page_slot}}"] = pageClone;
                            publishSubject(pageSlot.inlaySubject, variableS);
                            break;
                        }
                    }
                }
            }
            uiReference_.btnTabClose.transform.SetAsLastSibling();
        }

        private IEnumerator relayoutTabBar()
        {
            yield return new WaitForEndOfFrame();
            var rtCloseButton = uiReference_.btnTabClose.GetComponent<RectTransform>();

            float width = 0;
            foreach (var toggle in uiReference_.toggleTabS)
            {
                if (!toggle.gameObject.activeSelf)
                    continue;
                width += toggle.GetComponent<RectTransform>().sizeDelta.x;
            }
            width += rtCloseButton.sizeDelta.x;

            float offset = width / 2;
            float lastX = -offset;
            foreach (var toggle in uiReference_.toggleTabS)
            {
                if (!toggle.gameObject.activeSelf)
                    continue;
                var rt = toggle.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(lastX + rt.sizeDelta.x / 2, 0);
                lastX += rt.sizeDelta.x + 24;
            }
            rtCloseButton.anchoredPosition = new Vector2(lastX + rtCloseButton.sizeDelta.x / 2, 0);
        }

        private void publishSubject(MyConfigBase.Subject _subject, Dictionary<string, object> _variableS)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var parameter in _subject.parameters)
            {
                if (parameter.type == "_")
                {
                    if (_variableS.ContainsKey(parameter.value))
                        parameters[parameter.key] = _variableS[parameter.value];
                }
                else if (parameter.type == "string")
                    parameters[parameter.key] = parameter.value;
                else if (parameter.type == "int")
                    parameters[parameter.key] = int.Parse(parameter.value);
                else if (parameter.type == "float")
                    parameters[parameter.key] = float.Parse(parameter.value);
                else if (parameter.type == "bool")
                    parameters[parameter.key] = bool.Parse(parameter.value);
            }
            (entry_ as MyEntry).getDummyModel().Publish(_subject.message, parameters);
        }

        private void fitImage()
        {
            pictureScale_ = 1.0f;
            if (style_.coverPicture.fitmode == "none")
                return;

            var rtImageSize = uiReference_.imgPicture.GetComponent<RectTransform>();
            var rtPanelSize = uiReference_.tfPanel.GetComponent<RectTransform>();
            // 容器的宽高比
            float ratioParent = rtPanelSize.rect.size.x / rtPanelSize.rect.size.y;
            //图片和容器的宽度差值
            float differenceX = rtImageSize.sizeDelta.x - rtPanelSize.rect.width;
            //图片和容器的高度差值
            float differenceY = rtImageSize.sizeDelta.y - rtPanelSize.rect.height;
            // 将高度的差值换算成和宽度的比例尺一致
            differenceY *= ratioParent;

            float fitX = rtImageSize.sizeDelta.x;
            float fitY = rtImageSize.sizeDelta.y;

            Action fitWidth = () =>
            {
                fitX = rtPanelSize.rect.width;
                fitY = rtImageSize.sizeDelta.y / rtImageSize.sizeDelta.x * fitX;
            };
            Action fitHeight = () =>
            {
                fitY = rtPanelSize.rect.height;
                fitX = rtImageSize.sizeDelta.x / rtImageSize.sizeDelta.y * fitY;
            };

            // 缩小以适配面板
            if (style_.coverPicture.fitmode == "zoomout")
            {
                // 两边都大于面板
                if (differenceX > 0 && differenceY > 0)
                {
                    // 适配宽度
                    if (differenceX > differenceY)
                        fitWidth();
                    // 适配高度
                    else
                        fitHeight();
                }
                // 横向超出面板
                else if (differenceX > 0 && differenceY <= 0)
                    fitWidth();
                // 纵向超出面板
                else if (differenceX <= 0 && differenceY > 0)
                    fitHeight();
            }
            // 放大以适配面板
            else if (style_.coverPicture.fitmode == "zoomout")
            {
                // 两边都小于面板
                if (differenceX < 0 && differenceY < 0)
                {
                    // 适配高度
                    if (differenceX < differenceY)
                        fitHeight();
                    // 适配宽度
                    else
                        fitWidth();
                }
            }
            // 放大以及缩小
            else if (style_.coverPicture.fitmode == "zoom")
            {
                // 两边都大于面板
                if (differenceX > 0 && differenceY > 0)
                {
                    // 适配宽度
                    if (differenceX > differenceY)
                        fitWidth();
                    // 适配高度
                    else
                        fitHeight();
                }
                // 两边都小于面板
                else if (differenceX < 0 && differenceY < 0)
                {
                    // 适配高度
                    if (differenceX < differenceY)
                        fitHeight();
                    // 适配宽度
                    else
                        fitWidth();
                }
                // 横向超出面板
                else if (differenceX > 0 && differenceY <= 0)
                    fitWidth();
                // 纵向超出面板
                else if (differenceX <= 0 && differenceY > 0)
                    fitHeight();
            }

            pictureFitedSize_ = new Vector2(fitX, fitY);
            pictureScale_ = 1.0f;
            rtImageSize.sizeDelta = pictureFitedSize_;
        }

        private IEnumerator fitDescription()
        {
            if (!uiReference_.frameDescription.gameObject.activeSelf)
                yield break;
            yield return new WaitForEndOfFrame();
            var rtScrollView = uiReference_.frameDescription.Find("ScrollView").GetComponent<RectTransform>();
            var rtText = uiReference_.txtDescription.GetComponent<RectTransform>();
            var rtFrame = uiReference_.frameDescription.GetComponent<RectTransform>();
            var sizeDelta = rtFrame.sizeDelta;
            // rtScrollView.sizeDelta.y 是负数
            float height = Mathf.Min(rtText.sizeDelta.y - rtScrollView.sizeDelta.y, style_.sizeRange.descriptionMaxHeight);
            sizeDelta.y = height;
            rtFrame.sizeDelta = sizeDelta;
        }

        private void handleLikeChanged(LibMVCS.Model.Status _status, object _data)
        {
            DummyModel.DummyStatus status = _status as DummyModel.DummyStatus;
            int count;
            status.likeStatusS.TryGetValue(activeContentUri_, out count);
            uiReference_.tgLike.transform.Find("Label").GetComponent<Text>().text = count.ToString();
        }
    }
}
