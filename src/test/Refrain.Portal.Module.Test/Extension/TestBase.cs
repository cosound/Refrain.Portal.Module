namespace Refrain.Portal.Module.Test.Extension
{
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Indexing.View;
    using Moq;
    using NUnit.Framework;

    public class TestBase
    {
        protected Mock<IPortalApplication> PortalApplication { get; set; }
        protected Mock<IView> View { get; set; }

        [SetUp]
        public void SetUp()
        {
            PortalApplication = new Mock<IPortalApplication>();
            View = new Mock<IView>();
        }
    }
}
