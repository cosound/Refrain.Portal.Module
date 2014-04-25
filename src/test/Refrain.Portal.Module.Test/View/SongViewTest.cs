namespace Refrain.Portal.Module.Test.View
{
    using System;
    using System.Linq;
    using Module.View;
    using NUnit.Framework;

    [TestFixture]
    public class SongViewTest : TestBase
    {
        [Test]
        public void Index_GivenTrackAndSimilarity_MapValuesToViewData()
        {
            var view = Make_SongView();

            var result = view.Index(AudioTrackObject).FirstOrDefault() as SongViewData;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(new Guid("bb8dd249-c192-405b-8a86-b3fb5ca49819")));
            Assert.That(result.Similarity.Type, Is.EqualTo("000001"));
            Assert.That(result.Similarity.Songs[0].SongId, Is.EqualTo(new Guid("fe5ce389-1581-4fe3-91cf-bdcc465d68d0")));
            Assert.That(result.Similarity.Songs[0].Rank, Is.EqualTo(1));
            Assert.That(result.Similarity.Songs[0].Distance, Is.EqualTo(0.06));
            Assert.That(result.Similarity.Songs[0].RelativeImportance, Is.EqualTo("0 0 0 0 0 0.02"));
        }

        private SongView Make_SongView()
        {
            return new SongView(Make_Config());
        }
    }
}
