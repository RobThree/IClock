using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IClock.Tests
{
    [TestClass]
    public class TestClockTests
    {
        [TestMethod]
        public void TestClock_GetTime_Returns_DefaultTime()
        {
            var target = new TestClock();
            Assert.AreEqual(TestClock.DefaultTime, target.GetTime());
        }

        [TestMethod]
        public void TestClock_GetTime_Returns_GivenTime()
        {
            var time = new DateTime(2020, 1, 2, 3, 4, 5, 6, DateTimeKind.Local);
            var target = new TestClock(time);
            Assert.AreEqual(time, target.GetTime());
        }

        [TestMethod]
        public void TestClock_Tick_DefaultIncrement_IsOneSecond()
        {
            var target = new TestClock();
            var t1 = target.GetTime();
            target.Tick();
            var t2 = target.GetTime();

            Assert.AreEqual(TimeSpan.FromSeconds(1), t2 - t1);
        }

        [TestMethod]
        public void TestClock_Tick_Uses_GivenIncrement()
        {
            var ts = TimeSpan.FromDays(Math.PI);
            var target = new TestClock(TestClock.DefaultTime, ts);
            var t1 = target.GetTime();
            target.Tick();
            var t2 = target.GetTime();

            Assert.AreEqual(ts, t2 - t1);
        }

        [TestMethod]
        public void TestClock_Set_ChangesTime()
        {
            var time = new DateTime(2020, 1, 2, 3, 4, 5, 6, DateTimeKind.Local);
            var target = new TestClock();
            target.Set(time);

            Assert.AreEqual(time, target.GetTime());
        }

        [TestMethod]
        public void TestClock_Adjust_SetsTime()
        {
            var target = new TestClock();
            var timespan = TimeSpan.FromSeconds(Math.PI);
            var t1 = target.GetTime();
            var r = target.Adjust(timespan);
            var t2 = target.GetTime();

            Assert.AreEqual(timespan, t2 - t1);
            Assert.AreEqual(t1.Add(timespan), r);
        }

        private static DateTimeOffset GetTime1() => TestClock.GetDeterministicRandomTime();
        private static DateTimeOffset GetTime2() => TestClock.GetDeterministicRandomTime();

        [TestMethod]
        public void TestClock_GetDeterministicRandomTime_IsDeterministic()
        {
            // Each invocation within the same method should return the same value
            Assert.AreEqual(TestClock.GetDeterministicRandomTime(), TestClock.GetDeterministicRandomTime());
            Assert.AreEqual(TestClock.GetDeterministicRandomTime("A"), TestClock.GetDeterministicRandomTime("A"));

            // Each invocation from different methods should (likely) return the different values
            Assert.AreNotEqual(GetTime1(), GetTime2());
            Assert.AreNotEqual(TestClock.GetDeterministicRandomTime("A"), TestClock.GetDeterministicRandomTime("B"));
        }

        [TestMethod]
        public void TestClock_TestVector()  // Do not change method name or fix expected value when you do
        {
            // Pre-calculated test vector
            var expected = new DateTimeOffset(635894622187178864L, TimeSpan.Zero);

            // Make sure GetRandomTime() returns a deterministic "random" time
            Assert.AreEqual(expected, TestClock.GetDeterministicRandomTime());
            // Make sure a second invocation doesn't change the returned time
            Assert.AreEqual(expected, TestClock.GetDeterministicRandomTime());
        }

        [TestMethod]
        public void TestClock_GetDeterministicRandomTime_UsesGivenOffset()
        {
            var offset = TimeSpan.FromMinutes(123);
            Assert.AreEqual(offset, TestClock.GetDeterministicRandomTime(offset).Offset);
        }

    }
}