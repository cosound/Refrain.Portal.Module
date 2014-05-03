namespace Refrain.Portal.Module
{
    using System.Collections.Generic;
    using System.Configuration;
    using CHAOS.Net;
    using Chaos.Mcm;
    using Chaos.Mcm.Data;
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
        private IMcmRepository McmRepository { get; set; }

        public IEnumerable<string> GetExtensionNames(Protocol version)
        {
            yield return "Search";
            yield return "Song";
        }

        public IExtension GetExtension<TExtension>(Protocol version) where TExtension : IExtension
        {
            return GetExtension(version, typeof (TExtension).Name);
        }

        public IExtension GetExtension(Protocol version, string name)
        {
            if ("Search".Equals(name))
                return new Search(PortalApplication, null);
            
            if ("Song".Equals(name))
                return new Song(PortalApplication, McmRepository);

            throw new ExtensionMissingException(name);
        }

        public void Load(IPortalApplication portalApplication)
        {
            PortalApplication = portalApplication;
            Configuration = new CoSoundConfiguration();

            PortalApplication.OnModuleLoaded += PortalApplication_OnModuleLoaded;
        }

        private bool _areViewsLoaded = false;
        
        void PortalApplication_OnModuleLoaded(object sender, ApplicationDelegates.ModuleArgs args)
        {
            if (_areViewsLoaded) return; 
            
            var mcmModule = args.Module as IMcmModule;

            if (mcmModule == null) return;

            McmRepository = mcmModule.McmRepository;

            CreateView(new SearchView(Configuration), "refrain-search");
            CreateView(new SongView(Configuration, McmRepository), "refrain-song");
            CreateView(new TwitterMoodView(Configuration), "refrain-twittermood");

            _areViewsLoaded = true;
        }

        private void CreateView(IView view, string coreName)
        {
            view.WithPortalApplication(PortalApplication);
            view.WithCache(PortalApplication.Cache);
            view.WithIndex(new SolrCore(new HttpConnection(ConfigurationManager.AppSettings["SOLR_URL"]), coreName));
            
            PortalApplication.ViewManager.AddView(view);
        }
    }
}
