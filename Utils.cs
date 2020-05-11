using System;
using System.Net.Http;
using System.Diagnostics;


namespace create_repo
{
  class Utils
  {
    public static readonly HttpClient client = new HttpClient();
    
    public static string GetResponse(string prompt)
    {
      Console.WriteLine(prompt);
      string response = Console.ReadLine();

      return response;
    }

    public static string CheckForString(string response, string prompt)
    {
      if(response.Length > 0)
      {
        return response;
      } else
      {
        return GetResponse(prompt);
      }
    }

    public static async void ConfirmResubmit()
    {
      Console.WriteLine("Token is invalid.  Try again? Y/N");
      char response = Console.ReadKey(true).KeyChar;

      if(response == 'y')
      {
        await Authentication.Authenticate();
      }
    }

    public static bool YesOrNo(string prompt)
    {
      Console.WriteLine(prompt + " Y/N");
      char response = Console.ReadKey(true).KeyChar;

      if(response == 'y')
      {
        return true;
      } else
      {
        return false;
      }
    }

    private static void WaitForGitProcess(string process)
    {
      string gitCommand = "git";
      Process GitProcess = Process.Start(gitCommand, process);
      GitProcess.WaitForExit();
    }

    public static void PushRepo(string location, string commitMessage)
    {
      string gitInit = "init";
      string gitAdd = "add .";
      string gitCommit = $"commit -m '{commitMessage}'";
      string addOrigin = $"remote add origin {location}";
      string firstPush = "push -u origin master";

      WaitForGitProcess(gitInit);
      WaitForGitProcess(gitAdd);
      WaitForGitProcess(gitCommit);
      WaitForGitProcess(addOrigin);
      WaitForGitProcess(firstPush);
    }    
  }

  class UserCred
  {
    public string username {get;}
    public string token {get;}

    public UserCred(string username, string token)
    {
      this.username = username;
      this.token = token;
    }
  }
}