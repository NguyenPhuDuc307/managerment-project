namespace NetCore.ViewModels.Systems
{
    public class UpdatePermissionRequest
    {
        public List<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();
    }
}