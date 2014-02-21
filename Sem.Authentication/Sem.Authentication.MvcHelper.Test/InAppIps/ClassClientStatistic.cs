// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassClientStatistic.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClientStatisticTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test.InAppIps
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.Processing;

    public static class ClassClientStatistic
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void InitializesDateTimeProperties()
            {
                var target = new ClientStatistic();
                var expected = DateTime.UtcNow.ToString("s");
                Assert.AreEqual(expected, target.FirstRequest.ToString("s"));
                Assert.AreEqual(expected, target.LastRequest.ToString("s"));
            }
        }

        [TestClass]
        public class RequestsPerSecond
        {
            [TestMethod]
            public void Returns1ForFirstCall()
            {
                var target = new ClientStatistic();
                target.IncreaseRequestCount();
                Assert.AreEqual(1, target.RequestsPerSecond);
            }

            [TestMethod]
            public async Task Returns1ForTwoCallsInTwoSeconds()
            {
                var target = new ClientStatistic();
                target.IncreaseRequestCount();
                await Task.Delay(1100);
                target.IncreaseRequestCount();
                Assert.AreEqual(1, target.RequestsPerSecond);
            }
        }
    }
}