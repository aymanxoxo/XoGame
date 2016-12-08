using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiddleMan.Tests
{
    [TestClass]
    public class MyEventTests
    {
        [TestMethod]
        public void NormalCase()
        {
            var str = "Hello";
            var x = new Temp();
            x.Subscribe(s =>
            {
                Assert.AreEqual(str, s);
            });
            x.Publish(str);
        }

        [TestMethod]
        public void NormalCase_UnSubscribe()
        {
            var str = "Hello";
            var x = new Temp();
            x.Subscribe(MyMethod);
            x.UnSubscribe(MyMethod);
            x.Publish(str);
        }
        
        public void MyMethod(string str)
        {
            if(!string.IsNullOrEmpty(str))
                Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubscribeWithNull_ThrowArgumentNullException()
        {
            var str = "Hello";
            var x = new Temp();
            x.Subscribe(null);
            x.Publish(str);
        }
        
    }
}
