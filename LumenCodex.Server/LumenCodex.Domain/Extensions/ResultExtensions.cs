using LumenCodex.Domain.Result;

namespace LumenCodex.Domain.Extensions;

public static class ResultExtensions
{
    public static T Match<T>(
        this Result.Result result,
        Func<T> onSuccess, 
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() :
        onFailure(result.Error);
    }
}