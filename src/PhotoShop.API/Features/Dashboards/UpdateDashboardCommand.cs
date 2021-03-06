using PhotoShop.Core.Interfaces;
using PhotoShop.Core.Models;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace PhotoShop.API.Features.Dashboards
{
    public class UpdateDashboardCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Dashboard.DashboardId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public DashboardDto Dashboard { get; set; }
        }

        public class Response
        {            
            public Guid DashboardId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventStore _eventStore;
            
            public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var dashboard = _eventStore.Load<Dashboard>(request.Dashboard.DashboardId);

                dashboard.ChangeName(request.Dashboard.Name);

                _eventStore.Save(dashboard);

                return Task.FromResult(new Response() { DashboardId = request.Dashboard.DashboardId }); 
            }
        }
    }
}
