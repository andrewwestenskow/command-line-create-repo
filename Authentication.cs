using System;
using Meziantou.Framework.Win32;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;


namespace create_repo
{
  class Authentication
  {
    
    private static string AppName = "GitHubCreateRepo";
    public static async Task<bool> Authenticate()
    {
      Console.WriteLine("Fetching credentials...");
      Utils.client.DefaultRequestHeaders.Add("User-Agent", AppName);
      var cred = CredentialManager.ReadCredential(applicationName: AppName);
      if(cred == null)
      {
        Console.WriteLine("Credentials not found.");
        string username = Utils.GetResponse("Input your github username");
        string newToken = Utils.GetResponse("Input a valid GitHub access token");

        bool areCredentialsValid = await ValidateCredentials(username, newToken);

        if(areCredentialsValid)
        {
        CredentialManager.WriteCredential(
          applicationName: AppName, 
          userName: username, 
          secret: newToken, 
          persistence: CredentialPersistence.LocalMachine,
          comment: "Used to authenticate with GitHub api"
          );
        } else 
        {
          Utils.ConfirmResubmit();
        }
      } else 
      {
        bool areCredentialsValid = await ValidateCredentials(cred.UserName, cred.Password);

        if(!areCredentialsValid)
        {
          Utils.ConfirmResubmit();
        }
      }

      return true;
    }

    public static async Task<bool> ValidateCredentials(string username, string token)
    {
      Console.WriteLine("Validating token...");
      using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/repos"))
      {
        requestMessage.Headers.Authorization = 
          new AuthenticationHeaderValue("Token", token
        );
        
        HttpResponseMessage response = await Utils.client.SendAsync(requestMessage);
        bool success = response.IsSuccessStatusCode;

        if(success)
        {
          Utils.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
          Console.WriteLine("Token Validated.");
          return true;
        } else 
        {
          Console.WriteLine("Token vaildation failed.");
          DeleteCredentials();
          return false;
        }
      }
    }

    public static void DeleteCredentials()
    {
      var cred = CredentialManager.ReadCredential(applicationName: AppName);

      if(cred != null)
      {
        CredentialManager.DeleteCredential(AppName);
      }
    }
  }
}