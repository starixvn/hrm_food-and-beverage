namespace NexFB.Web.Services.Interface
{
    using NexFB.Core.Enums;

    /// <summary>
    /// The interface of authority service.
    /// </summary>
    public interface IWebCommonService
    {
        int GetPermission(string functionKey);

        bool HasPermissionOnFunction(string functionKey, Policy policy);
    }
}
