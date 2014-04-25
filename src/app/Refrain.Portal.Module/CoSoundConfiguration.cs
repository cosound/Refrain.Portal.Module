﻿namespace Refrain.Portal.Module
{
    using System;

    public class CoSoundConfiguration
    {
        public ObjectTypes ObjectTypes { get; set; }
        public MetadataSchemas MetadataSchemas { get; set; }

        public CoSoundConfiguration()
        {
            ObjectTypes = new ObjectTypes();
            MetadataSchemas = new MetadataSchemas();
        }
    }

    public class MetadataSchemas
    {
        public Guid AudioMusicTrack { get { return new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFF120151"); } }
        public Guid GenericSimilarity { get { return new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFF120800"); } }
    }

    public class ObjectTypes
    {
        public uint ManifestationId { get { return 120; } }
    }
}
