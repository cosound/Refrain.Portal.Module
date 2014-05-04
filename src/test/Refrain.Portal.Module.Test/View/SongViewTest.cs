namespace Refrain.Portal.Module.Test.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Module.View;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SongViewTest : TestBase
    {
        [Test]
        public void Index_GivenTrackAndSimilarity_MapValuesToViewData()
        {
            var view = Make_SongView();
            var song = Make_SongObject();
            McmRepository.Setup(m => m.ObjectGet(It.IsAny<IEnumerable<Guid>>(), true, false, false, false, false)).Returns(new[] { song });
            McmRepository.Setup(m => m.ObjectGet(It.IsAny<Guid>(), true, false, false, false, false)).Returns(song);

            var result = view.Index(AudioTrackObject).FirstOrDefault() as SongViewData;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(new Guid("bb8dd249-c192-405b-8a86-b3fb5ca49819")));
            Assert.That(result.Title, Is.EqualTo("Amours mortes (tant de peine)"));
            Assert.That(result.ArtistName, Is.EqualTo("Danièle Dupré"));
            Assert.That(result.CountryName, Is.EqualTo("Luxembourg"));
            Assert.That(result.Year, Is.EqualTo("1957"));
            Assert.That(result.DataSet, Is.EqualTo(2));
            Assert.That(result.YoutubeUri, Is.EqualTo("http://www.youtube.com/watch?v=-0nMxZ8L1cI"));
            Assert.That(result.SpotifyId, Is.EqualTo("3g4Pmtk3Up0NCYRFqcEPsQ"));
            Assert.That(result.Similarity.Type, Is.EqualTo("000001"));
            Assert.That(result.Similarity.Songs[0].SongId, Is.EqualTo(new Guid("fe5ce389-1581-4fe3-91cf-bdcc465d68d0")));
            Assert.That(result.Similarity.Songs[0].SongTitle, Is.EqualTo("Hou toch van mij"));
            Assert.That(result.Similarity.Songs[0].ArtistName, Is.EqualTo("Bob Benny"));
            Assert.That(result.Similarity.Songs[0].CountryName, Is.EqualTo("Belgium"));
            Assert.That(result.Similarity.Songs[0].Year, Is.EqualTo("1959"));
            Assert.That(result.Similarity.Songs[0].YoutubeUri, Is.EqualTo("http://www.youtube.com/watch?v=JvbH5oGZR6M"));
            Assert.That(result.Similarity.Songs[0].SpotifyId, Is.EqualTo("2y9PZQQSbjzjQ0zBIXTWY7"));
            Assert.That(result.Similarity.Songs[0].Rank, Is.EqualTo(1));
            Assert.That(result.Similarity.Songs[0].Distance, Is.EqualTo(0.06));
            Assert.That(result.Similarity.Songs[0].RelativeImportance, Is.EqualTo("0 0 0 0 0 0.02"));
        }

        private SongView Make_SongView()
        {
            return new SongView(Make_Config(), McmRepository.Object);
        }
    }
}
