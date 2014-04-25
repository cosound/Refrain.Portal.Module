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
        }

        private SongView Make_SongView()
        {
            return new SongView(Make_Config());
        }
    }

    
}
