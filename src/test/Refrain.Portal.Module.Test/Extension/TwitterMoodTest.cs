namespace Refrain.Portal.Module.Test.Extension
{
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

            extension.Get(country);

            View.Verify(m => m.GroupedQuery(It.Is<IQuery>(item =>
                item.Query == "*:*" &&
                item.Sort == "DateCreated desc" &&
                item.Group.Field == "Country.Name")));
        }
        
        [Test]
        public void Get_GivenACountry_GetTheCountrysLatestMood()
        {
            PortalApplication.Setup(m => m.ViewManager.GetView("TwitterMood")).Returns(View.Object);
            var extension = new TwitterMood(PortalApplication.Object);
            var country = "country";

            extension.Get(country);

            View.Verify(m => m.GroupedQuery(It.Is<IQuery>(item =>
                item.Query == "Country.Name:country" &&
                item.Sort == "DateCreated desc" &&
                item.Group.Field == "Country.Name")));
        }
        
        [Test]
        public void Get_GivenMultipleCountries_GetTheCountriesLatestMood()
        {
            PortalApplication.Setup(m => m.ViewManager.GetView("TwitterMood")).Returns(View.Object);
            var extension = new TwitterMood(PortalApplication.Object);
            var country = "country1,country2, country3";

            extension.Get(country);

            View.Verify(m => m.GroupedQuery(It.Is<IQuery>(item =>
                item.Query == "Country.Name:country1 country2 country3" &&
                item.Sort == "DateCreated desc" &&
                item.Group.Field == "Country.Name")));
        }
    }
}
