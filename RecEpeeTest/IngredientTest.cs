using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecEpee.Framework;
using RecEpee.Models;
using System;

namespace RicettarioTest
{
    [TestClass]
    public class IngredientTest
    {
        [TestMethod]
        public void TestNullInput()
        {
            Ingredient ingredient = IngredientParser.Parse(null);
            Assert.IsNull(ingredient);
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            Ingredient ingredient = IngredientParser.Parse(String.Empty);
            Assert.IsNull(ingredient);
        }

        [TestMethod]
        public void TestWhitespaceInput()
        {
            Ingredient ingredient = IngredientParser.Parse(" ");
            Assert.IsNull(ingredient);
        }

        [TestMethod]
        public void TestSingleString()
        {
            TestName("cioccolato", "cioccolato");
        }

        [TestMethod]
        public void TestSingleStringWithWhitespace()
        {
            TestName(" cioccolato ", "cioccolato");
        }

        [TestMethod]
        public void TestMultipleWordsName()
        {
            TestName(" cioccolato al latte", "cioccolato al latte");
        }

        [TestMethod]
        public void TestNameAndQuantity()
        {
            TestNameAndQuantity("uova 2", "uova", 2);
        }

        [TestMethod]
        public void TestMultipleWordsNameAndQuantity()
        {
            TestNameAndQuantity("uova fresche 2", "uova fresche", 2);
        }

        [TestMethod]
        public void TestNameQuantityAndUnit()
        {
            TestNameQuantityAndUnit("zucchero 100 g", "zucchero", 100, "g");
        }

        [TestMethod]
        public void TestNameQuantityAndUnitWithoutSpace()
        {
            TestNameQuantityAndUnit("zucchero 100g", "zucchero", 100, "g");
        }

        [TestMethod]
        public void TestMultipleWordsNameQuantityAndUnit()
        {
            TestNameQuantityAndUnit("zucchero a velo 3 cucchiaini", "zucchero a velo", 3, "cucchiaini");
        }

        private static void TestName(string input, string expected)
        {
            Ingredient ingredient = IngredientParser.Parse(input);
            Assert.AreEqual(expected, ingredient.Name);
        }

        private static void TestNameAndQuantity(string input, string expectedName, int expectedQuantity)
        {
            Ingredient ingredient = IngredientParser.Parse(input);
            Assert.AreEqual(expectedName, ingredient.Name);
            Assert.AreEqual(expectedQuantity, ingredient.Quantity);
        }

        private static void TestNameQuantityAndUnit(string input, string expectedName, int expectedQuantity, string expectedUnit)
        {
            Ingredient ingredient = IngredientParser.Parse(input);
            Assert.AreEqual(expectedName, ingredient.Name);
            Assert.AreEqual(expectedQuantity, ingredient.Quantity);
            Assert.AreEqual(expectedUnit, ingredient.Unit);
        }
    }
}
