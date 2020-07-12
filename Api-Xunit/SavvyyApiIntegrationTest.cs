using Domain;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XunitTestApi
{
    public class SavvyyApiIntegrationTest
    {
        [Fact]
        public async Task TestGetAll()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/Api/v1/Books");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
        [Fact]
        public async Task TestGetSingle()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/Api/v1/Books/1");
                response.EnsureSuccessStatusCode();

                try
                {
                    var testBook = JsonConvert.DeserializeObject<Book>(response.Content.ReadAsStringAsync().Result);
                }
                catch (Exception)
                {

                    Assert.True(false,"Returned Json is not a Book object.");
                }



                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestPost()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/Api/v1/Books", new StringContent(JsonConvert.SerializeObject(
                    new Book()
                    {
                        Author = "Dostoyevski",
                        Title = "Karamazof Brothers 2",
                        Price = 300,
                        CoverImage = "karamazov.jpg",
                        Description = "It's Dostoyevski."
                    }), Encoding.UTF8, "application/json"));

                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }
        [Fact]
        public async Task TestDeleteSingle()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.DeleteAsync("/Api/v1/Books/1");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestPut()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PutAsync("/Api/v1/Books/3", new StringContent(JsonConvert.SerializeObject(
                    new Book()
                    {
                        Author = "Dostoyevski",
                        Title = "Karamazof Brothers",
                        Price = 300,
                        CoverImage = "karamazov.jpg",
                        Description = "It's Dostoyevski."
                    }), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }
    }
}
