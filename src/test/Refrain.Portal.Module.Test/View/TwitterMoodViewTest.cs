namespace Refrain.Portal.Module.Test.View
{
    using System;
    using System.Linq;
    using Module.View;
    using NUnit.Framework;

    public class TwitterMoodViewTest : TestBase
    {
        [Test]
        public void Index_GivenStateMood_ReturnMappedViewData()
        {
            var view = new TwitterMoodView(new CoSoundConfiguration());
            var moodObject = Make_TwitterMoodObject();

            var result = view.Index(moodObject).First() as TwitterMoodViewData;

            Assert.That(result.Identity, Is.EqualTo(moodObject.Guid));
            Assert.That(result.Country, Is.EqualTo("azerbaijan"));
            Assert.That(result.DateCreated, Is.EqualTo(new DateTime(2014, 05, 03, 21, 16, 55)));
        }

        [Test]
        public void GetIndexableFields_ValuesSet_ReturnFields()
        {
            var data = Make_TwitterMoodViewData();

            var results = data.GetIndexableFields().ToDictionary(item => item.Key);

            Assert.That(results["Id"].Value, Is.EqualTo("10000000-0000-0000-0000-000000000001"));
            Assert.That(results["Country.Name"].Value, Is.EqualTo("country"));
            Assert.That(results["Valence"].Value, Is.EqualTo("0.4"));
            Assert.That(results["DateCreated"].Value, Is.EqualTo("2000-01-01T10:00:00Z"));
        }

        private static TwitterMoodViewData Make_TwitterMoodViewData()
        {
            return new TwitterMoodViewData
                {
                    Identity = new Guid("10000000-0000-0000-0000-000000000001"),
                    Country = "country",
                    Valence = 0.4,
                    DateCreated = new DateTime(2000, 01, 01, 10,00,00)
                };
        }
    }
}
