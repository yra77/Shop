

namespace Shop.Controls.Map
{
    public interface IMap : IView
    {
        Enums.MapType MapType { get; }
        bool IsShowingUser { get; }
        string Address { get; set; }
    }
}
