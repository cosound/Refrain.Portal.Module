namespace Refrain.Portal.Module.Extension
{
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Extension;
    using Data;

    public class Search : AExtension
    {
        public IRefrainRepository Repository { get; set; }

        public Search(IPortalApplication portalApplication, IRefrainRepository repository) : base(portalApplication)
        {
            Repository = repository;
        }
    }
}
