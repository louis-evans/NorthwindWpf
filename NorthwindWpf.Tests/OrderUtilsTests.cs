
using NorthwindWpf.Core.Utils;
using NUnit.Framework;
using System;

namespace NorthwindWpf.Tests
{
    [TestFixture]
    public class OrderUtilsTests
    {
        [Test]
        public void Should_Be_Final_Price_Of_49_14([Values(15.75)] decimal unitPrice, [Values(4)] int qty, [Values(22)] float discountPct)
        {
            Assert.AreEqual(49.14, Math.Round(OrderUtils.CalculateLineTotal(unitPrice, qty, discountPct), 2));
        }
    }
}
