namespace Refrain.Portal.Module.Data.Model
{
    using System;

    public class Song
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsEurovisionTrack { get; set; }
    }
}
