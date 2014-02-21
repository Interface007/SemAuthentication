// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassLandmineMvcAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClassLandmineMvcAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test.InAppIps
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.InAppIps;
    using Sem.Authentication.InAppIps.Processing;
    using Sem.Authentication.MvcHelper.InAppIps;

    public static class ClassLandmineMvcAttribute
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void InitializesDefaultExtractors()
            {
                var target = new LandmineMvcAttribute();
                Assert.IsTrue(target.ContextProcessors.Any(x => x.IdExtractor is SessionIdExtractor));
                Assert.IsTrue(target.ContextProcessors.Any(x => x.IdExtractor is UserHostExtractor));
            }

            [TestMethod]
            public void InitializesExplicitExtractors()
            {
                var target = new LandmineMvcAttribute(new[] { typeof(EmptyExtractor) });
                Assert.AreEqual(1, target.ContextProcessors.Count());
                Assert.IsTrue(target.ContextProcessors.Any(x => x.IdExtractor is EmptyExtractor));
            }

            [ExcludeFromCodeCoverage]
            public class EmptyExtractor : IIdExtractor
            {
                public string Extract(HttpContextBase context)
                {
                    return string.Empty;
                }
            }
        }

        [TestClass]
        public class ExpectedValue
        {
            [TestMethod]
            public void SetterAndGetterInSync()
            {
                var target = new LandmineMvcAttribute { ExpectedValue = "5D0C2EA8BAB44ACFAD107ACAD809D90E" };
                Assert.AreEqual("5D0C2EA8BAB44ACFAD107ACAD809D90E", target.ExpectedValue);
            }
        }

        [TestClass]
        public class LandmineName
        {
            [TestMethod]
            public void SetterAndGetterInSync()
            {
                var target = new LandmineMvcAttribute { LandmineName = "D6B1C3FAB8B3474C955E9D204EDD0B87" };
                Assert.AreEqual("D6B1C3FAB8B3474C955E9D204EDD0B87", target.LandmineName);
            }
        }

        [TestClass]
        public class Instance
        {
            [TestMethod]
            public void ReturnsCorrectType()
            {
                var target = new LandmineMvcAttribute();
                Assert.IsInstanceOfType(target.Instance, typeof(Landmine));
            }
        }
    }
}
