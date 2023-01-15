

using Shop.Enums;


namespace Shop.Controls.Map
{
    public partial class Map : View, IMap
    {


        public static readonly BindableProperty MapTypeProperty = BindableProperty.Create(nameof(MapType),
                                                      typeof(MapType), typeof(Map), default(MapType));

        public MapType MapType
        {
            get { return (MapType)GetValue(MapTypeProperty); }
            set { SetValue(MapTypeProperty, value); }
        }


        public static readonly BindableProperty IsShowingUserProperty = BindableProperty.Create(nameof(IsShowingUser),
                                                      typeof(bool), typeof(Map), default(bool));

        public bool IsShowingUser
        {
            get { return (bool)GetValue(IsShowingUserProperty); }
            set { SetValue(IsShowingUserProperty, value); }
        }


        public static readonly BindableProperty AddressProperty = BindableProperty.Create(nameof(Address),
                                                      typeof(string), typeof(Map), default(string), BindingMode.TwoWay);

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }
    }
}

