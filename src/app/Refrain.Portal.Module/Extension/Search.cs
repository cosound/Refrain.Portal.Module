namespace Refrain.Portal.Module.Extension
{
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
                    Query = string.Format("str_Text:({1}*)^5Text:(\"{0}\")^2str_Artist.Name:({1}*)^5Artist.Name:(\"{0}\")^2str_Country.Name:({1}*)^5Country.Name:(\"{0}\")^2Contest.Year:{0}(FreeText:{0})", query.ToLower(), query.ToLower().Replace(" ", "\\ ")),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

            var view = PortalApplication.ViewManager.GetView("Search");
            return view.Query(q);
        }
    }
}
