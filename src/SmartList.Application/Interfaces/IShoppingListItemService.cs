using SmartList.Application.DTOs.ShoppingListItem;
using SmartList.Domain.Entity;
using System.Linq.Expressions;

namespace SmartList.Application.Interfaces;

public interface IShoppingListItemService : IBaseService<ListItem, ShoppingListItemCreateRequest, ShoppingListItemUpdateRequest, ShoppingListItemResponse>
{
    Expression<Func<ListItem, bool>>? GetDynamicFilter(ShoppingListItemFilterRequest? request);
}
