namespace Refrain.Portal.Module.Test.View
{
    using System;
    using System.Linq;
    using Module.View;
    using NUnit.Framework;

    public class TweetViewTest : TestBase
    {
        [Test]
        public void Index_GivenStateMood_ReturnMappedTweetViewData()
        {
            var view = new TweetView(new CoSoundConfiguration());
            var moodObject = Make_TwitterMoodObject();

            var result = view.Index(moodObject as object).First() as TweetViewData;

            Assert.That(result.Identity, Is.EqualTo("462722417044226048"));
            Assert.That(result.Country, Is.EqualTo("azerbaijan"));
            Assert.That(result.DateCreated, Is.EqualTo(new DateTime(2014, 05, 03, 21, 16, 55)));
            Assert.That(result.EmbedCode, Is.EqualTo("%3Cobjects+provider_url%3D%22https%3A%2F%2Ftwitter.com%22+version%3D%221.0%22+url%3D%22https%3A%2F%2Ftwitter.com%2Fbs16996%2Fstatuses%2F462722417044226048%22+author_name%3D%22EL+CYRUS%22+height%3D%22None%22+width%3D%22550%22+html%3D%22%3Cblockquote+class%3D%22twitter-tweet%22%3E%3Cp%3E%3Ca+href%3D%22https%3A%2F%2Ftwitter.com%2Fsearch%3Fq%3D%2523NP%26amp%3Bsrc%3Dhash%22%3E%23NP%3C%2Fa%3E+SABINA+BABAYEVA+-+WHEN+THE+MUSIC+DIES%26%2310%3BEUROVISION+SONG+CONTEST+2012+AZERBAIJAN%3C%2Fp%3E%26mdash%3B+EL+CYRUS+%28%40bs16996%29+%3Ca+href%3D%22https%3A%2F%2Ftwitter.com%2Fbs16996%2Fstatuses%2F462722417044226048%22%3EMay+3%2C+2014%3C%2Fa%3E%3C%2Fblockquote%3E%0A%3Cscript+async+src%3D%22%2F%2Fplatform.twitter.com%2Fwidgets.js%22+charset%3D%22utf-8%22%3E%3C%2Fscript%3E%22+author_url%3D%22https%3A%2F%2Ftwitter.com%2Fbs16996%22+provider_name%3D%22Twitter%22+cache_age%3D%223153600000%22+type%3D%22rich%22%2F%3E"));
        }

        [Test]
        public void GetIndexableFields_ValuesSet_ReturnFields()
        {
            var data = Make_TweetViewData();

            var results = data.GetIndexableFields().ToDictionary(item => item.Key);

            Assert.That(results["Id"].Value, Is.EqualTo("twitter id"));
            Assert.That(results["Country.Name"].Value, Is.EqualTo("country"));
            Assert.That(results["DateCreated"].Value, Is.EqualTo("2000-01-01T10:00:00Z"));
        }

        private static TweetViewData Make_TweetViewData()
        {
            return new TweetViewData
                {
                    Identity = "twitter id",
                    Country = "country",
                    DateCreated = new DateTime(2000, 01, 01, 10,00,00),
                    EmbedCode = "embedcode"
                };
        }
    }
}
