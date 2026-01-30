using SmartList.Application.DTOs.Product;
using SmartList.Domain.Entity;
using System.Linq.Expressions;

namespace SmartList.Application.Interfaces;

public interface IProductService : IBaseService<Product, ProductCreateRequest, ProductUpdateRequest, ProductResponse>
{
    Expression<Func<Product, bool>>? GetDynamicFilter(ProductFilterRequest? request);
}
