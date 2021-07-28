using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using StackPoint.Data;

namespace StackPoint.Service2.Commands
{
    public class BindUserWithOrganizationCommandHandler : IRequestHandler<BindUserWithOrganizationCommand>
    {
        private readonly ILogger<BindUserWithOrganizationCommandHandler> _logger;
        private readonly DatabaseContext _databaseContext;

        public BindUserWithOrganizationCommandHandler(ILogger<BindUserWithOrganizationCommandHandler> logger,
            DatabaseContext databaseContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        }

        public Task<Unit> Handle(BindUserWithOrganizationCommand request, CancellationToken cancellationToken)
        {
            // TODO: Реализовать
            throw new NotImplementedException();
        }
    }
}