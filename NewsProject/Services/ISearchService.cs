using NewsProject.DataTransferObject;
using NewsProject.Models;

namespace NewsProject.Services
{
    public interface ISearchService
    {
        IAsyncEnumerable<New> SearchWithDate(DateDto date);
        IAsyncEnumerable<New> SearchWithText(string text);
    }
}
