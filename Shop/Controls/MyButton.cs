

namespace Shop.Controls
{
    internal class MyButton : Button
    {

        private bool _isPressed = false;

        public MyButton() : base()
        {
            Pressed += MyButton_Pressed;
        }

        private void MyButton_Pressed(object sender, EventArgs e)
        {
            if (!_isPressed)
            {
                this.BackgroundColor = Color.Parse("#4297F4");
                _isPressed = true;
            }
            else
            {
                this.BackgroundColor = Color.Parse("#C8C8C8");
                _isPressed = false;
            }
        }
    }
}
