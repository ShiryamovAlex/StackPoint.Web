using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackPoint.Domain.Models;

namespace StackPoint.Web.Commands
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<GetUsersQueryHandler> _logger;

        public GetUsersQueryHandler(IHttpClientFactory clientFactory, ILogger<GetUsersQueryHandler> logger)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var paging = new Paging(request.Page, request.Take);

            return PostAsync<Paging, List<UserDto>>("user", paging, cancellationToken);
        }

        private async Task<TResult> PostAsync<TContent, TResult>(string method, TContent content,
            CancellationToken cancellationToken) where TResult : class
        {
            _logger.Log(LogLevel.Information, "Запрос пользователей из БД");
            var json = JsonConvert.SerializeObject(content);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            var url = $"{Environment.GetEnvironmentVariable("SERVISE2_URL")}/{method}";
            var response = await client.PostAsync(url, data, cancellationToken);

            var result = response.Content.ReadAsStringAsync().Result;
            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<TResult>(result) : null;
        }
    }
}