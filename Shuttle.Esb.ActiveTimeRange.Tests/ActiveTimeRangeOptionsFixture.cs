using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Shuttle.Esb.ActiveTimeRange.Tests;

[TestFixture]
public class ActiveTimeRangeOptionsFixture
{
    protected ActiveTimeRangeOptions GetOptions()
    {
        var result = new ActiveTimeRangeOptions();

        new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\appsettings.json")).Build()
            .GetRequiredSection($"{ActiveTimeRangeOptions.SectionName}").Bind(result);

        return result;
    }

    [Test]
    public void Should_be_able_to_load_the_configuration()
    {
        var options = GetOptions();

        Assert.That(options, Is.Not.Null);
        Assert.That(options.ActiveFromTime, Is.EqualTo("8:00"));
        Assert.That(options.ActiveToTime, Is.EqualTo("23:00"));
    }
}