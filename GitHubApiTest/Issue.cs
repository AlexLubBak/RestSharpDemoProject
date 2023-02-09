namespace GitHubApiTest
{
    internal class Issue
    {
        public long number { get; set; }
        public string title { get; set; }
        public string body { get; set; }



        public List<Labels>labels { get; set; }


    }
}