namespace Refrain.Portal.Module.Test
{
    using System.Linq;
    using Chaos.Portal.Core;
    using Module.Extension;
    using NUnit.Framework;

    [TestFixture]
    public class CosoundModuleTest
    {
        [Test]
        public void GetExtensionNames_Latest_ReturnAll()
        {
            var module = Make_CosoundModule();

            var results = module.GetExtensionNames(Protocol.Latest).ToList();

            Assert.That(results.Contains("Search"), Is.True);
            Assert.That(results.Contains("Song"), Is.True);
            Assert.That(results.Contains("TwitterMood"), Is.True);
        }

        [Test]
        public void GetExtension_LatestSearch_ReturnExtension()
        {
            var module = Make_CosoundModule();

            var result = module.GetExtension(Protocol.Latest, "Search");

            Assert.That(result, Is.InstanceOf<Search>());
        }

        [Test]
        public void GetExtension_LatestSong_ReturnExtension()
        {
            var module = Make_CosoundModule();

            var result = module.GetExtension(Protocol.Latest, "Song");

            Assert.That(result, Is.InstanceOf<Song>());
        }

        [Test]
        public void GetExtension_LatestTwitterMood_ReturnExtension()
        {
            var module = Make_CosoundModule();

            var result = module.GetExtension(Protocol.Latest, "TwitterMood");

            Assert.That(result, Is.InstanceOf<TwitterMood>());
        }

        private static CosoundModule Make_CosoundModule()
        {
            return new CosoundModule();
        }
    }
}
