using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSUniversalLib;

namespace Tests
{
    [TestClass]
    public class CalculationTests
    {
        /// <summary>
        /// Тест с корректными входными данными
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_CorrectData()
        {
            Assert.AreEqual(114148, Calculation.GetQuantityForProduct(3, 1, 15, 20, 45));
        }

        /// <summary>
        /// Тест с несуществующим типом продукта
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NonExistentProductType()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(45, 1, 15, 20, 45));
        }

        /// <summary>
        /// Тест с несуществующим типом материала
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NonExistentMaterialType()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 71, 15, 20, 45));
        }

        /// <summary>
        /// Тест с отрицательным количеством
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NegativeCount()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, -15, 20, 45));
        }

        /// <summary>
        /// Тест с нулевым количеством
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_ZeroCount()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 0, 20, 45));
        }

        /// <summary>
        /// Тест с отрицательной шириной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NegativeWidth()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 15, -20, 45));
        }

        /// <summary>
        /// Тест с нулевой шириной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_ZeroWidth()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 15, 0, 45));
        }

        /// <summary>
        /// Тест с отрицательной длиной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NegativeLength()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 15, 20, -45));
        }

        /// <summary>
        /// Тест с нулевой длиной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_ZeroLength()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 15, 20, 0));
        }

        /// <summary>
        /// Тест с некорректными входными данными
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_AllIncorrect()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(-3, -1, -15, -20, -45));
        }

        /// <summary>
        /// Тест с дробной шириной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatWidth()
        {
            Assert.AreEqual(1173126, Calculation.GetQuantityForProduct(3, 1, 15, 205.54564f, 45));
        }

        /// <summary>
        /// Тест с дробной длиной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatLength()
        {
            Assert.AreEqual(1155782, Calculation.GetQuantityForProduct(3, 1, 15, 20, 455.64f));
        }

        /// <summary>
        /// Тест с дробной шириной и длиной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatLengthWidth()
        {
            Assert.AreEqual(11878287, Calculation.GetQuantityForProduct(3, 1, 15, 205.54564f, 455.64f));
        }

        /// <summary>
        /// Тест с дробной шириной и длиной для типа продукта 1
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatLengthWidthProductType1()
        {
            Assert.AreEqual(1549955, Calculation.GetQuantityForProduct(1, 1, 15, 205.54564f, 455.64f));
        }

        /// <summary>
        /// Тест с дробной шириной и длиной для типа материала 2
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatLengthWidthMaterialType2()
        {
            Assert.AreEqual(1547162, Calculation.GetQuantityForProduct(1, 2, 15, 205.54564f, 455.64f));
        }
    }
}
