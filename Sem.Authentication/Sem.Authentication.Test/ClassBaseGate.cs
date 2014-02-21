// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassBaseGate.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClassBaseGate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class ClassBaseGate
    {
        [TestClass]
        public class StatisticsGate
        {
            [TestMethod]
            public void DefaultsForProperties()
            {
                var target = new SimpleGate();
                Assert.IsTrue(target.CheckRequestGate);
                Assert.IsTrue(target.CheckStatisticGate);
                Assert.IsTrue(target.RequestGate(null, null));
                Assert.IsTrue(target.StatisticsGate(null, null));
            }
        }
    }
}