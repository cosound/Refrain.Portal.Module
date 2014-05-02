namespace Refrain.Portal.Module.Test.View
{
    using System;
    using System.Linq;
    using Module.View;
    using NUnit.Framework;

    [TestFixture]
    public class SearchViewTest : TestBase
    {
        [Test]
        public void Index_Track_PropertiesAreSet()
        {
            var view = Make_SearchView();

            var result = view.Index(AudioTrackObject).FirstOrDefault() as SearchViewData;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(new Guid("bb8dd249-c192-405b-8a86-b3fb5ca49819")));
            Assert.That(result.Text, Is.EqualTo("Amours mortes (tant de peine)"));
            Assert.That(result.ArtistName, Is.EqualTo("Danièle Dupré"));
            Assert.That(result.CountryName, Is.EqualTo("Luxembourg"));
            Assert.That(result.ContestYear, Is.EqualTo("1957"));
        }



        protected SearchView Make_SearchView()
        {
            return new SearchView(Make_Config());
        }
    }
}
