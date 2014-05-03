namespace Refrain.Portal.Module.View
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using CHAOS.Serialization;
    using Chaos.Portal.Core.Indexing.View;

    public class TwitterMoodView : AView
    {
        public CoSoundConfiguration Config { get; set; }

        public TwitterMoodView(CoSoundConfiguration config) : base("Twitter")
        {
            Config = config;
        }

        public override IList<IViewData> Index(object objectsToIndex)
        {
            var obj = objectsToIndex as Chaos.Mcm.Data.Dto.Object;

            if(obj == null) return new List<IViewData>();

            var metadata = obj.Metadatas.FirstOrDefault(item => item.MetadataSchemaGuid == Config.MetadataSchemas.StateMood);

            if(metadata == null) return new List<IViewData>();

            var valence = metadata.MetadataXml.Descendants("ResultSummary").Descendants("Result").FirstOrDefault(r => r.Element("Type").Value == "Valence").Element("Value").Value;
            var country = metadata.MetadataXml.Descendants("Actor").Descendants("Main").First().Value;

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
