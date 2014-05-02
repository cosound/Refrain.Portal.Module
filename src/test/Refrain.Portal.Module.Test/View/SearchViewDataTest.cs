namespace Refrain.Portal.Module.Test.View
{
    using System;
    using System.Linq;
    using Module.View;
    using NUnit.Framework;

    [TestFixture]
    public class SearchViewDataTest
    {
        [Test]
        public void GetIndexableFields_IdIsSet_ReturnIdField()
        {
            var data = Make_SearchViewData();

            var fields = data.GetIndexableFields().ToList();

            Assert.That(fields[0].Key, Is.EqualTo("Id"));
            Assert.That(fields[0].Value, Is.EqualTo("10000000-0000-0000-0000-000000000001"));
        }
        
        [Test]
        public void GetIndexableFields_TextIsSet_ReturnTextField()
        {
            var data = Make_SearchViewData();

            var fields = data.GetIndexableFields().ToList();

            Assert.That(fields[1].Key, Is.EqualTo("Text"));
            Assert.That(fields[1].Value, Is.EqualTo("text"));
        }

        [Test]
        public void GetIndexableFields_ArtistIsSet_ReturnArtistField()
        {
            var data = Make_SearchViewData();

            var fields = data.GetIndexableFields().ToList();

            Assert.That(fields[2].Key, Is.EqualTo("Artist.Name"));
            Assert.That(fields[2].Value, Is.EqualTo("name"));
        }
        
        [Test]
        public void GetIndexableFields_CountryIsSet_ReturnCountryNameField()
        {
            var data = Make_SearchViewData();

            var fields = data.GetIndexableFields().ToList();

            Assert.That(fields[3].Key, Is.EqualTo("Country.Name"));
            Assert.That(fields[3].Value, Is.EqualTo("countryname"));
        }
        
        [Test]
        public void GetIndexableFields_ContestYearIsSet_ReturnContestYearField()
        {
            var data = Make_SearchViewData();

            var fields = data.GetIndexableFields().ToList();

            Assert.That(fields[4].Key, Is.EqualTo("Contest.Year"));
            Assert.That(fields[4].Value, Is.EqualTo("1957"));
        }

        private static SearchViewData Make_SearchViewData()
        {
            var data = new SearchViewData
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000001"),
                    Text = "text",
                    ArtistName = "name",
                    CountryName = "countryname",
                    ContestYear = "1957"
                };
            return data;
        }
    }
}
