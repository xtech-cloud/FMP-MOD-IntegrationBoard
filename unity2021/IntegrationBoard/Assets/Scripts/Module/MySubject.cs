
namespace XTC.FMP.MOD.IntegrationBoard.LIB.Unity
{
    public class MySubject : MySubjectBase
    {
        /// <summary>
        /// �����ǩ
        /// </summary>
        /// <remarks>
        /// ��"__"��ͷ�ͽ�β��ҳ��������ҳ��
        /// </remarks>
        /// <example>
        /// var data = new Dictionary<string, object>();
        /// data["uid"] = "default";
        /// data["page"] = "__profile__";
        /// model.Publish(/XTC/IntegrationBoard/ActivatePage, data);
        /// </example>
        public const string ActivatePage = "/XTC/IntegrationBoard/ActivatePage";

        /// <summary>
        /// ֱ�Ӵ�
        /// </summary>
        /// <remarks>
        /// ��������ָ������λ����ʾָ������
        /// </remarks>
        /// var data = new Dictionary<string, object>();
        /// data["uid"] = "default";
        /// model.Publish(/XTC/IntegrationBoard/CreateAndOpen, data);
        /// </example>
        public const string DirectOpen= "/XTC/IntegrationBoard/DirectOpen";

        public const string DirectClose = "/XTC/IntegrationBoard/DirectClose";
    }
}
