using System;

namespace Refrain.Portal.Module.Core
{
  public static class Context
  {
    public static IMetricRatingGateway MetricRatingGateway { get; set; }
  }

  public interface IMetricRatingGateway
  {
    void Set(Guid songId1, Guid songId2, Guid sessionId, string metricFilter, uint metricIndex, int rating);
  }
}