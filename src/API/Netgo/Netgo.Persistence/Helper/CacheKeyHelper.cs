using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Netgo.Persistence.Helper;

public static class CacheKeyHelper
{
    public static string GenerateKey<T>(Expression<Func<T, bool>>? filter, int page, int pageSize)
    {
        var extractor = new ExpressionParameterExtractor();
        var parameters = extractor.Extract(filter!);
        string filterJson = JsonConvert.SerializeObject(parameters);
        string filterHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(filterJson)));
        return $"products-{filterHash}-page{page}-size{pageSize}";
    }
}
