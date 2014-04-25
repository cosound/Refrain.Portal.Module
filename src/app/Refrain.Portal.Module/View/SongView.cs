﻿namespace Refrain.Portal.Module.View
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using CHAOS.Serialization;
    using Chaos.Portal.Core.Indexing.View;
    using Object = Chaos.Mcm.Data.Dto.Object;

    public class SongView : AView
    {
        public CoSoundConfiguration Config { get; set; }

        public SongView(CoSoundConfiguration config) : base("Song")
        {
            Config = config;
        }

        public override IList<IViewData> Index(object objectsToIndex)
        {
            var obj = objectsToIndex as Object;

            if( obj == null ) return new List<IViewData>();
            if( obj.ObjectTypeID != Config.ObjectTypes.ManifestationId ) return new List<IViewData>();
           
            return Index(obj).ToList();
        }

        private IEnumerable<IViewData> Index(Object manifest)
        {
            var similarityXml = manifest.Metadatas.FirstOrDefault(item => item.MetadataSchemaGuid == Config.MetadataSchemas.GenericSimilarity);

            if (similarityXml == null || similarityXml.MetadataXml.Root == null) yield break;

            foreach (var similarityElement in similarityXml.MetadataXml.Root.Elements("Similarity"))
            {
                var basicsElement = similarityElement.Element("Basics");
              
                if (basicsElement == null) continue;

                var identifier = basicsElement.Descendants("Identifier").FirstOrDefault();

                if (identifier == null) continue;

                var songSimilarities = new List<SongSimilarity>();

                foreach (var pointElement in similarityElement.Descendants("EndPoints").Descendants("Point"))
                {
                    var guid = pointElement.Element("GUID").Value;
                    var rank = pointElement.Element("Rank").Value;
                    var dist = pointElement.Element("Dist").Value;
                    var relativeImportance = pointElement.Element("RelativeImportanceOfSubspaces").Value;

                    songSimilarities.Add(new SongSimilarity
                        {
                            SongId = new Guid(guid),
                            Rank = uint.Parse(rank),
                            Distance = double.Parse(dist, CultureInfo.InvariantCulture),
                            RelativeImportance = relativeImportance
                        });
                }

                yield return new SongViewData
                {
                    Id = manifest.Guid,
                    Similarity = new Similarity
                        {
                            Type = identifier.Value,
                            Songs = songSimilarities
                        }
                };
            }
        }
    }

    public class SongViewData : IViewData
    {
        [Serialize]
        public Guid Id { get; set; }

        [Serialize]
        public Similarity Similarity { get; set; }

        public SongViewData()
        {
            Similarity = new Similarity();
        }

        public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            throw new System.NotImplementedException();
        }

        public KeyValuePair<string, string> UniqueIdentifier { get { return new KeyValuePair<string, string>("Id", Id.ToString()); } }
        public string Fullname { get { return "Refrain.Portal.Module.Test.View.SongViewData"; } }
    }

    public class Similarity
    {
        [Serialize]
        public string Type { get; set; }

        [Serialize]
        public IList<SongSimilarity> Songs { get; set; }

        public Similarity()
        {
            Songs = new List<SongSimilarity>();
        }
    }

    public class SongSimilarity
    {
        [Serialize]
        public Guid SongId { get; set; }

        [Serialize]
        public uint Rank { get; set; }

        [Serialize]
        public double Distance { get; set; }

        [Serialize]
        public string RelativeImportance { get; set; }
    }
}
