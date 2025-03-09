using AuctriAPI.Application.Services.DateTime;

namespace AuctriAPI.Infrastructure.Services.DateTime; 
    
public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}

