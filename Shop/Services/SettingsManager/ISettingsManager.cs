

namespace Shop.Services.SettingsManager
{
    public interface ISettingsManager
    {
        string Email { get; set; }
        string Password { get; set; }
        string Name { get; set; }
        string Language { get; set; }
        string Tel { get; set; }
        string Address { get; set; }
        int State { get; set; }// 0 - default, 1 - changes without the internet 
        bool IsCircleCart { get; set; }
        bool IsCircleFavorit { get; set; }
        DateTime LastUpdate_DB { get; set; }
    }
}
