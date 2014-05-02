namespace Refrain.Portal.Module.Data
{
    using System.Linq;
    using System.Xml.Linq;
    using Chaos.Mcm.Data.Dto;
    using Exceptions;
    using Model;

    public class SongMapper
    {
        public static Song Create(Object manifest, CoSoundConfiguration config)
        {
            var trackXml = manifest.Metadatas.FirstOrDefault(item => item.MetadataSchemaGuid == config.MetadataSchemas.AudioMusicTrack);

            if (trackXml == null) throw new NotATrackException("trackXml == null");

            return new Song
                {
                    Id = manifest.Guid,
                    YoutubeUri = GetYouTubeUri(trackXml),
                    SpotifyId = GetSpotifyId(trackXml),
                    Title = GetMainTitle(trackXml),
                    ArtistName = GetArtistName(trackXml),
                    CountryName = GetCountryName(trackXml),
                    ContestYear = GeContestYear(trackXml),
                    IsEurovisionTrack = IsEurovisionTrack(trackXml)
                };
        }

        private static bool IsEurovisionTrack(Metadata trackXml)
        {
            var source = trackXml.MetadataXml.Descendants("Source");

            return source.Any(element =>
            {
                var xElement = element.Element("ID");
                return xElement != null && xElement.Value == "DTU:ESCGigaCollection";
            });
        }

        private static string GetYouTubeUri(Metadata trackXml)
        {
            var linkElement = trackXml.MetadataXml.Descendants("ForeignReference")
                                                  .Where(item => item.Element("Type").Value == "Youtube")
                                                  .Elements("URI")
                                                  .FirstOrDefault();

            return linkElement == null ? null : linkElement.Value;
        }
        
        private static string GetSpotifyId(Metadata trackXml)
        {
            var Id = trackXml.MetadataXml.Descendants("ForeignReference")
                                                  .Where(item => item.Element("Type").Value == "Spotify")
                                                  .Elements("Id")
                                                  .FirstOrDefault();

            return Id == null ? null : Id.Value;
        }

        private static string GetArtistName(Metadata trackXml)
        {
            var val = trackXml.MetadataXml.Descendants("Artist")
                                          .Descendants("Main")
                                          .Elements("Name")
                                          .FirstOrDefault();

            return val == null ? null : val.Value;
        }

        private static string GetCountryName(Metadata trackXml)
        {
            var val = trackXml.MetadataXml.Descendants("Location")
                                          .Descendants("Main")
                                          .Elements("Name")
                                          .FirstOrDefault();

            return val == null ? null : val.Value;
        }
        
        private static string GeContestYear(Metadata trackXml)
        {
            var val = trackXml.MetadataXml.Descendants("Timeline")
                                          .Where(item => item.Element("Description") != null && item.Element("Description").Value == "Event:Apperance")
                                          .Descendants("AtYear")
                                          .FirstOrDefault();

            return val == null ? null : val.Value;
        }

        private static string GetMainTitle(Metadata trackXml)
        {
            var titleElement = trackXml.MetadataXml.Descendants("Titles")
                                                   .Descendants("Main")
                                                   .Descendants("Title").FirstOrDefault();
            
            return titleElement == null ? null : titleElement.Value;
        }
    }
}
