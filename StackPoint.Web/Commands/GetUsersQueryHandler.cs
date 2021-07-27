using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using StackPoint.Domain.Models;

namespace StackPoint.Web.Commands
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IHttpClientFactory _clientFactory;

        public GetUsersQueryHandler(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var paging = new Paging(request.Page, request.Take);

            return PostAsync<Paging, List<UserDto>>("user", paging, cancellationToken);
        }

        private async Task<TResult> PostAsync<TContent, TResult>(string method, TContent content,
            CancellationToken cancellationToken) where TResult : class
        {
            var json = JsonConvert.SerializeObject(content);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var url = $"https://localhost:5002/{method}";
            var response = await client.PostAsync(url, data, cancellationToken);

            var result = response.Content.ReadAsStringAsync().Result;
            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<TResult>(result) : null;
        }
    }
}