namespace Refrain.Portal.Module.Extension
{
    using System;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Extension;
    using Chaos.Portal.Core.Indexing.Solr.Request;

    public class Song : AExtension
    {
        public Song(IPortalApplication portalApplication) : base(portalApplication)
        {
        }

        public IPagedResult<IResult> Get(Guid id, string type)
        {
            var query = new SolrQuery
                {
                    Query = String.Format("Id:{0}_{1}", id, type),
                    PageSize = 1
                };

            var view = ViewManager.GetView("Song");
            return view.Query(query);
        }
    }
}
