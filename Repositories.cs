using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace create_repo
{
  class RepositoryUtils
  {
    public static async Task<RepoResponse> CreateRepository(
      string name, string description, string visibility, string template, string orgName
    )
    {
      
      string repoName = Utils.CheckForString(name, "Input repo name");
      string repoDescription = Utils.CheckForString(description, "Input repo description");
      string isRepoPrivate = Utils.CheckForString(visibility, "Is repo private?");

      Repository newRepo = new Repository(repoName, repoDescription, isRepoPrivate, template);

      var body = new StringContent(
        JsonSerializer.Serialize<Repository>(newRepo),  UnicodeEncoding.UTF8, "application/json"
      );

     var Response = await Utils.PostRepo(body, orgName);

      if(Response.IsSuccessStatusCode)
      {
        var Details = JsonSerializer.Deserialize<NewRepo>(Response.Content.ReadAsStringAsync().Result);

        string message = $"Created repo {Details.name} at {Details.location}";

        Console.WriteLine(message);

        RepoResponse Result = new RepoResponse(
          Details.location, Response.IsSuccessStatusCode, message
        );

        return Result;
      } else 
      {
        RepoResponse Result = new RepoResponse(
          "", Response.IsSuccessStatusCode, "Failed to create repository"
        );

        Console.WriteLine(Result.message);

        return Result;
      }

    }
  }

  class Repository
  {
    public string name {get;}
    public string description {get;}
    public string visibility {get;}
    public string gitignore_template{get;}

    public Repository(string name, string description, string visibility, string template)
    {
      this.name = name;
      this.description = description;
      this.visibility = visibility;
      this.gitignore_template = template;
    }
  }

  class NewRepo
  {
    [JsonPropertyName("name")]
    public string name {get; set;}
    [JsonPropertyName("clone_url")]
    public string location {get; set;}
  }

  class RepoResponse
  {
    public string location {get; set;}
    public bool isSuccess {get; set;}
    public string message {get; set;}

    public RepoResponse(string location, bool isSuccess, string message)
    {
      this.location = location;
      this.isSuccess = isSuccess;
      this.message = message;
    }
  }
}