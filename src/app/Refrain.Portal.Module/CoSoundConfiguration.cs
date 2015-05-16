namespace Refrain.Portal.Module
{
  using System;

  public class CoSoundConfiguration
  {
    public ObjectTypes ObjectTypes { get; set; }
    public MetadataSchemas MetadataSchemas { get; set; }

    public string ConnectionString { get; set; }

    public CoSoundConfiguration()
    {
      ObjectTypes = new ObjectTypes();
      MetadataSchemas = new MetadataSchemas();

      ConnectionString =
        "user id=refrainApp;password=f3IGK6FOqn;server=cosound.cvwtednyvdne.eu-west-1.rds.amazonaws.com;persist security info=True;database=refrain-statistics;Allow User Variables=True;Charset=utf8;";
    }
  }

  public class MetadataSchemas
  {
    public Guid AudioMusicTrack
    {
      get { return new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFF120151"); }
    }

    public Guid GenericSimilarity
    {
      get { return new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFF120800"); }
    }

    public Guid StateMood
    {
      get { return new Guid("ffffffff-ffff-ffff-ffff-ffffff220100"); }
    }
  }

  public class ObjectTypes
  {
    public uint ManifestationId
    {
      get { return 120; }
    }

    public uint ActorContent
    {
      get { return 220; }
    }
  }
}