using AutoMapper;
using FluentValidation;
using LinqKit;
using SmartList.Application.DTOs.Product;
using SmartList.Application.Interfaces;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace SmartList.Application.Services;

public class ProductService : BaseService<Product, ProductCreateRequest, ProductUpdateRequest, ProductResponse>, IProductService
{
    public ProductService(
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<ProductCreateRequest> validator,
        IValidator<ProductUpdateRequest> updateValidator) 
        : base(uow, mapper, validator, updateValidator)
    {
    }

    public Expression<Func<Product, bool>>? GetDynamicFilter(ProductFilterRequest? request)
    {
        if (request == null)
            return null;

        var predicate = PredicateBuilder.New<Product>(true);

        if (!string.IsNullOrEmpty(request.Name))
            predicate = predicate.And(p => p.Name.Contains(request.Name));

        if (!string.IsNullOrEmpty(request.DefaultEan))
            predicate = predicate.And(p => p.DefaultEan.Contains(request.DefaultEan));

        if (!string.IsNullOrEmpty(request.Category))
            predicate = predicate.And(p => p.Category != null && p.Category.Name.Contains(request.Category));

        return predicate;
    }
}
