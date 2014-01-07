using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem.Authentication.MvcHelper.Test.InAppIps
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.MvcHelper.AppInfrastructure;

    public static class ClassUserMapping
    {
        [TestClass]
        public class Ctor
        {
            [TestMethod]
            public void DefaultCtor()
            {
                var target = new UserMapping();
                Assert.IsNotNull(target);
                Assert.IsNull(target.Name);
                Assert.IsNull(target.ExternalId);
            }

            [TestMethod]
            public void ParametrizedCtor()
            {
                var target = new UserMapping("1", "2");
                Assert.AreEqual("1", target.Name);
                Assert.AreEqual("2", target.ExternalId);
            }
        }
    }
}
