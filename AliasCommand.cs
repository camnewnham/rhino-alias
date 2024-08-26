using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;

namespace Alias
{
    /// <summary>
    /// A command that assigns macros to aliases in Rhino's ApplicationSettings.
    /// </summary>
    public class AliasCommand : Command
    {
        public AliasCommand()
        {
            Instance = this;
        }
        public static AliasCommand Instance { get; private set; }
        public override string EnglishName => "Alias";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            GetString getAlias = new GetString();
            getAlias.SetCommandPrompt("Enter Alias");
            if (getAlias.Get() != GetResult.String)
            {
                return Result.Cancel;
            }

            string alias = getAlias.StringResult();

            if (string.IsNullOrEmpty(alias))
            {
                return Result.Failure;
            }

            bool isOverride = Rhino.ApplicationSettings.CommandAliasList.IsAlias(alias);

            GetString getCommand = new GetString();
            if (isOverride)
            {
                getCommand.SetCommandPrompt($"Override existing alias \"{alias}\"");
            }
            else
            {
                getCommand.SetCommandPrompt($"Enter Command for \"{alias}\"");
            }

            if (getCommand.Get() != GetResult.String)
            {
                return Result.Cancel;
            }
            string macro = getCommand.StringResult();
            if (string.IsNullOrEmpty(macro))
            {
                return Result.Failure;
            }

            if (isOverride)
            {
                if (!Rhino.ApplicationSettings.CommandAliasList.SetMacro(alias, macro))
                {
                    Rhino.RhinoApp.WriteLine($"Failed to override alias \"{alias}\" -> \"{macro}\"");
                    return Result.Failure;
                }
            }
            else
            {
                if (!Rhino.ApplicationSettings.CommandAliasList.Add(alias, macro))
                {
                    Rhino.RhinoApp.WriteLine($"Failed to set alias \"{alias}\" -> \"{macro}\"");
                    return Result.Failure;
                }
            }

            if (!Rhino.ApplicationSettings.CommandAliasList.SetMacro(alias, macro))
            {
                Rhino.RhinoApp.WriteLine($"Failed to set alias \"{alias}\" -> \"{macro}\"");
                return Result.Failure;
            }
            Rhino.RhinoApp.WriteLine($"Assigned alias \"{alias}\" -> \"{macro}\"");

            return Result.Success;
        }
    }
}
