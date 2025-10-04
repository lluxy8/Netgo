using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Domain;
using Netgo.Persistence.Helper;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text;

namespace Netgo.Persistence.Repositories
{
    public class CachedProductRepository : IProductRepository
    {
        private readonly IProductRepository _decorated;
        private readonly IDistributedCache _cache;

        public CachedProductRepository(IProductRepository repository, IDistributedCache cache)
        {
            _decorated = repository;
            _cache = cache;
        }

        public Task Delete(Product entity)
        {
            return _decorated.Delete(entity);
        }

        public Task<bool> Exists(Guid Id)
        {
            return _decorated.Exists(Id);
        }

        public Task<IReadOnlyList<Product>> GetAll()
        {
            return _decorated.GetAll();
        }

        public async Task<PagedResult<Product>> GetAllFilteredPaged(
            Expression<Func<Product, bool>>? filter = null,
            int page = 1,
            int pageSize = 10)
        {         
            string key = CacheKeyHelper.GenerateKey(filter, page, pageSize);
            string? cachedData = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedData))
            {
                var result = await _decorated.GetAllFilteredPaged(filter, page, pageSize);

                if (result is null || !result.Items.Any())
                    return result;

                await _cache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) 
                    });

                return result;
            }

            return JsonConvert.DeserializeObject<PagedResult<Product>>(cachedData)!;
        }

        public async Task<Product?> GetById(Guid Id)
        {
            string key = $"product-{Id}";

            string? cachedMember = await _cache.GetStringAsync(key);

            Product? product;

            if(string.IsNullOrEmpty(cachedMember))
            {
                product = await _decorated.GetById(Id);

                if (product is null)
                    return product;

                await _cache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(product),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });

                return product;
            }


            product = JsonConvert.DeserializeObject<Product>(cachedMember);
            return product;
        }
        public async Task<List<Product>> GetProductsByUserId(Guid userId)
        {
            string key = $"products-user-{userId}";

            string? cachedData = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedData))
            {
                var result = await _decorated.GetProductsByUserId(userId);

                if (result is null || result.Count == 0)
                    return result;

                await _cache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });

                return result;
            }

            return JsonConvert.DeserializeObject<List<Product>>(cachedData)!;
        }


        public async Task<Product?> GetProductWithDetails(Guid productId)
        {
            string key = $"product-details-{productId}";

            string? cachedData = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedData))
            {
                var result = await _decorated.GetProductWithDetails(productId);

                if (result is null)
                    return null;

                await _cache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(result, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });


                return result;
            }

            return JsonConvert.DeserializeObject<Product>(cachedData)!;
        }


        public Task Insert(Product entity)
        {
            return _decorated.Insert(entity);
        }

        public Task Update(Product entity)
        {
            return _decorated.Update(entity);
        }
    }
}
