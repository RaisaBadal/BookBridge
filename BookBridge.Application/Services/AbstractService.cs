using AutoMapper;
using BookBridge.Domain.Interfaces;

namespace BookBridge.Application.Services
{
    public abstract class AbstractService(IUnitOfWorkRepo unitOfWorkRepo, IMapper autoMapper)
    {
        protected readonly IUnitOfWorkRepo UnitOfWorkRepo = unitOfWorkRepo;
        protected readonly IMapper AutoMapper = autoMapper;
    }
}
