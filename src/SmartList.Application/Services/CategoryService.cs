using AutoMapper;
using FluentValidation;
using LinqKit;
using SmartList.Application.DTOs.Category;
using SmartList.Application.Interfaces;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace SmartList.Application.Services;

public class CategoryService : BaseService<Category, CategoryCreateRequest, CategoryUpdateRequest, CategoryResponse>, ICategoryService
{
    public CategoryService(
        IUnitOfWork uow, 
        IMapper mapper, 
        IValidator<CategoryCreateRequest> validator,
        IValidator<CategoryUpdateRequest> updateValidator) 
        : base(uow, mapper, validator, updateValidator)
    {
    }

    public Expression<Func<Category, bool>>? GetDynamicFilter(CategoryFilterRequest? request)
    {
        if (request == null)
            return null;

        var predicate = PredicateBuilder.New<Category>(true);

        if (!string.IsNullOrEmpty(request.Name))
            predicate = predicate.And(p => p.Name.Contains(request.Name));

        return predicate;
    }
}
