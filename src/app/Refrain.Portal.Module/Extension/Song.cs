namespace Refrain.Portal.Module.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Chaos.Mcm.Data;
    using Chaos.Mcm.Extension.v6;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Extension;
    using Chaos.Portal.Core.Indexing.Solr.Request;
    using Data;
    using Domain;
    using Exceptions;
    using View;

    public class Song : AExtension
    {
        public IMcmRepository McmRepository { get; set; }

        public Song(IPortalApplication portalApplication, IMcmRepository mcmRepository) : base(portalApplication)
        {
            McmRepository = mcmRepository;
        }

        public Trace Index()
        {
            var cache = new Dictionary<Guid, Data.Model.Song>();
            IList<Chaos.Mcm.Data.Dto.Object> objects;
            var config = new CoSoundConfiguration();
            var trace = new Trace();

            var songView = (SongView)ViewManager.GetView("Song");
            songView.WithSongCache(cache);

            for (var i = 0u; (objects = McmRepository.ObjectGet(null, i, 200, true)).Any(); i++)
            {
                try
                {
                    var songs = objects.Select(item => SongMapper.Create(item, config));

                    foreach (var song in songs.Where(song => !cache.ContainsKey(song.Id)))
                    {
                        cache.Add(song.Id, song);
                    }
                }
                catch (CannotMapException)
                {
                }

                ViewManager.GetView("Song").Index(objects);

                trace.Lines.Add(objects.Count + " was sent to index");
            }

            trace.Lines.Add("Cache size " + cache.Count.ToString());

            return trace;
        }

        public IPagedResult<IResult> Get(Guid id, string type, uint dataSet = 3)
        {
            var query = new SolrQuery
                {
                    Query = String.Format("Id:{0}_{1}_{2}", id, type, dataSet.ToString(CultureInfo.InvariantCulture)),
                    PageSize = 1
                };

            var view = ViewManager.GetView("Song");
            return view.Query(query);
        }
    }
}
