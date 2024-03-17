using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionCreatedConsumer : IConsumer<Fault<AuctionCreated>>
{
    private readonly IMapper _mapper;

    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        var exception = context.Message.Exceptions.First();

        if (exception.ExceptionType == "System.ArgumentException")
        {
            var auction = _mapper.Map<AuctionCreated>(context.Message.Message);
            var item = _mapper.Map<Item>(auction);
            await item.SaveAsync();
        }
        else
        {
            Console.WriteLine("Error in AuctionCreatedConsumer: " + exception.Message);
        }
    }
}
