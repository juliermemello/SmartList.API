using AutoMapper;
using FluentValidation;
using LinqKit;
using SmartList.Application.DTOs.ShoppingList;
using SmartList.Application.Interfaces;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace SmartList.Application.Services;

public class ShoppingListService : BaseService<ShoppingList, ShoppingListCreateRequest, ShoppingListUpdateRequest, ShoppingListResponse>, IShoppingListService
{
    public ShoppingListService(
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<ShoppingListCreateRequest> validator,
        IValidator<ShoppingListUpdateRequest> updateValidator) 
        : base(uow, mapper, validator, updateValidator)
    {
    }

    public Expression<Func<ShoppingList, bool>>? GetDynamicFilter(ShoppingListFilterRequest? request)
    {
        if (request == null)
            return null;

        var predicate = PredicateBuilder.New<ShoppingList>(true);

        if (!string.IsNullOrEmpty(request.Name))
            predicate = predicate.And(p => p.Name.Contains(request.Name));

        return predicate;
    }
}
