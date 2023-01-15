

namespace Shop.Controls
{
    internal class MyStepper : StackLayout
    {

        private ImageButton _plusBtn;
        private ImageButton _minusBtn;
        private Label _label;


        public MyStepper()
        {

            _plusBtn = CreateButton("plus", "plus.png");
            _minusBtn = CreateButton("minus", "minus.png");
            CreateLabel();

            // _label.SetBinding(_label.TextProperty, new Binding(nameof(Text), BindingMode.TwoWay, source: this));

            Orientation = StackOrientation.Horizontal;
            Spacing = 15;

            _plusBtn.Clicked += PlusBtn_Clicked;
            _minusBtn.Clicked += MinusBtn_Clicked;

            Children.Add(_minusBtn);
            Children.Add(_label);
            Children.Add(_plusBtn);

            IsChange = 0;
            _label.Text = Text.ToString();
        }


        public static readonly BindableProperty TextProperty =
          BindableProperty.Create(
             propertyName: "Text",
              returnType: typeof(int),
              declaringType: typeof(MyStepper),
              defaultValue: 1,
              defaultBindingMode: BindingMode.TwoWay);
        public int Text
        {
            get { return (int)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); OnPropertyChanged(nameof(Text)); }
        }

        //for signal view model
        public static readonly BindableProperty IsChangeProperty =
          BindableProperty.Create(
             propertyName: "IsChange",
              returnType: typeof(int),
              declaringType: typeof(MyStepper),
              defaultValue: 0,
              defaultBindingMode: BindingMode.TwoWay);
        public int IsChange
        {
            get { return (int)GetValue(IsChangeProperty); }
            set { SetValue(IsChangeProperty, value); OnPropertyChanged(nameof(IsChange)); }
        }
        /// ///////

        public static readonly BindableProperty MinimumValueProperty =
            BindableProperty.Create("MinimumValue", typeof(int), typeof(MyStepper), defaultValue: 0);
        public int MinimumValue
        {
            get { return (int)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }


        public static readonly BindableProperty MaximumValueProperty =
            BindableProperty.Create("MaximumValue", typeof(int), typeof(MyStepper), defaultValue: 10);
        public int MaximumValue
        {
            get { return (int)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }


      
        private void CreateLabel()
        {
            _label = new Label
            {
                TextColor = Color.Parse("#000000"),
                BackgroundColor = Color.Parse("#F8F9FA"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "OpenSansSemibold",
                MaxLines = 1,
            };
        }

        private ImageButton CreateButton(string buttons_name, string path)
        {
            return new ImageButton
            {
                Source = path,
                WidthRequest = 20,
                HeightRequest = 20,
                CornerRadius = 10,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = (buttons_name == "plus") ?
                                   Color.Parse("#4297F4") :
                                   Color.Parse("#FFFFFF"),
            };
        }

        private void MinusBtn_Clicked(object sender, EventArgs e)
        {
            if (Text > MinimumValue)
            {
                _label.Text = (--Text).ToString();
                IsChange++;
            }
        }

        private void PlusBtn_Clicked(object sender, EventArgs e)
        {
            if (Text < MaximumValue)
            {
                _label.Text = (++Text).ToString();
                IsChange++;
            }
        }

    }
}
