using System;

namespace WSUniversalLib
{
    public class Calculation
    {
        private static readonly float[] coefs = new float[] { 1.1f, 2.5f, 8.43f };
        private static readonly float[] defects = new float[] { 0.3f, 0.12f };

        /// <summary>
        /// Количество необходимого качественного сырья на одну единицу продукции рассчитывается как площадь продукции, умноженная на коэффициент типа продукции.
        /// </summary>
        /// <param name="productType">Тип продукции</param>
        /// <param name="materialType">Тип материала</param>
        /// <param name="count">Количество продукции</param>
        /// <param name="width">Ширина продукции</param>
        /// <param name="length">Длина продукции</param>
        /// <returns>Количество необходимого качественного сырья</returns>
        public static int GetQuantityForProduct(int productType, int materialType, int count, float width, float length)
        {
            if (productType > coefs.Length || materialType > defects.Length
                || count < 1 || width < 1 || length < 1 || productType < 0 || materialType < 0) 
            {
                return -1;
            }
            float square = width * length;
            float amount = square * count * coefs[productType - 1];
            float countMaterials = amount / (1 - defects[materialType - 1] / 100);
            return Convert.ToInt32(Math.Ceiling(countMaterials));
        }
    }
}
