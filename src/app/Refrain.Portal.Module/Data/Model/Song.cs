namespace Refrain.Portal.Module.Data.Model
{
    using System;

    public class Song
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
        public string CountryName { get; set; }
        public string ContestYear { get; set; }
        public string YoutubeUri { get; set; }
        public string SpotifyId { get; set; }
        public bool IsEurovisionTrack { get; set; }
    }
}
