namespace Refrain.Portal.Module.Domain.Dto
{
    using CHAOS.Serialization;

    public class Tweet
    {
        [Serialize("Id")]
        public string Identity { get; set; }

        [Serialize]
        public string EmbedCode { get; set; }
    }
}
