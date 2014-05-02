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
                    Title = GetMainTitle(trackXml),
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

        private static string GetMainTitle(Metadata trackXml)
        {
            var titleElement = trackXml.MetadataXml.Descendants("Titles")
                                                   .Descendants("Main")
                                                   .Descendants("Title").FirstOrDefault();
            
            return titleElement == null ? null : titleElement.Value;
        }
    }
}
