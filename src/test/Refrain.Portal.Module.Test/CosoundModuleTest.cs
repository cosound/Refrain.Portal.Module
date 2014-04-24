namespace Refrain.Portal.Module.Test
{
    using System.Linq;
    using Chaos.Portal.Core;
    using Extension;

    [TestFixture]
    public class CosoundModuleTest
    {
        [Test]
        public void GetExtensionNames_Latest_ReturnAll()
        {
            var module = Make_CosoundModule();

            var results = module.GetExtensionNames(Protocol.Latest).ToList();

            Assert.That(results.Contains("Album"), Is.True);
        }

        [Test]
        public void GetExtension_LatestAlbum_ReturnExtension()
        {
            var module = Make_CosoundModule();

            var result = module.GetExtension(Protocol.Latest, "Album");

            Assert.That(result, Is.InstanceOf<Album>());
        }

        private static CosoundModule Make_CosoundModule()
        {
            return new CosoundModule();
        }
    }
}
