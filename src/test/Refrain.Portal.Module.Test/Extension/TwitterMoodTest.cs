namespace Refrain.Portal.Module.Test.Extension
{
    using System;
    using Chaos.Portal.Core.Indexing;
    using Module.Extension;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class TwitterMoodTest : TestBase
    {
        [Test]
        public void Get_GivenNoCountry_GetAllCountriesLatestMood()
        {
            PortalApplication.Setup(m => m.ViewManager.GetView("TwitterMood")).Returns(View.Object);
            var extension = new TwitterMood(PortalApplication.Object);
            var country = "";

            extension.Get(country, null, null);

            View.Verify(m => m.GroupedQuery(It.Is<IQuery>(item =>
                item.Query == "*:*" &&
                item.Sort == "DateCreated asc" &&
                item.Group.Field == "Country.Name")));
        }
        
        [Test]
        public void Get_GivenACountry_GetTheCountrysLatestMood()
        {
            PortalApplication.Setup(m => m.ViewManager.GetView("TwitterMood")).Returns(View.Object);
            var extension = new TwitterMood(PortalApplication.Object);
            var country = "country";

            extension.Get(country, null, null);

            View.Verify(m => m.GroupedQuery(It.Is<IQuery>(item =>
                item.Query == "Country.Name:country" &&
                item.Sort == "DateCreated asc" &&
                item.Group.Field == "Country.Name")));
        }
        
        [Test]
        public void Get_GivenMultipleCountries_GetTheCountriesLatestMood()
        {
            PortalApplication.Setup(m => m.ViewManager.GetView("TwitterMood")).Returns(View.Object);
            var extension = new TwitterMood(PortalApplication.Object);
            var country = "country1,country2, country3";

            extension.Get(country, null, null);

            View.Verify(m => m.GroupedQuery(It.Is<IQuery>(item =>
                item.Query == "Country.Name:country1 country2 country3" &&
                item.Sort == "DateCreated asc" &&
                item.Group.Field == "Country.Name")));
        }
        
        [Test]
        public void Get_GivenBeforeDateTime_GetTheMoodStateAtThatTime()
        {
            PortalApplication.Setup(m => m.ViewManager.GetView("TwitterMood")).Returns(View.Object);
            var extension = new TwitterMood(PortalApplication.Object);
            var country = "";
            var before = new DateTime(2000, 01, 01, 17, 00, 00);

            extension.Get(country, before, null);

            View.Verify(m => m.GroupedQuery(It.Is<IQuery>(item =>
                item.Query == "*:*" &&
                item.Filter == "DateCreated:[* TO 2000-01-01T17:00:00Z]" &&
                item.Sort == "DateCreated asc" &&
                item.Group.Field == "Country.Name")));
        }
        
        [Test]
        public void Get_GivenBeforeAndAfterDateTime_GetTheMoodStateBetweenDates()
        {
            PortalApplication.Setup(m => m.ViewManager.GetView("TwitterMood")).Returns(View.Object);
            var extension = new TwitterMood(PortalApplication.Object);
            var country = "";
            var before = new DateTime(2001, 01, 01, 17, 00, 00);
            var after = new DateTime(2000, 01, 01, 17, 00, 00);

            extension.Get(country, before, after);

            View.Verify(m => m.GroupedQuery(It.Is<IQuery>(item =>
                item.Query == "*:*" &&
                item.Filter == "DateCreated:[2000-01-01T17:00:00Z TO 2001-01-01T17:00:00Z]" &&
                item.Sort == "DateCreated asc" &&
                item.Group.Field == "Country.Name")));
        }
        
        [Test]
        public void Get_GivenAfterDateTime_GetTheMoodStateBetweenNowAndTheAfterDate()
        {
            PortalApplication.Setup(m => m.ViewManager.GetView("TwitterMood")).Returns(View.Object);
            var extension = new TwitterMood(PortalApplication.Object);
            var country = "";
            var after = new DateTime(2000, 01, 01, 17, 00, 00);

            extension.Get(country, null, after);

            View.Verify(m => m.GroupedQuery(It.Is<IQuery>(item =>
                item.Query == "*:*" &&
                item.Filter == "DateCreated:[2000-01-01T17:00:00Z TO *]" &&
                item.Sort == "DateCreated asc" &&
                item.Group.Field == "Country.Name")));
        }
    }
}
