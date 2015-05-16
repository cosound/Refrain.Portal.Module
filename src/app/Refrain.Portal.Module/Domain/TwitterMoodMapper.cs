namespace Refrain.Portal.Module.Domain
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Chaos.Mcm.Data.Dto;
    using Dto;
    using Exceptions;
    using Object = Chaos.Mcm.Data.Dto.Object;

    public class TwitterMoodMapper
    {
        public static TwitterMood Create(Object obj, CoSoundConfiguration config)
        {
            if (obj.ObjectTypeID != config.ObjectTypes.ActorContent)
                throw new CannotMapException(string.Format("Incorrect object type, was {0} expected {1}", obj.ObjectTypeID, config.ObjectTypes.ActorContent));

            var metadata = obj.Metadatas.FirstOrDefault(item => item.MetadataSchemaGuid == config.MetadataSchemas.StateMood);

            if (metadata == null)
                throw new CannotMapException(string.Format("Twitter Mood Metadata Schema not found one object {0}",  obj.Guid));

            var datetime = metadata.MetadataXml.Descendants("Timeline").Elements("StartAtDateTime").FirstOrDefault();
            if(datetime == null) throw new CannotMapException("CreatedDateTime not found in metadata");

            var resultSummeryElement = metadata.MetadataXml.Descendants("ResultSummary").Descendants("Result").FirstOrDefault(r =>
                {
                    var typeElement = r.Element("Type");
                    return typeElement != null && typeElement.Value == "Valence";
                });
            if (resultSummeryElement == null)
                throw new CannotMapException(string.Format("Could not find ResultSummary in metadata {0}", metadata.Guid)); ;

            var valenceElement = resultSummeryElement.Element("Value");
            if (valenceElement == null)
                throw new CannotMapException(string.Format("Could not find Valence in metadata {0}", metadata.Guid));

            var valence = valenceElement.Value;

            var countryElement = metadata.MetadataXml.Descendants("Actor").Descendants("Main").FirstOrDefault();
            if (countryElement == null)
                throw new CannotMapException(string.Format("Twitter Mood Metadata Schema not found one object {0}", metadata.Guid));
            
            var country = countryElement.Value;

            var tweetElements = metadata.MetadataXml.Descendants("ForeignReference").Where(item =>
                {
                    var typeElement = item.Element("Type");
                    var idElement = item.Element("Identity");
                    var embedElement = item.Element("EmbeddingCode");
                    return typeElement != null && typeElement.Value == "Data:Twitter:Tweet" &&
                           idElement != null && !string.IsNullOrEmpty(idElement.Value) &&
                           embedElement != null && !string.IsNullOrEmpty(embedElement.Value);
                });

            return new TwitterMood
                {
                    Identity = obj.Guid,
                    Valence = double.Parse(valence, CultureInfo.InvariantCulture),
                    Country = country,
                    Tweets = tweetElements.Select(item => new Tweet
                        {
                            EmbedCode = item.Element("EmbeddingCode").Value, 
                            Identity = item.Element("Identity").Value
                        }).ToList(),
                    DateCreated = DateTime.ParseExact(datetime.Value, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture)
                };
        }
    }
}
