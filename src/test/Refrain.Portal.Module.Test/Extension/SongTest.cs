namespace Refrain.Portal.Module.Test.Extension
{
    using System;
    using Chaos.Portal.Core.Indexing;
    using Module.Extension;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SongTest : TestBase
    {
        [Test]
        public void Get_GivenOneWord_CallSolrWithCorrectQuery()
        {
            var extension = new Song(PortalApplication.Object, null);
            var id = new Guid("10000000-0000-0000-0000-000000000001");
            var type = "111111";
            PortalApplication.Setup(m => m.ViewManager.GetView("Song")).Returns(View.Object);

            extension.Get(id, type);

            View.Verify(m => m.Query(It.Is<IQuery>(item => item.Query == "Id:10000000-0000-0000-0000-000000000001_111111_3")));
        }
    }
}
