namespace Refrain.Portal.Module.Extension
{
    using System.Linq;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Extension;
    using Chaos.Portal.Core.Indexing.Solr.Request;

    public class Tweet : AExtension
    {
        public Tweet(IPortalApplication portalApplication) : base(portalApplication)
        {
            
        }

        public IGroupedResult<IResult> Get(string country, uint groupLimit = 5)
        {
            var tweetView = PortalApplication.ViewManager.GetView("Tweet");

            var tweetQuery = new SolrQuery
            {
                Query = GenerateQuery(country),
                Sort = "DateCreated desc",
                Group = new SolrGroup
                {
                    Field = "Country.Name",
                    Limit = groupLimit
                },
                PageSize = 100
            };

            var tweetResult = tweetView.GroupedQuery(tweetQuery);

            return tweetResult;
        }

        private static string GenerateQuery(string country)
        {
            if (string.IsNullOrEmpty(country)) return "*:*";

            var countries = country.Split(',');

            return string.Format("Country.Name:({0})", string.Join(" ", countries.Select(item => item.Trim())));
        }
    }
}
