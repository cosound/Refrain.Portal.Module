using System;
using CHAOS.Serialization;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Extension;
using Chaos.Portal.v5.Extension.Result;
using Refrain.Portal.Module.Core;

namespace Refrain.Portal.Module.Extension
{
  public class MetricRating : AExtension
  {
    public MetricRating(IPortalApplication portalApplication) : base(portalApplication) {}

    public EndpointResult Set(Guid songId1, Guid songId2, string metricFilter, uint metricIndex, int rating)
    {
      if(Request.Session == null)
        throw new Exception("Session missing");

      Context.MetricRatingGateway.Set(songId1,
                                      songId2,
                                      Request.Session.Guid,
                                      metricFilter,
                                      metricIndex,
                                      rating);

      return EndpointResult.Success();
    }
  }

  public class MetricStatsResult : AResult
  {
    [Serialize]
    public string SongId1 { get; set; }
    
    [Serialize]
    public string SongId2 { get; set; }
    
    [Serialize]
    public string MetricFilter { get; set; }
    
    [Serialize]
    public uint MetricIndex { get; set; }
    
    [Serialize]
    public int Rating { get; set; }
  }
}
