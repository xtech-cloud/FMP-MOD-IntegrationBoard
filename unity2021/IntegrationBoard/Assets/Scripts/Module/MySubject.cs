
namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    public class MySubject : MySubjectBase
    {
        /// <summary>
        /// 激活标签
        /// </summary>
        /// <remarks>
        /// 以"__"开头和结尾的页面是内置页面
        /// </remarks>
        /// <example>
        /// var data = new Dictionary<string, object>();
        /// data["uid"] = "default";
        /// data["page"] = "__profile__";
        /// model.Publish(/XTC/IntegrationBoard/ActivatePage, data);
        /// </example>
        public const string ActivatePage = "/XTC/IntegrationBoard/ActivatePage";
    }
}
