namespace Alias
{
    public class AliasPlugin : Rhino.PlugIns.PlugIn
    {
        public AliasPlugin()
        {
            Instance = this;
        }
        public static AliasPlugin Instance { get; private set; }

    }
}