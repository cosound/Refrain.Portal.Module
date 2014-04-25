namespace Refrain.Portal.Module.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CHAOS.Serialization;
    using Chaos.Portal.Core.Indexing.View;
    using Object = Chaos.Mcm.Data.Dto.Object;

    public class SearchView : AView
    {
        private CoSoundConfiguration Config { get; set; }

        public SearchView(CoSoundConfiguration config) : base("Search")
        {
            Config = config;
        }

        public override IList<IViewData> Index(object objectsToIndex)
        {
            var obj = objectsToIndex as Object;

            if (obj == null) return new List<IViewData>();
            if (obj.ObjectTypeID != Config.ObjectTypes.ManifestationId) return new List<IViewData>();

            return Index(obj).ToList();
        }

        private IEnumerable<IViewData> Index(Object manifest)
        {
            var trackXml = manifest.Metadatas.FirstOrDefault(item => item.MetadataSchemaGuid == Config.MetadataSchemas.AudioMusicTrack);

            if(trackXml == null) yield break;

            var mainTitle = trackXml.MetadataXml.Root.Element("Titles").Element("Main").Element("Title").Value;

            yield return new SearchViewData
                {
                    Text = mainTitle
                };
        }
    }

    public class SearchViewData : IViewData
    {
        [Serialize]
        public Guid Id { get; set; }

        [Serialize]
        public string Text { get; set; }

        public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            throw new System.NotImplementedException();
        }

        public KeyValuePair<string, string> UniqueIdentifier { get {return new KeyValuePair<string, string>("Id", Id.ToString());} }
        public string Fullname { get { return "Refrain.Portal.Module.View.SearchViewData"; } }
    }
}
