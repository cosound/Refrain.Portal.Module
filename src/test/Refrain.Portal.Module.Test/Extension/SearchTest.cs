﻿namespace Refrain.Portal.Module.Test.Extension
{
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Indexing;
    using Chaos.Portal.Core.Indexing.View;
    using Module.Extension;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SearchTest
    {
        private Mock<IPortalApplication> PortalApplication { get; set; }
        private Mock<IView> View { get; set; }

        [SetUp]
        public void SetUp()
        {
            PortalApplication = new Mock<IPortalApplication>();
            View = new Mock<IView>();
        }

        [Test]
        public void Get_GivenOneWord_CallSolrWithCorrectQuery()
        {
            var extension = new Search(PortalApplication.Object, null);
            var query = "somestring";
            PortalApplication.Setup(m => m.ViewManager.GetView("Search")).Returns(View.Object);

            extension.Get(query);

            View.Verify(m => m.Query(It.Is<IQuery>(item => item.Query == "(Text:\"somestring\"^5)(Text:(somestring)^2)(Text:somestring*)")));
        }
    }
}
