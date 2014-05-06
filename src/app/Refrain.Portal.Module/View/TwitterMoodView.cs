namespace Refrain.Portal.Module.View
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Indexing;
    using Chaos.Portal.Core.Indexing.View;
    using Domain;
    using Domain.Dto;
    using Exceptions;

    public class TwitterMoodView : AView
    {
        public CoSoundConfiguration Config { get; set; }

        public TwitterMoodView(CoSoundConfiguration config) : base("TwitterMood")
        {
            Config = config;
        }

        public override IList<IViewData> Index(object objectsToIndex)
        {
            var obj = objectsToIndex as Chaos.Mcm.Data.Dto.Object;

            try
            {
                var twitterMood = TwitterMoodMapper.Create(obj, Config);

                return new[]
                    { 
                        new TwitterMoodViewData
                            {
                                Id = twitterMood.Id,
                                Valence = twitterMood.Valence,
                                Country = twitterMood.Country,
                                DateCreated = twitterMood.DateCreated
                            }
                    };
            }
            catch (CannotMapException e)
            {
                return new List<IViewData>();
            }
        }

        public override IGroupedResult<IResult> GroupedQuery(IQuery query)
        {
            var response = Core.Query(query);

            var lst = new List<ResultGroup<TwitterMoodViewData>>();

            foreach (var queryResultGroup in response.QueryResultGroups)
            {
                foreach (var queryResult in queryResultGroup.Groups)
                {
                    var keys = queryResult.Results.Select(item => CreateKey(item.Id));

                    var startIndex = queryResult.StartIndex;
                    var totalCount = queryResult.FoundCount;
                    var results = Cache.Get<TwitterMoodViewData>(keys);

                    lst.Add(new ResultGroup<TwitterMoodViewData>(totalCount, startIndex, results.ToList()) { Value = queryResult.Value });
                }
            }

            return new GroupedResult<TwitterMoodViewData>(lst);
        }
    }

    public class TwitterMoodViewData : TwitterMood, IViewData
    {
        public KeyValuePair<string, string> UniqueIdentifier { get { return new KeyValuePair<string, string>("Id", Id.ToString()); } }
        public string Fullname { get { return "Refrain.Portal.Module.View.TwitterMoodViewData"; } }

        public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            yield return UniqueIdentifier;
            yield return new KeyValuePair<string, string>("Country.Name", Country);
            yield return new KeyValuePair<string, string>("Valence", Valence.ToString(CultureInfo.InvariantCulture));
            yield return new KeyValuePair<string, string>("DateCreated", DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
        }
    }
}
