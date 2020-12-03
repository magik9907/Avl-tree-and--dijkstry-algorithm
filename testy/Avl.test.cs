using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using projekt;
namespace testy
{
    [TestClass]
    public class UnitTest1
    {
        Avl avl = new Avl();

        

        [TestMethod]
        [Description("test adding first element in AVL tree")]
        public void AddFirstElement()
        {
            string city = "Medium";
            avl.add(city);
            Assert.AreEqual(city, avl.tree.city);
        }

        [TestMethod]
        public void AddLeftElement()
        {
            string city = "Medium";
            avl.add(city);
             city = "Amsterdam";
            avl.add(city);
            Assert.AreEqual(city, avl.tree.left.city);
        }
        [TestMethod]
        public void AddRightElement()
        {
            string city = "Medium";
            avl.add(city);
            city = "Zambrow";
            avl.add(city);
            Assert.AreEqual(city, avl.tree.right.city);

        }

        [TestMethod]
        public void AddFullThirdLevelOfTree()
        {
            string city1 = "Medium";
            avl.add(city1);
            string city22 = "Zambrow";
            avl.add(city22);
            string city21 = "Amsterdam";
            avl.add(city21);
            string city31 = "Abar";
            avl.add(city31);
            string city33 = "Barcelona";
            avl.add(city33);
            string city32 = "Quwejt";
            avl.add(city32);
            string city34 = "Kapsztad";
            avl.add(city34);
            Assert.AreEqual("Amsterdam", avl.tree.right.city);

        }

    }
}
