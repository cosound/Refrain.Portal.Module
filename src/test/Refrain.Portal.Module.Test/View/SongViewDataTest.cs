namespace Refrain.Portal.Module.Test.View
{
    using System;
    using System.Linq;
    using Module.View;
    using NUnit.Framework;

    [TestFixture]
    public class SongViewDataTest
    {
        [Test]
        public void GetIndexableFields_FieldsAreSet_ReturnFields()
        {
            var data = new SongViewData
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000001"),
                    DataSet = 3,
                    Similarity = new Similarity
                        {
                            Type = "111111"
                        }
                };

            var fields = data.GetIndexableFields().ToDictionary(item => item.Key);

            Assert.That(fields["Id"].Value, Is.EqualTo("10000000-0000-0000-0000-000000000001_111111"));
            Assert.That(fields["Similarity.Type"].Value, Is.EqualTo("111111"));
            Assert.That(fields["DataSet"].Value, Is.EqualTo("3"));
        }
    }
}
