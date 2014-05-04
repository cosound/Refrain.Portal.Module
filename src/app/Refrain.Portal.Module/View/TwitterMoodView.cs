namespace Refrain.Portal.Module.View
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using CHAOS.Serialization;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Indexing;
    using Chaos.Portal.Core.Indexing.View;

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

            if (obj == null || obj.ObjectTypeID != Config.ObjectTypes.ActorContent) return new List<IViewData>();

            var metadata = obj.Metadatas.FirstOrDefault(item => item.MetadataSchemaGuid == Config.MetadataSchemas.StateMood);

            if(metadata == null) return new List<IViewData>();

            var firstOrDefault = metadata.MetadataXml.Descendants("ResultSummary").Descendants("Result").FirstOrDefault(r =>
                {
                    var xElement = r.Element("Type");
                    return xElement != null && xElement.Value == "Valence";
                });
            if (firstOrDefault == null) return new List<IViewData>();

            var element = firstOrDefault.Element("Value");
            if (element == null) return new List<IViewData>();
             
            var valence = element.Value;

            var orDefault = metadata.MetadataXml.Descendants("Actor").Descendants("Main").FirstOrDefault();
            if (orDefault == null) return new List<IViewData>(); 
            
            var country = orDefault.Value;

            return new[]
                {
                    new TwitterMoodViewData
                        {
                            Id = obj.Guid,
                            Valence = double.Parse(valence, CultureInfo.InvariantCulture),
                            Country = country,
                            DateCreated = obj.DateCreated
                        }
                };
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

    public class TwitterMoodViewData : IViewData
    {

        [Serialize]
        public Guid Id { get; set; }
        
        [Serialize]
        public string Country { get; set; }

        [Serialize]
        public double Valence { get; set; }

        [Serialize]
        public DateTime DateCreated { get; set; }

        public KeyValuePair<string, string> UniqueIdentifier { get { return new KeyValuePair<string, string>("Id", Id.ToString()); } }
        public string Fullname { get { return "Refrain.Portal.Module.View.TwitterMoodViewData"; } }

        public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            yield return UniqueIdentifier;
            yield return new KeyValuePair<string, string>("Country.Name", Country);
            yield return new KeyValuePair<string, string>("Valence", Valence.ToString(CultureInfo.InvariantCulture));
            yield return new KeyValuePair<string, string>("DateCreated", DateCreated.ToString("yyyy-MM-dd'T'hh:mm:ss'Z'"));
        }
    }
}
