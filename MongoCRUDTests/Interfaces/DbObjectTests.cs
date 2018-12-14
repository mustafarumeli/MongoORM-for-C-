using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MongoCRUD.Interfaces.Tests
{
    [TestClass()]
    public class DbObjectTests
    {
        private class TestClass:DbObject
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string FieldValue;
        }
        [TestMethod()]
        public void ToStringTest()
        {
            var testObject = new TestClass
            {
                FieldValue = "FieldValue",
                Value = "PropValue",
                Name = "PropName",
                CreationDate = new DateTime(2015,5,13,10,10,10),
                _id = "TestId"
            };
            var result = testObject.ToString();
            Assert.AreEqual(result, "Name:PropName, Value:PropValue, _id:TestId, CreationDate:15/05/13 10:10:10 AM, FieldValue:FieldValue");
        }
    }
}