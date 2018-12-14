using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoCRUD;

namespace MongoCRUD.Tests
{
    [TestClass()]
    public class MongoConnectionObjectTests
    {
        [TestMethod()]
        public void ToStringTest_AllFields()
        {
            var mObject = new MongoConnectionObject
            {
                ReplicasIpConfig = {new MongoConnectionObject.IpConfig
                {
                    Host = "HostTest",
                    Port = 27017
                },
                new MongoConnectionObject.IpConfig
                    {
                        Host = "HostTest2",
                        Port = 27017
                    } },
                ConnectionOption = { { "TestKey1", "TestValue1" }, { "TestKey2", "TestValue2" } },
                DatabaseName = "DatabaseTest",
                MainIpConfig = new MongoConnectionObject.IpConfig
                {
                    Host = "MainHostTest",
                    Port = 27017
                },
                Password = "123456",
                UserName = "UserTest"
            };
            var test = mObject.ToString();
            Assert.AreEqual(test, "mongodb://UserTest:123456@MainHostTest:27017,HostTest:27017,HostTest2:27017/DatabaseTest?TestKey1=TestValue1&TestKey2=TestValue2");
        }
        [TestMethod()]
        public void ToStringTest_NoUser()
        {
            var mObject = new MongoConnectionObject
            {
                ReplicasIpConfig = {new MongoConnectionObject.IpConfig
                    {
                        Host = "HostTest",
                        Port = 27017
                    },
                    new MongoConnectionObject.IpConfig
                    {
                        Host = "HostTest2",
                        Port = 27017
                    } },
                ConnectionOption = { { "TestKey1", "TestValue1" }, { "TestKey2", "TestValue2" } },
                DatabaseName = "DatabaseTest",
                MainIpConfig = new MongoConnectionObject.IpConfig
                {
                    Host = "MainHostTest",
                    Port = 27017
                }
            };
            var test = mObject.ToString();
            Assert.AreEqual(test, "mongodb://MainHostTest:27017,HostTest:27017,HostTest2:27017/DatabaseTest?TestKey1=TestValue1&TestKey2=TestValue2");
        }
        [TestMethod()]
        public void ToStringTest_NoDatabaseName()
        {
            var mObject = new MongoConnectionObject
            {
                ReplicasIpConfig = {new MongoConnectionObject.IpConfig
                    {
                        Host = "HostTest",
                        Port = 27017
                    },
                    new MongoConnectionObject.IpConfig
                    {
                        Host = "HostTest2",
                        Port = 27017
                    } },
                ConnectionOption = { { "TestKey1", "TestValue1" }, { "TestKey2", "TestValue2" } },
                MainIpConfig = new MongoConnectionObject.IpConfig
                {
                    Host = "MainHostTest",
                    Port = 27017
                },
                Password = "123456",
                UserName = "UserTest"
            };
            var test = mObject.ToString();
            Assert.AreEqual(test, "mongodb://UserTest:123456@MainHostTest:27017,HostTest:27017,HostTest2:27017/?TestKey1=TestValue1&TestKey2=TestValue2");
        }
        [TestMethod()]
        public void ToStringTest_NoReplica()
        {
            var mObject = new MongoConnectionObject
            {
                ConnectionOption = { { "TestKey1", "TestValue1" }, { "TestKey2", "TestValue2" } },
                DatabaseName = "DatabaseTest",
                MainIpConfig = new MongoConnectionObject.IpConfig
                {
                    Host = "MainHostTest",
                    Port = 27017
                },
                Password = "123456",
                UserName = "UserTest"
            };
            var test = mObject.ToString();
            Assert.AreEqual(test, "mongodb://UserTest:123456@MainHostTest:27017/DatabaseTest?TestKey1=TestValue1&TestKey2=TestValue2");
        }
        [TestMethod()]
        public void ToStringTest_NoConnectionOption()
        {
            var mObject = new MongoConnectionObject
            {
                ReplicasIpConfig = {new MongoConnectionObject.IpConfig
                    {
                        Host = "HostTest",
                        Port = 27017
                    },
                    new MongoConnectionObject.IpConfig
                    {
                        Host = "HostTest2",
                        Port = 27017
                    } },
                DatabaseName = "DatabaseTest",
                MainIpConfig = new MongoConnectionObject.IpConfig
                {
                    Host = "MainHostTest",
                    Port = 27017
                },
                Password = "123456",
                UserName = "UserTest"
            };
            var test = mObject.ToString();
            Assert.AreEqual(test, "mongodb://UserTest:123456@MainHostTest:27017,HostTest:27017,HostTest2:27017/DatabaseTest");
        }
        [TestMethod()]
        public void ToStringTest_NoDatabaseName_NoConnectionOption()
        {
            var mObject = new MongoConnectionObject
            {
                ReplicasIpConfig = {new MongoConnectionObject.IpConfig
                    {
                        Host = "HostTest",
                        Port = 27017
                    },
                    new MongoConnectionObject.IpConfig
                    {
                        Host = "HostTest2",
                        Port = 27017
                    } },
                MainIpConfig = new MongoConnectionObject.IpConfig
                {
                    Host = "MainHostTest",
                    Port = 27017
                },
                Password = "123456",
                UserName = "UserTest"
            };
            var test = mObject.ToString();
            Assert.AreEqual(test, "mongodb://UserTest:123456@MainHostTest:27017,HostTest:27017,HostTest2:27017");
        }

        [TestMethod]
        public void ToStringTest_CtorHost()
        {
            

        }
    }
}