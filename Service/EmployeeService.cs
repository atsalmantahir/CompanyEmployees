using Contracts;
using Service.Contracts;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{

    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager logger)
    {
        _repository = repositoryManager;
        _logger = logger;
    }
}
