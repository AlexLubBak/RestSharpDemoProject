using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

namespace GitHubApiTest
{
    public class ApiTests
    {

        private RestClient client;
        private const string baseUsrl = "https://api.github.com";
        private const string partialUrl = "repos/AlexLubBak/postman/issues";
        private const string username = "AlexLubBak";
        private const string password = "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";

        [SetUp]
        public void SetUp()
        {

            this.client = new RestClient(baseUsrl);
            this.client.Authenticator = new HttpBasicAuthenticator(username, password);


        }




        [Test]
        public void Test_GetSingleIssue()
        {

            var request = new RestRequest($"{partialUrl}/1", Method.Get);
            var response = this.client.Execute(request);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP status code property");

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.That(issue.title, Is.EqualTo("First issue"));
            Assert.That(issue.number, Is.EqualTo(1));
        }

        [Test]
        public void Test_GetSingleIssueLabels()
        {

            var request = new RestRequest($"{partialUrl}/1", Method.Get);
            var response = this.client.Execute(request);

            //Assert.NotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP status code property");

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            for (int i = 0; i < issue.labels.Count; i++)
            {
                Assert.That(issue.labels[i].name, Is.Not.Null);
            }
        }


        [Test]
        public void Test_GetAllIssue()
        {

            var request = new RestRequest($"{partialUrl}", Method.Get);
            var response = this.client.Execute(request);

            //Assert.NotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP status code property");

            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

            foreach (var issue in issues)
            {
                Assert.That(issue.title, Is.Not.Empty);
                Assert.That(issue.number, Is.GreaterThan(0));
            }
        }

        [Test]
        //�������� ��������� �� Issue. �������� body.
        public void Test_CreateIssue()
        {
            //Arrange
            var request = new RestRequest($"{partialUrl}", Method.Post);

            //���������� ������
            var issueBody = new
            {
                title = "Test issue from RestSharp" + DateTime.Now.Ticks,
                body = "some body from my issue",
                labels = new string[] { "bug", "critical", "release" }

            };
            request.AddBody(issueBody);

            //Act

            var response = this.client.Execute(request);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content); //����� �� ���� issue, � �� ���� �� �����


            //Assert

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "HTTP status code property");
            Assert.That(issue.number, Is.GreaterThan(0));  //������ �� issue �� � �� ����� �� ����
            Assert.That(issue.title, Is.EqualTo($"{issueBody.title}")); //����������� ��������� title,
            //���� � ����� �� title, ����� ��� ������ �� ��� 94.
            Assert.That(issue.body, Is.EqualTo(issueBody.body));


        }
        //������� �� ����� ������ ����. ����� �� ������� ��������, � ���� ���� ��� ���� ����� ������ 
        [Test]
        public void Test_Zipopotsmus_DD()

        {
            var restClient = new RestClient("https://api.zippopotam.us");
            var request = new RestRequest("us/90210", Method.Get);

            var response = restClient.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");

            var location = JsonSerializer.Deserialize<Location>(response.Content);

            Assert.That(location.country, Is.EqualTo("United States"));

        }

        //����� ������. ������ �������� ���������� � ���� ����.

        [TestCase("US", "90210", "United States")]
        [TestCase("BG", "1000", "Bulgaria")]
        //[TestCase("DE", "0167", "Germany")]  //���� ����� � ������� �� �������� � ������
       

        public void Test_Zippopotsmus_DD(string countryCode, string zipCode, string expectedCountry)
        {
            var restClient = new RestClient("https://api.zippopotam.us");
            var request = new RestRequest(countryCode + "/" + zipCode, Method.Get);

            var response = restClient.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");

            var location = JsonSerializer.Deserialize<Location>(response.Content);

            Assert.That(location.country, Is.EqualTo(expectedCountry));
        }











}
}