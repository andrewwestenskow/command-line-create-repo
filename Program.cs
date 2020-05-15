using CommandLine;
using System.Threading.Tasks;
using System;

namespace create_repo
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args).WithParsed(o => AsyncMain(o).Wait());
        }

        static async Task AsyncMain(Options Options)
        {
            await Authentication.Authenticate();

            var RepoResponse = await RepositoryUtils.CreateRepository(
                Options.name, Options.description, Options.visibility, Options.template, Options.orgName
            );

            if(RepoResponse.isSuccess)
            {
                if(Options.useCurrentDirectory)
                {
                Utils.PushRepo(RepoResponse.location, Options.commitMessage);
                }
            }
        }
    }
}
