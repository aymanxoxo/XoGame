using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiddleMan.Tests
{
	[TestClass]
    public class EventAggregatorTests
    {
	    [TestMethod]
	    public void NormalCase_GetInstance()
	    {
	        var x = EventAggregator.Resolve<Temp>();
	        Assert.IsNotNull(x);
	    }

	    [TestMethod]
	    public void RegisterObject_GetTheSameObject()
	    {
	        var t = new Temp {Id = 1};
	        var x = EventAggregator.Resolve<Temp>();
	        Assert.IsNotNull(x);
	        Assert.AreEqual(x.Id, t.Id);
	    }

	    [TestMethod]
	    [ExpectedException(typeof (ArgumentNullException))]
	    public void RegisterNullObject_ThrowNullException()
	    {
	        EventAggregator.Register(null);
	    }
		

	    [TestMethod]
	    public void ResolveObjectWithTheAbstractParent_ReturnNull()
	    {
	        var t = new Temp {Id = 1};
	        var x = EventAggregator.Resolve<MyEvent<string>>();
	        Assert.IsNull(x);
	    }

        [TestMethod]
        public void ResolveObjectWithTheParentInterface_ReturnNull()
        {
            var t = new Temp { Id = 1 };
            var x = EventAggregator.Resolve<IMyEvent>();
            Assert.IsNull(x);
        }

    }
}
