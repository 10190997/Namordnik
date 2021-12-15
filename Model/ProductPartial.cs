using System.Linq;

namespace Namordnik.Model
{
    public partial class Product
    {
        /// <summary>
        /// Цена рассчитывается из цены материалов
        /// </summary>
        public decimal Price
        {
            get
            {
                decimal price = 0;
                foreach (ProductMaterial item in ProductMaterials)
                {
                    price += item.Material.Cost;
                }
                return price;
            }
        }

        /// <summary>
        /// Список материалов
        /// </summary>
        public string MaterialList
        {
            get
            {
                string materials = "";
                foreach (ProductMaterial item in ProductMaterials)
                {
                    materials += item.Material.Title;
                    if (item != ProductMaterials.Last())
                    {
                        materials += ", ";
                    }

                }
                return materials;
            }
        }

        /// <summary>
        /// Путь к изображению с учетом заглушки
        /// </summary>
        public string PicturePath => Image ?? "../../products/picture.png";
    }
}
