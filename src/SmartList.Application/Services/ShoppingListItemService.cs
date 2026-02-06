using AutoMapper;
using FluentValidation;
using LinqKit;
using SmartList.Application.DTOs.ShoppingListItem;
using SmartList.Application.Interfaces;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace SmartList.Application.Services;

public class ShoppingListItemService : BaseService<ListItem, ShoppingListItemCreateRequest, ShoppingListItemUpdateRequest, ShoppingListItemResponse>, IShoppingListItemService
{
    public ShoppingListItemService(
        IUnitOfWork uow, 
        IMapper mapper, 
        IValidator<ShoppingListItemCreateRequest> validator,
        IValidator<ShoppingListItemUpdateRequest> updateValidator) 
        : base(uow, mapper, validator, updateValidator)
    {
    }

    public Expression<Func<ListItem, bool>>? GetDynamicFilter(ShoppingListItemFilterRequest? request)
    {
        if (request == null)
            return null;

        var predicate = PredicateBuilder.New<ListItem>(true);

        if (request.ProductId > 0)
            predicate = predicate.And(p => p.ProductId == request.ProductId);

        if (!string.IsNullOrEmpty(request.Product))
            predicate = predicate.And(p => p.Product != null && p.Product.Name != null && p.Product.Name.Contains(request.Product));

        if (request.CategoryId > 0)
            predicate = predicate.And(p => p.Product != null && p.Product.CategoryId == request.CategoryId);

        if (!string.IsNullOrEmpty(request.CategoryName))
            predicate = predicate.And(p => p.Product != null && p.Product.Category != null && p.Product.Category.Name != null && p.Product.Category.Name.Contains(request.CategoryName));

        if (!string.IsNullOrEmpty(request.Ean))
            predicate = predicate.And(p => p.Product != null && p.Product.DefaultEan != null && p.Product.DefaultEan.Contains(request.Ean));

        return predicate;
    }
}
