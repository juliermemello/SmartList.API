using SmartList.Application.DTOs.ShoppingList;
using SmartList.Domain.Entity;
using System.Linq.Expressions;

namespace SmartList.Application.Interfaces;

public interface IShoppingListService : IBaseService<ShoppingList, ShoppingListCreateRequest, ShoppingListUpdateRequest, ShoppingListResponse>
{
    Expression<Func<ShoppingList, bool>>? GetDynamicFilter(ShoppingListFilterRequest? request);
}
