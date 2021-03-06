﻿namespace Refrain.Portal.Module.Test.View
{
    using System;
    using System.Xml.Linq;
    using Chaos.Mcm.Data;
    using Chaos.Mcm.Data.Dto;
    using Moq;
    using NUnit.Framework;
    using Object = Chaos.Mcm.Data.Dto.Object;

    [TestFixture]
    public class TestBase
    {
        protected Mock<IMcmRepository> McmRepository { get; set; }
        protected Object AudioTrackObject { get; set; }

        protected CoSoundConfiguration Make_Config()
        {
            return new CoSoundConfiguration();
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var config = Make_Config();
            var track = XDocument.Load("View/AudioMusicTrack_bb8dd249c192405b8a86b3fb5ca49819_Amours_mortes_(tant_de_peine).xml");
            var similarity = XDocument.Load("View/GenericSimilarity_bb8dd249c192405b8a86b3fb5ca49819_Amours_mortes_(tant_de_peine).xml");

            var obj = new Object
                {
                    Guid = new Guid("bb8dd249-c192-405b-8a86-b3fb5ca49819"),
                    ObjectTypeID = config.ObjectTypes.ManifestationId,
                    Metadatas = new []
                        {
                            new Metadata
                                {
                                    MetadataSchemaGuid = config.MetadataSchemas.AudioMusicTrack,
                                    MetadataXml = track
                                },
                            new Metadata
                                {
                                    MetadataSchemaGuid = config.MetadataSchemas.GenericSimilarity,
                                    MetadataXml = similarity
                                }
                        }
                };

            AudioTrackObject = obj;
        }

        [SetUp]
        public void SetUp()
        {
            McmRepository = new Mock<IMcmRepository>();
        }

        protected Object Make_SongObject()
        {
            var config = Make_Config();
            var track = XDocument.Load("View/AudioMusicTrack_fe5ce38915814fe391cfbdcc465d68d0_Hou_toch_van_mij.xml");

            return new Object
                {
                    Guid = new Guid("fe5ce389-1581-4fe3-91cf-bdcc465d68d0"),
                    ObjectTypeID = config.ObjectTypes.ManifestationId,
                    Metadatas = new []
                        {
                            new Metadata
                                {
                                    MetadataSchemaGuid = config.MetadataSchemas.AudioMusicTrack,
                                    MetadataXml = track
                                }
                        }
                };
        }
        
        protected Object Make_TwitterMoodObject()
        {
            var config = Make_Config();
            var mood = XDocument.Load("View/ActorContent_18c323ca-d7c4-4493-8ad6-3e64d881c8e9.xml");

            return new Object
                {
                    Guid = new Guid("18c323ca-d7c4-4493-8ad6-3e64d881c8e9"),
                    ObjectTypeID = config.ObjectTypes.ActorContent,
                    DateCreated = new DateTime(2014,01,01),
                    Metadatas = new []
                        {
                            new Metadata
                                {
                                    MetadataSchemaGuid = config.MetadataSchemas.StateMood,
                                    MetadataXml = mood
                                }
                        }
                };
        }
    }
}
