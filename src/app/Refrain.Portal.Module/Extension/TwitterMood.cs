namespace Refrain.Portal.Module.Extension
{
    using System;
    using System.Linq;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Extension;
    using Chaos.Portal.Core.Indexing.Solr.Request;

    public class TwitterMood : AExtension
    {
        public TwitterMood(IPortalApplication portalApplication) : base(portalApplication)
        {
            
        }

        public IGroupedResult<IResult> Get(string country, DateTime? before, DateTime? after, uint groupPageSize = 1, uint groupPageIndex = 0)
        {
            var moodView = PortalApplication.ViewManager.GetView("TwitterMood");

            var moodQuery = new SolrQuery
                {
                    Query = GenerateQuery(country), 
                    Sort = "DateCreated desc", 
                    Filter = GenerateFilter(before, after),
                    PageSize = 100,
                    Group = new SolrGroup
                        {
                            Field = "Country.Name",
                            Limit = groupPageSize,
                            Offset = groupPageIndex * groupPageSize
                        }
                };

            var moodResult = moodView.GroupedQuery(moodQuery);

            return moodResult;
        }

        private static string GenerateFilter(DateTime? before, DateTime? after)
        {
            if(before.HasValue && after.HasValue)
                return string.Format("DateCreated:[{0} TO {1}]", after.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"), before.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));

            if (before.HasValue)
                return string.Format("DateCreated:[* TO {0}]", before.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
            
            if (after.HasValue)
                return string.Format("DateCreated:[{0} TO *]", after.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
            
            return "";
        }

        private static string GenerateQuery(string country)
        {
            if (string.IsNullOrEmpty(country)) return "*:*";

            var countries = country.Split(',');

            return string.Format("Country.Name:{0}", string.Join(" ", countries.Select(item => item.Trim())));
        }
    }
}
