

using Shop.ViewModels;


namespace Shop.Services.Interfaces
{
    public interface ICheck_AndroidServives
    {
        Task CheckServices(ChangeNethwork callback = null);
        Task PermissionsStatus();
    }
}
