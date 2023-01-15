

using System.Collections;


namespace Shop.Models
{
    internal class Brands : IEnumerable<string>
    {

        private List<string> Brand;


        public Brands()
        {
            Brand = new List<string>(new string[] { "Adidas", "New Balance", "Nike", "Puma"});
        }


        public IEnumerator<string> GetEnumerator()
        {
            foreach (string val in Brand)
            {
                yield return val;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
