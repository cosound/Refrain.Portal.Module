namespace Refrain.Portal.Module.View
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using CHAOS.Serialization;
  using Chaos.Portal.Core.Data.Model;
  using Chaos.Portal.Core.Indexing;
  using Chaos.Portal.Core.Indexing.View;
  using Domain;
  using Object = Chaos.Mcm.Data.Dto.Object;

  public class SearchView : AView
  {
    private CoSoundConfiguration Config { get; set; }

    public SearchView(CoSoundConfiguration config)
      : base("Search")
    {
      Config = config;
    }

    public override IList<IViewData> Index(object objectsToIndex)
    {
      var obj = objectsToIndex as Object;

      if (obj == null) return new List<IViewData>();
      if (obj.ObjectTypeID != Config.ObjectTypes.ManifestationId) return new List<IViewData>();

      return Index(obj).ToList();
    }

    private IEnumerable<IViewData> Index(Object manifest)
    {
      var song = SongMapper.Create(manifest, Config);

      yield return new SearchViewData
        {
          Identity = song.Id,
          Text = song.Title,
          ArtistName = song.ArtistName,
          CountryName = song.CountryName,
          ContestYear = song.ContestYear,
          IsEurovision = song.IsEurovisionTrack
        };
      ;
    }

    public override IPagedResult<IResult> Query(IQuery query)
    {
      return Query<SearchViewData>(query);
    }
  }

  public class SearchViewData : IViewData
  {
    [Serialize("Id")]
    public Guid Identity { get; set; }

    [Serialize]
    public string Text { get; set; }

    [Serialize]
    public string ArtistName { get; set; }

    [Serialize]
    public string CountryName { get; set; }

    [Serialize]
    public string ContestYear { get; set; }

    [Serialize]
    public bool IsEurovision { get; set; }

    public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
    {
      yield return UniqueIdentifier;

      if (!string.IsNullOrEmpty(Text))
        yield return new KeyValuePair<string, string>("Text", Text.ToLower());

      if (!string.IsNullOrEmpty(ArtistName))
        yield return new KeyValuePair<string, string>("Artist.Name", ArtistName.ToLower());

      if (!string.IsNullOrEmpty(CountryName))
        yield return new KeyValuePair<string, string>("Country.Name", CountryName.ToLower());

      if (!string.IsNullOrEmpty(ContestYear))
        yield return new KeyValuePair<string, string>("Contest.Year", ContestYear);

      yield return new KeyValuePair<string, string>("IsESC", IsEurovision.ToString().ToLower());
    }

    public KeyValuePair<string, string> UniqueIdentifier
    {
      get { return new KeyValuePair<string, string>("Id", Identity.ToString()); }
    }

    public string Fullname
    {
      get { return "Refrain.Portal.Module.View.SearchViewData"; }
    }
  }
}