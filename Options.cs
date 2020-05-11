using CommandLine;

namespace create_repo
{
  public class Options
  {
    [Option("here", Required = false, Default = false, HelpText = "Push repo from current location")]
    public bool useCurrentDirectory {get; set;}

    [Option('m', "message", Required = false, Default = "\"Initial Commit\"", HelpText="Commit message")]
    public string commitMessage {get; set;}
    [Option('t', "template", Required = false, HelpText=".gitignore template to use")]
    public string template {get; set;}
    [Option("name", Required = false, HelpText = "Repo name")]
    public string name {get; set;}
    [Option("desc", Required = false, Default = "No description provided", HelpText = "Repo description")]
    public string description {get; set;}
    [Option("visibility", Required = false, Default = "public", HelpText = "Repo visibility 'public' or 'private'")]
    public string visibility {get; set;}
  }
}