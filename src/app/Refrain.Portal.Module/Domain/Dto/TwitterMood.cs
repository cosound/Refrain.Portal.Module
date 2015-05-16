namespace Refrain.Portal.Module.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using CHAOS.Serialization;

    public class TwitterMood
    {
        [Serialize("Id")]
        public Guid Identity { get; set; }

        [Serialize]
        public string Country { get; set; }

        [Serialize]
        public double Valence { get; set; }

        [Serialize]
        public DateTime DateCreated { get; set; }

        [Serialize]
        public IList<Tweet> Tweets { get; set; }

        public TwitterMood()
        {
            Tweets = new List<Tweet>();
        }
    }
}
