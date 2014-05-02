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
        public void Get_GivenOneWord_CallSolrWithCorrectQuery()
        {
            var extension = new Search(PortalApplication.Object, null);
            var query = "some string";
            PortalApplication.Setup(m => m.ViewManager.GetView("Search")).Returns(View.Object);

            extension.Get(query);

            View.Verify(m => m.Query(It.Is<IQuery>(item =>
                item.Query == "str_Text:(some\\ string*)^5Text:(\"some string\")^2str_Artist.Name:(some\\ string*)^5Artist.Name:(\"some string\")^2str_Country.Name:(some\\ string*)^5Country.Name:(\"some string\")^2Contest.Year:some string")));
        }
    }
}
