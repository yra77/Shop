

using Shop.Models;
using System.Collections.ObjectModel;


namespace Shop.Converters
{
    internal class ToProductWithLists
    {

        public static ObservableCollection<ProductWithList> ConvertToProductWithLists(List<Product> list)
        {

            ObservableCollection<ProductWithList> listNew = new ObservableCollection<ProductWithList>();
            //Parallel.For (0, list.Count, i=>
            for (int i = 0; i < list.Count; i++)
            {
             
                List<string> size = list[i].Sizes.Split(' ').ToList();
                var temp = list[i].Colors.Split(' ').ToList();

                List<Color> color = new List<Color>();
                foreach (var item in temp)
                {
                    color.Add(Color.Parse(item));
                }

                listNew.Add(new ProductWithList
                {
                    Id = list[i].Id,
                    Name = list[i].Name,
                    Photos_Large = list[i].Photos_Large,
                    Brand = list[i].Brand,
                    Sizes = size,
                    Color = color,
                    Count = list[i].Count,
                    Date = list[i].Date,
                    Gender = list[i].Gender,
                    IsNew = list[i].IsNew,
                    IsBestSeller = list[i].IsBestSeller,
                    Description = list[i].Description,
                    Price = list[i].Price
                });

            }//);
            return listNew;
        }

        public static ProductWithList ConvertToProductWithLists(Product prod)
        {

           // List<string> path = prod.PhotosPath_List.Split(' ').ToList();
            List<string> size = prod.Sizes.Split(' ').ToList();
            var temp = prod.Colors.Split(' ').ToList();

            List<Color> color = new List<Color>();
            foreach (var item in temp)
            {
                color.Add(Color.Parse(item));
            }

            ProductWithList aa = new ProductWithList()
            {
                Id = prod.Id,
                Name = prod.Name,
                Photos_Large = prod.Photos_Large,
                Brand = prod.Brand,
                Sizes = size,
                Color = color,
                Count = prod.Count,
                Date = prod.Date,
                Gender = prod.Gender,
                IsNew = prod.IsNew,
                IsBestSeller = prod.IsBestSeller,
                Description = prod.Description,
                Price = prod.Price
            };

            return aa;
        }
    }
}
