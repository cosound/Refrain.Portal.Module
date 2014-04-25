namespace Refrain.Portal.Module.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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


            yield return new SongViewData
                {
                    Id = manifest.Guid
                };
        }
    }

    public class SongViewData : IViewData
    {
        [Serialize]
        public Guid Id { get; set; }

        public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            throw new System.NotImplementedException();
        }

        public KeyValuePair<string, string> UniqueIdentifier { get { return new KeyValuePair<string, string>("Id", Id.ToString()); } }
        public string Fullname { get { return "Refrain.Portal.Module.Test.View.SongViewData"; } }
    }
}
