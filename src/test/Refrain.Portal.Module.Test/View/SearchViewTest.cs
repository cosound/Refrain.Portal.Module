namespace Refrain.Portal.Module.Test.View
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Chaos.Mcm.Data.Dto;
    using Module.View;
    using NUnit.Framework;
    using Object = Chaos.Mcm.Data.Dto.Object;

    [TestFixture]
    public class SearchViewTest
    {
        protected Object AudioTrackObject { get; set; }

        [TestFixtureSetUp]
        public void SetupFixture()
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

        [Test]
        public void Index_Track_PropertiesAreSet()
        {
            var view = Make_SearchView();

            var result = view.Index(AudioTrackObject).FirstOrDefault() as SearchViewData;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(new Guid("bb8dd249-c192-405b-8a86-b3fb5ca49819")));
            Assert.That(result.Text, Is.EqualTo("Amours mortes (tant de peine)"));
        }

        private SearchView Make_SearchView()
        {
            return new SearchView(Make_Config());
        }

        protected CoSoundConfiguration Make_Config()
        {
            return new CoSoundConfiguration();
        }
    }
}
