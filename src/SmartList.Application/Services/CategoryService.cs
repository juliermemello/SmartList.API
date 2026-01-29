using AutoMapper;
using FluentValidation;
using SmartList.Application.DTOs.Category;
using SmartList.Application.Interfaces;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace SmartList.Application.Services;

public class CategoryService : BaseService<Category, CategoryCreateRequest, CategoryResponse>, ICategoryService
{
    public CategoryService(
        IUnitOfWork uow, 
        IMapper mapper, 
        IValidator<CategoryCreateRequest> validator) 
        : base(uow, mapper, validator)
    {
    }

    public Expression<Func<Category, bool>>? GetDynamicFilter(CategoryFilterRequest? request)
    {
        throw new NotImplementedException();
    }
}
