

namespace Shop.Converters
{

    public class ColorToString
    {

        public static string Convert(Color color)
        {
            string _color = null;

            if (color == Color.Parse("Red"))
                _color = "Red";
            if (color == Color.Parse("Pink"))
                _color = "Pink";
            if (color == Color.Parse("White"))
                _color = "White";
            if (color == Color.Parse("Black"))
                _color = "Black";
            if (color == Color.Parse("Green"))
                _color = "Green";
            if (color == Color.Parse("Blue"))
                _color = "Blue";
            if (color == Color.Parse("Yellow"))
                _color = "Yellow";
            if (color == Color.Parse("Brown"))
                _color = "Brown";
            if (color == Color.Parse("Gray"))
                _color = "Gray";

            return _color;
        }
    }
}
