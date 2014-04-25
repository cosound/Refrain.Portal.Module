﻿namespace Refrain.Portal.Module
{
    using System.Collections.Generic;
    using System.Configuration;
    using CHAOS.Net;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Exceptions;
    using Chaos.Portal.Core.Extension;
    using Chaos.Portal.Core.Indexing.Solr;
    using Chaos.Portal.Core.Indexing.View;
    using Chaos.Portal.Core.Module;
    using Extension;
    using View;

    public class CosoundModule  : IModule
    {
        private IPortalApplication PortalApplication { get; set; }
        private CoSoundConfiguration Configuration { get; set; }

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
            Configuration = new CoSoundConfiguration();

            CreateView(new SearchView(Configuration), "refrain-search");
        }

        private void CreateView(IView view, string coreName)
        {
            view.WithPortalApplication(PortalApplication);
            view.WithCache(PortalApplication.Cache);
            view.WithIndex(new SolrCore(new HttpConnection(ConfigurationManager.AppSettings["SOLR_URL"]), coreName));
        }
    }
}
