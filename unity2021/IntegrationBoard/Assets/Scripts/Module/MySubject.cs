
namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    public class MySubject : MySubjectBase
    {
        /// <summary>
        /// 激活标签
        /// </summary>
        /// <see cref="https://github.com/xtech-cloud/fmp-doc/blob/main/module-hub/xtc_integrationboard/README.md"/>
        /// <remarks>
        /// 以"__"开头和结尾的页面是内置页面
        /// </remarks>
        public const string ActivatePage = "/XTC/IntegrationBoard/ActivatePage";

        /// <summary>
        /// 直接创建并打开
        /// </summary>
        /// <see cref="https://github.com/xtech-cloud/fmp-doc/blob/main/module-hub/xtc_integrationboard/README.md"/>
        public const string DirectOpen= "/XTC/IntegrationBoard/DirectOpen";

        /// <summary>
        /// 直接关闭并销毁
        /// </summary>
        /// <see cref="https://github.com/xtech-cloud/fmp-doc/blob/main/module-hub/xtc_integrationboard/README.md"/>
        public const string DirectClose = "/XTC/IntegrationBoard/DirectClose";

        /// <summary>
        /// 关闭所有并销毁
        /// </summary>
        /// <see cref="https://github.com/xtech-cloud/fmp-doc/blob/main/module-hub/xtc_integrationboard/README.md"/>
        public const string CloseAll = "/XTC/IntegrationBoard/CloseAll";

        /// <summary>
        /// 刷新
        /// </summary>
        public const string Refresh = "/XTC/IntegrationBoard/Refresh";

        /// <summary>
        /// 重置自动关闭计时器
        /// </summary>
        public const string ResetAutoCloseTimer = "/XTC/IntegrationBoard/ResetAutoCloseTimer";
    }
}
