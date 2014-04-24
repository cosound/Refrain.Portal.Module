namespace Refrain.Portal.Module
{
    using System.Collections.Generic;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Exceptions;
    using Chaos.Portal.Core.Extension;
    using Chaos.Portal.Core.Module;
    using Extension;

    public class CosoundModule  : IModule
    {
        private IPortalApplication PortalApplication { get; set; }

        public IEnumerable<string> GetExtensionNames(Protocol version)
        {
            yield return "Album";
        }

        public IExtension GetExtension<TExtension>(Protocol version) where TExtension : IExtension
        {
            return GetExtension(version, typeof (TExtension).Name);
        }

        public IExtension GetExtension(Protocol version, string name)
        {
            if("Album".Equals(name))
                return new Album(PortalApplication);

            throw new ExtensionMissingException(name);
        }

        public void Load(IPortalApplication portalApplication)
        {
            PortalApplication = portalApplication;
        }
    }
}
