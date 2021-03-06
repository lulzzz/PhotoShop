using PhotoShop.Core.Interfaces;
using PhotoShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoShop.API.Features.Bookings
{
    public class GetBookingsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<BookingDto> Bookings { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IRepository _repository;
            
			public Handler(IRepository repository) => _repository = repository;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => Task.FromResult(new Response()
                {
                    Bookings = _repository.Query<Booking>().Select(x => BookingDto.FromBooking(x)).ToList()
                });
        }
    }
}
