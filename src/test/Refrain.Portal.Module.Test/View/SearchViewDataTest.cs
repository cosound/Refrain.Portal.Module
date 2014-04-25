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
            var data = new SearchViewData
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000001"),
                    Text = "text"
                };

            var fields = data.GetIndexableFields().ToList();

            Assert.That(fields[0].Key, Is.EqualTo("Id"));
            Assert.That(fields[0].Value, Is.EqualTo("10000000-0000-0000-0000-000000000001"));
            Assert.That(fields[1].Key, Is.EqualTo("Text"));
            Assert.That(fields[1].Value, Is.EqualTo("text"));
        }
    }
}
