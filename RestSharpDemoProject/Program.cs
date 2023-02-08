using RestSharp;
using System.Text.Json;

namespace RestSharpDemoProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RestClient client = new RestClient("https://api.github.com");
            RestRequest request = new RestRequest("/repos/{user}/{repoName}/issues/{id}/labels", Method.Get);

            request.AddUrlSegment("user", "AlexLubBak");
            request.AddUrlSegment("repoName", "postman");
            request.AddUrlSegment("id", "1");
            var response = client.Execute(request);

            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);
            


            var labels = JsonSerializer.Deserialize<List<Labels>>(response.Content);

            foreach (var label in labels)
            {
                Console.WriteLine("Label name: " + label.name);
                Console.WriteLine("Label id: " + label.id);
                Console.WriteLine("Label color: " + label.color);
            }


        }
    }
}