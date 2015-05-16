using System;
using Chaos.Portal.Core.Data.Model;
using NUnit.Framework;
using Refrain.Portal.Module.Core;
using Refrain.Portal.Module.Extension;

namespace Refrain.Portal.Module.Test.Extension
{
  [TestFixture]
  public class MetricRatingTest : TestBase
  {
    [Test]
    public void Set_GivenValidMetricStats_ReturnSuccess()
    {
      var ext = Make_MetricRatingExtension();
      var stats = new MetricStatsResult
        {
          SongId1 = "00000000-0000-0000-0000-000000000001",
          SongId2 = "00000000-0000-0000-0000-000000000002",
          MetricFilter = "111111",
          MetricIndex = 0,
          Rating = -1
        };
      Request.Setup(p => p.Session).Returns(new Session());
      var spy = new MetricRatingGatewaySpy();
      Context.MetricRatingGateway = spy;

      var result = ext.Set(Guid.Parse(stats.SongId1), Guid.Parse(stats.SongId2), stats.MetricFilter, stats.MetricIndex, stats.Rating);

      Assert.That(result.WasSuccess, Is.True);
      Assert.That(spy.WasCalled, Is.True);
    }

    [Test, ExpectedException(typeof(Exception))]
    public void Set_NoSessionSet_Throw()
    {
      var ext = Make_MetricRatingExtension();

      ext.Set(Guid.Empty, Guid.Empty, null, 0, 0);
    }

    private MetricRating Make_MetricRatingExtension()
    {
      return (MetricRating) new MetricRating(PortalApplication.Object).WithPortalRequest(Request.Object);
    }
  }

  public class MetricRatingGatewaySpy : IMetricRatingGateway
  {
    public bool WasCalled;

    public void Set(Guid songId1, Guid songId2, Guid sessionId, string metricFilter, uint metricIndex, int rating)
    {
      WasCalled = true;
    }
  }
}
