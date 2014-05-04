﻿namespace Refrain.Portal.Module.Extension
{
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

        public IGroupedResult<IResult> Get(string country)
        {
            var moodView = PortalApplication.ViewManager.GetView("TwitterMood");

            var moodQuery = new SolrQuery
                {
                    Query = GenerateQuery(country), Sort = "DateCreated desc", Group = new SolrGroup
                        {
                            Field = "Country.Name", Limit = 1
                        },
                    PageSize = 100
                };

            var moodResult = moodView.GroupedQuery(moodQuery);

            return moodResult;
        }

        private static string GenerateQuery(string country)
        {
            if (string.IsNullOrEmpty(country)) return "*:*";

            var countries = country.Split(',');

            return string.Format("Country.Name:{0}", string.Join(" ", countries.Select(item => item.Trim())));
        }
    }
}
