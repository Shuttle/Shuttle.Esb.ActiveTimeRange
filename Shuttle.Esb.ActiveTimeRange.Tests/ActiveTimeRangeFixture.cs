using System;
using NUnit.Framework;

namespace Shuttle.Esb.ActiveTimeRange.Tests
{
	[TestFixture]
	public class ActiveTimeRangeFixture
	{
		[Test]
		public void Should_be_able_to_create_a_time_range_for_the_whole_day()
		{
			var range = new ActiveTimeRange("*", "*");

			var now = DateTime.Now;

			Assert.IsTrue(range.Active(now));
			Assert.IsTrue(range.Active(new DateTime(now.Year, now.Month, now.Day, 0, 1, 0)));
			Assert.IsTrue(range.Active(new DateTime(now.Year, now.Month, now.Day, 23, 59, 0)));
		}

		[Test]
		public void Should_be_able_to_create_a_smaller_time_range()
		{
			var range = new ActiveTimeRange("13:30", "13:35");

			var now = DateTime.Now;

			Assert.IsFalse(range.Active(new DateTime(now.Year, now.Month, now.Day, 12, 30, 0)));
			Assert.IsFalse(range.Active(new DateTime(now.Year, now.Month, now.Day, 13, 29, 0)));

			Assert.IsTrue(range.Active(new DateTime(now.Year, now.Month, now.Day, 13, 30, 0)));
			Assert.IsTrue(range.Active(new DateTime(now.Year, now.Month, now.Day, 13, 32, 0)));
			Assert.IsTrue(range.Active(new DateTime(now.Year, now.Month, now.Day, 13, 35, 0)));

			Assert.IsFalse(range.Active(new DateTime(now.Year, now.Month, now.Day, 13, 36, 0)));
			Assert.IsFalse(range.Active(new DateTime(now.Year, now.Month, now.Day, 14, 30, 0)));
		}
	}
}