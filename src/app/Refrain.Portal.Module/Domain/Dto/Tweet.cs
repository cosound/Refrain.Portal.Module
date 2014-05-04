namespace Refrain.Portal.Module.Domain.Dto
{
    using CHAOS.Serialization;

    public class Tweet
    {
        [Serialize]
        public string Id { get; set; }

        [Serialize]
        public string EmbedCode { get; set; }
    }
}
