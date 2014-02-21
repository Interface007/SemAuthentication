// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassLandmineExtensions.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClassLandmineExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.MvcHelper.InAppIps;

    public static class ClassLandmineExtensions
    {
        [TestClass]
        public class Landmine
        {
            [TestMethod]
            public void GeneratesExpectedHtml()
            {
                var result = LandmineExtensions.Landmine(null, new[] { "userid" });
                Assert.AreEqual(@"<input type=""hidden"" style=""hidden"" name=""Landmine"" value=""8008"" />", result.ToHtmlString());
            }
        }
    }
}
