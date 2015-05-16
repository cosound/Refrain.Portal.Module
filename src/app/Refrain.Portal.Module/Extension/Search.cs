namespace Refrain.Portal.Module.Extension
{
  using System;
  using Chaos.Portal.Core;
  using Chaos.Portal.Core.Data.Model;
  using Chaos.Portal.Core.Extension;
  using Chaos.Portal.Core.Indexing.Solr.Request;
  using Data;

  public class Search : AExtension
  {
    public IRefrainRepository Repository { get; set; }

    public Search(IPortalApplication portalApplication, IRefrainRepository repository) : base(portalApplication)
    {
      Repository = repository;
    }

    public IPagedResult<IResult> Get(string query, uint pageIndex = 0, uint pageSize = 5)
    {
      var q = new SolrQuery
        {
          Query = GenerateQuery(query),
          Filter = GenerateFilter(query),
          PageIndex = pageIndex,
          PageSize = pageSize
        };

      var view = PortalApplication.ViewManager.GetView("Search");
      return view.Query(q);
    }

    private static string GenerateQuery(string query)
    {
      var terms = query.Split(' ');

      if (terms.Length == 1)
      {
        var year = GetYearOrZero(terms[0]);

        if (year != 0) return "*:*";
      }

      return string.Format("str_Text:({1}*)^25Text:({0}*)^15str_Artist.Name:({1}*)^15Artist.Name:({0}*)^5str_Country.Name:({1}*)^5Country.Name:({0}*)",
        query.ToLower(), query.ToLower().Replace(" ", "\\ "));
    }

    private string GenerateFilter(string query)
    {
      var year = GetYearOrZero(query);

      return year == 0 ? "(IsESC:True)" : string.Format("(Contest.Year:{0})AND(IsESC:True)", year);
    }

    private static int GetYearOrZero(string query)
    {
      var terms = query.Split(' ');
      var year = 0;

      foreach (var word in terms)
      {
        if (int.TryParse(word, out year)) break;
      }

      return year;
    }

    public IPagedResult<IResult> By(Guid id)
    {
      var q = new SolrQuery
        {
          Query = "Id:" + id,
          PageSize = 1
        };

      var view = PortalApplication.ViewManager.GetView("Search");
      return view.Query(q);
    }
  }
}