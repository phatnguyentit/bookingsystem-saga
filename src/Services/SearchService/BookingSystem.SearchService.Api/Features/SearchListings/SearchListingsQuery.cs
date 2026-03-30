using BookingSystem.SearchService.Infrastructure.Search;
using MediatR;

namespace BookingSystem.SearchService.Api.Features.SearchListings;

public record SearchListingsQuery(
    string? Query,
    DateOnly? CheckIn,
    DateOnly? CheckOut,
    decimal? MaxPrice,
    int Page = 1,
    int PageSize = 20) : IRequest<SearchResult>;

public class SearchListingsHandler(ISearchService searchService)
    : IRequestHandler<SearchListingsQuery, SearchResult>
{
    public Task<SearchResult> Handle(SearchListingsQuery q, CancellationToken cancellationToken)
        => searchService.SearchListingsAsync(
            q.Query, q.CheckIn, q.CheckOut, q.MaxPrice, q.Page, q.PageSize, cancellationToken);
}
