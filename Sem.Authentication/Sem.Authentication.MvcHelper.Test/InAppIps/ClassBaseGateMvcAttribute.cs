// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassBaseGateMvcAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClassBaseGateMvcAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test.InAppIps
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Sem.Authentication.MvcHelper.InAppIps;
    using Sem.Authentication.Processing;

    public static class ClassBaseGateMvcAttribute
    {
        [TestClass]
        public class OnActionExecuting
        {
            [TestMethod]
            public void NotCallsStatisticsGateIfNotWanted()
            {
                var hasBeenCalled = false;
                var mock = new Mock<IGate>();
                mock.Setup(x => x.CheckRequestGate).Returns(false);
                mock.Setup(x => x.CheckStatisticGate).Returns(false);
                mock.Setup(x => x.StatisticsGate(It.IsAny<string>(), It.IsAny<ConcurrentDictionary<string, ClientStatistic>>())).Returns(() => hasBeenCalled = true);
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost"), string.Empty, string.Empty);

                var target = new TestGateMvcAttribute(mock.Object);
                target.OnActionExecuting(context);

                Assert.IsFalse(hasBeenCalled);

                // call gate to execute the code
                mock.Setup(x => x.CheckStatisticGate).Returns(true);
                target.OnActionExecuting(context);
                Assert.IsTrue(hasBeenCalled);
            }

            [TestMethod]
            public void CallsStatisticsGateIfWanted()
            {
                var hasBeenCalled = false;
                var mock = new Mock<IGate>();
                mock.Setup(x => x.CheckRequestGate).Returns(false);
                mock.Setup(x => x.CheckStatisticGate).Returns(true);
                mock.Setup(x => x.StatisticsGate(It.IsAny<string>(), It.IsAny<ConcurrentDictionary<string, ClientStatistic>>())).Returns(() => hasBeenCalled = true);
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost"), string.Empty, string.Empty);

                var target = new TestGateMvcAttribute(mock.Object);
                target.OnActionExecuting(context);

                Assert.IsTrue(hasBeenCalled);
            }
         
            [TestMethod]
            public void NotCallsRequestGateIfNotWanted()
            {
                var hasBeenCalled = false;
                var mock = new Mock<IGate>();
                mock.Setup(x => x.CheckRequestGate).Returns(false);
                mock.Setup(x => x.CheckStatisticGate).Returns(false);
                mock.Setup(x => x.RequestGate(It.IsAny<string>(), It.IsAny<HttpRequestBase>())).Returns(() => hasBeenCalled = true);
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost"), string.Empty, string.Empty);

                var target = new TestGateMvcAttribute(mock.Object);
                target.OnActionExecuting(context);

                Assert.IsFalse(hasBeenCalled);

                // call gate to execute the code
                mock.Setup(x => x.CheckRequestGate).Returns(true);
                target.OnActionExecuting(context);
                Assert.IsTrue(hasBeenCalled);
            }
         
            [TestMethod]
            public void CallsRequestGateIfWanted()
            {
                var hasBeenCalled = false;
                var mock = new Mock<IGate>();
                mock.Setup(x => x.CheckRequestGate).Returns(true);
                mock.Setup(x => x.CheckStatisticGate).Returns(false);
                mock.Setup(x => x.RequestGate(It.IsAny<string>(), It.IsAny<HttpRequestBase>())).Returns(() => hasBeenCalled = true);
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost"), string.Empty, string.Empty);

                var target = new TestGateMvcAttribute(mock.Object);
                target.OnActionExecuting(context);

                Assert.IsTrue(hasBeenCalled);
            }
        }

        public class TestGateMvcAttribute : BaseGateMvcAttribute
        {
            private readonly IGate gate;

            public TestGateMvcAttribute(IGate gate)
            {
                this.gate = gate;
            }

            public override IGate Instance
            {
                get
                {
                    return this.gate;
                }
            }
        }
    }
}
