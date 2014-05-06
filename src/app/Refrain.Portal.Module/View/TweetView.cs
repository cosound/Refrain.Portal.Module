namespace Refrain.Portal.Module.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Indexing;
    using Chaos.Portal.Core.Indexing.View;
    using Domain;
    using Domain.Dto;
    using Exceptions;
    using Object = Chaos.Mcm.Data.Dto.Object;

    public class TweetView : AView
    {
        public CoSoundConfiguration Config { get; set; }

        public TweetView(CoSoundConfiguration coSoundConfiguration) : base("Tweet")
        {
            Config = coSoundConfiguration;
        }

        public override IList<IViewData> Index(object objectsToIndex)
        {
            var obj = objectsToIndex as Object;

            if(obj == null) return new List<IViewData>();

            return Index(obj).ToList();
        }
        
        public IEnumerable<IViewData> Index(Object moodState)
        {
            try
            {
                var twitterMood = TwitterMoodMapper.Create(moodState, Config);

                return twitterMood.Tweets.Select(item => new TweetViewData
                    {
                        Country = twitterMood.Country,
                        DateCreated = twitterMood.DateCreated,
                        EmbedCode = item.EmbedCode,
                        Id = item.Id
                    });
            }
            catch (CannotMapException e)
            {
                return new List<IViewData>();
            }
        }

        public override IGroupedResult<IResult> GroupedQuery(IQuery query)
        {
            var response = Core.Query(query);

            var lst = new List<ResultGroup<TweetViewData>>();

            foreach (var queryResultGroup in response.QueryResultGroups)
            {
                foreach (var queryResult in queryResultGroup.Groups)
                {
                    var keys = queryResult.Results.Select(item => CreateKey(item.Id));

                    var startIndex = queryResult.StartIndex;
                    var totalCount = queryResult.FoundCount;
                    var results = Cache.Get<TweetViewData>(keys);

                    lst.Add(new ResultGroup<TweetViewData>(totalCount, startIndex, results.ToList()) { Value = queryResult.Value });
                }
            }

            return new GroupedResult<TweetViewData>(lst);
        }
    }

    public class TweetViewData : Tweet, IViewData
    {
        public KeyValuePair<string, string> UniqueIdentifier { get { return new KeyValuePair<string, string>("Id", Id); } }
        public string Fullname { get { return "Refrain.Portal.Module.View.TweetViewData"; } }

        public string Country { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            yield return UniqueIdentifier;
            yield return new KeyValuePair<string, string>("Country.Name", Country);
            yield return new KeyValuePair<string, string>("DateCreated", DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
        }
    }
}
