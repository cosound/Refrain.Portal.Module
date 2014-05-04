namespace Refrain.Portal.Module.View
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using CHAOS.Serialization;
    using Chaos.Mcm.Data;
    using Chaos.Mcm.Data.Dto;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Indexing;
    using Chaos.Portal.Core.Indexing.View;
    using Data;
    using Data.Model;
    using Exceptions;
    using Object = Chaos.Mcm.Data.Dto.Object;

    public class SongView : AView
    {
        public CoSoundConfiguration Config { get; set; }
        public IMcmRepository McmRepository { get; set; }

        protected IDictionary<Guid, Song> SongCache { get; set; }

        public SongView(CoSoundConfiguration config, IMcmRepository mcmRepository) : base("Song")
        {
            Config = config;
            McmRepository = mcmRepository;
            SongCache = new Dictionary<Guid, Song>();
        }

        public override IPagedResult<IResult> Query(IQuery query)
        {
            return Query<SongViewData>(query);
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

            var song = SongMapper.Create(manifest, Config);

            CacheAllSongs(similarityXml);

            foreach (var similarityElement in similarityXml.MetadataXml.Descendants("Similarity"))
            {
                var basicsElement = similarityElement.Element("Basics");
              
                if (basicsElement == null) continue;

                var name = basicsElement.Element("Name").Value;
                var dataSet = (name.Contains("Spotify") ? DataSet.Spotify : 0) | (name.Contains("ESC") ? DataSet.Eurovision : 0);
                var identifier = basicsElement.Descendants("Identifier").FirstOrDefault();

                if (identifier == null) continue;

                var songSimilarities = (from point in similarityElement.Descendants("EndPoints").Descendants("Point")
                                        select CreateSongSimilarity(point)).ToList();

                yield return new SongViewData
                {
                    Id = manifest.Guid,
                    Title = song.Title,
                    ArtistName = song.ArtistName,
                    CountryName = song.CountryName,
                    Year = song.ContestYear,
                    YoutubeUri = song.YoutubeUri,
                    SpotifyId = song.SpotifyId,
                    DataSet = (uint)dataSet,
                    Similarity = new Similarity
                        {
                            Type = identifier.Value,
                            Songs = songSimilarities,
                        }
                };
            }
        }

        private void CacheAllSongs(Metadata similarityXml)
        {
            var guids =
                similarityXml.MetadataXml.Descendants("EndPoints")
                             .Descendants("Guid")
                             .Select(item => Guid.Parse(item.Value))
                             .Distinct();
            var missingGuids = guids.Where(item => !SongCache.ContainsKey(item));
            IEnumerable<Guid> page;
            for (var i = 0; (page = missingGuids.Skip(i*100).Take(100)).Any(); i++)
            {
                var objects = McmRepository.ObjectGet(page, true);

                try
                {
                    var songs = objects.Select(item => SongMapper.Create(item, Config));

                    foreach (var song in songs.Where(song => !SongCache.ContainsKey(song.Id)))
                    {
                        SongCache.Add(song.Id, song);
                    }
                }
                catch (NotATrackException)
                {
                }
            }
        }

        private SongSimilarity CreateSongSimilarity(XContainer pointElement)
        {
            var songId = new Guid(pointElement.Element("GUID").Value);

            try
            {
                var song = GetSong(songId);

                return new SongSimilarity
                    {
                        SongId = songId,
                        SongTitle = song.Title,
                        ArtistName = song.ArtistName,
                        CountryName = song.CountryName,
                        Year = song.ContestYear,
                        YoutubeUri = song.YoutubeUri,
                        SpotifyId = song.SpotifyId,
                        Rank = uint.Parse(pointElement.Element("Rank").Value), 
                        Distance = double.Parse(pointElement.Element("Dist").Value, CultureInfo.InvariantCulture), 
                        RelativeImportance = pointElement.Element("RelativeImportanceOfSubspaces").Value
                    };
            }
            catch (Exception e)
            {
                throw new Exception(songId.ToString(), e);
            }
        }

        private Song GetSong(Guid songId)
        {
            if (SongCache.ContainsKey(songId)) 
                return SongCache[songId];

            var manifest = McmRepository.ObjectGet(songId, true);
            var song = SongMapper.Create(manifest, Config);

            lock (SongCache)
            {
                if (!SongCache.ContainsKey(songId)) 
                    SongCache.Add(songId, song);
            }

            return song;
        }

        public void WithSongCache(IDictionary<Guid, Song> cache)
        {
            SongCache = cache;
        }
    }

    public class SongViewData : IViewData
    {
        [Serialize]
        public Guid Id { get; set; }

        [Serialize]
        public string Title { get; set; }

        [Serialize]
        public string ArtistName { get; set; }

        [Serialize]
        public string CountryName { get; set; }

        [Serialize]
        public string Year { get; set; }

        [Serialize]
        public string YoutubeUri { get; set; }

        [Serialize]
        public string SpotifyId { get; set; }

        [Serialize]
        public uint DataSet { get; set; }

        [Serialize]
        public Similarity Similarity { get; set; }

        public SongViewData()
        {
            Similarity = new Similarity();
        }

        public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            yield return UniqueIdentifier;
            yield return new KeyValuePair<string, string>("Similarity.Type", Similarity.Type);
            yield return new KeyValuePair<string, string>("DataSet", DataSet.ToString(CultureInfo.InvariantCulture));
        }

        public KeyValuePair<string, string> UniqueIdentifier { get { return new KeyValuePair<string, string>("Id", Id.ToString() + "_" + Similarity.Type); } }
        public string Fullname { get { return "Refrain.Portal.Module.Test.View.SongViewData"; } }
    }

    [Flags]
    public enum DataSet : uint
    {
        Eurovision = 1 << 0,
        Spotify = 1 << 1
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
        public string SongTitle { get; set; }

        [Serialize]
        public string ArtistName { get; set; }

        [Serialize]
        public string CountryName { get; set; }

        [Serialize]
        public string Year { get; set; }

        [Serialize]
        public string YoutubeUri { get; set; }

        [Serialize]
        public string SpotifyId { get; set; }

        [Serialize]
        public uint Rank { get; set; }

        [Serialize]
        public double Distance { get; set; }

        [Serialize]
        public string RelativeImportance { get; set; }
    }
}
