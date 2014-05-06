namespace Refrain.Portal.Module.Test.Extension
{
    using Chaos.Portal.Core.Indexing;
    using Module.Extension;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SearchTest : TestBase
    {
        [Test]
        public void Get_GivenTwoWords_CallSolrWithCorrectQuery()
        {
            var extension = new Search(PortalApplication.Object, null);
            var query = "some string";
            PortalApplication.Setup(m => m.ViewManager.GetView("Search")).Returns(View.Object);

            extension.Get(query);

            var actualQuery = "str_Text:(some\\ string*)^25Text:(some string*)^15str_Artist.Name:(some\\ string*)^15Artist.Name:(some string*)^5str_Country.Name:(some\\ string*)^5Country.Name:(some string*)";
            View.Verify(m => m.Query(It.Is<IQuery>(item =>
                item.Query == actualQuery && 
                item.Filter == "")));
        }

        [Test]
        public void Get_GivenYear_CallSolr()
        {
            var extension = new Search(PortalApplication.Object, null);
            var query = "2000";
            PortalApplication.Setup(m => m.ViewManager.GetView("Search")).Returns(View.Object);

            extension.Get(query);

            var actual = "Contest.Year:2000";
            View.Verify(m => m.Query(It.Is<IQuery>(item =>
                item.Query == "*:*" &&
                item.Filter == actual)));
        }

        [Test]
        public void Get_GivenYearAndArtist_CallSolrWithCorrectQuery()
        {
            var extension = new Search(PortalApplication.Object, null);
            var query = "2014 basim";
            PortalApplication.Setup(m => m.ViewManager.GetView("Search")).Returns(View.Object);

            extension.Get(query);

            var actualQuery = "str_Text:(2014\\ basim*)^25Text:(2014 basim*)^15str_Artist.Name:(2014\\ basim*)^15Artist.Name:(2014 basim*)^5str_Country.Name:(2014\\ basim*)^5Country.Name:(2014 basim*)";
            var actualFilter = "Contest.Year:2014";
            View.Verify(m => m.Query(It.Is<IQuery>(item =>
                item.Query == actualQuery &&
                item.Filter == actualFilter)));
        }
    }
}
