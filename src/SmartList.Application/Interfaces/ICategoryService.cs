using SmartList.Application.DTOs.Category;
using SmartList.Domain.Entity;
using System.Linq.Expressions;

namespace SmartList.Application.Interfaces;

public interface ICategoryService : IBaseService<Category, CategoryCreateRequest, CategoryResponse>
{
    Expression<Func<Category, bool>>? GetDynamicFilter(CategoryFilterRequest? request);
}
