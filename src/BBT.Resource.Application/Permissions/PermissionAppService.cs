using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Services;
using BBT.Prism.Uow;

namespace BBT.Resource.Permissions;

public class PermissionAppService(
    IServiceProvider serviceProvider,
    IUnitOfWork unitOfWork,
    IPermissionRepository permissionRepository,
    IPermissionGrantRepository permissionGrantRepository,
    MultiLingualPermissionObjectMapper multiLingualMapper,
    IPermissionManager permissionManager,
    PermissionGrantStrategyFactory strategyGrantFactory)
    : ApplicationService(serviceProvider), IPermissionAppService
{
    public async Task<PermissionListResultDto> GetAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        var strategy = strategyGrantFactory.CreateStrategy(providerName);

        var grantedPermissions = await strategy.GetAsync(
            applicationId, clientId, providerName, providerKey, cancellationToken);

        var permissions = await permissionRepository.GetListAsync(
            applicationId,
            clientId,
            cancellationToken);

        var result = new PermissionListResultDto
        {
            Permissions = new List<PermissionGrantDto>()
        };

        foreach (var permission in permissions)
        {
            var permissionGrantDto = multiLingualMapper.Map(permission);
            permissionGrantDto.IsGranted = grantedPermissions.Any(gp => gp.Name == permission.Name);
            permissionGrantDto.ParentName = Permission.ExtractParentName(permission.Name);
            permissionGrantDto.Provider = grantedPermissions.FirstOrDefault(gp => gp.Name == permission.Name)?.Provider;
            result.Permissions.Add(permissionGrantDto);
        }

        return result;
    }

    public async Task UpdateAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        UpdatePermissionInput input,
        CancellationToken cancellationToken = default)
    {
        var permissionsToGrant = new List<PermissionGrant>();
        var permissionsToDeny = new List<PermissionGrant>();

        foreach (var permissionDto in input.Permissions)
        {
            var existingGrant = await permissionGrantRepository
                .FindAsync(
                    applicationId,
                    clientId,
                    providerName,
                    providerKey,
                    permissionDto.Name,
                    cancellationToken);

            if (permissionDto.IsGranted)
            {
                if (existingGrant == null)
                {
                    var newGrant = new PermissionGrant(
                        GuidGenerator.Create(),
                        applicationId,
                        clientId,
                        permissionDto.Name,
                        providerName,
                        providerKey);

                    permissionsToGrant.Add(newGrant);
                }
            }
            else
            {
                if (existingGrant != null)
                {
                    permissionsToDeny.Add(existingGrant);
                }
            }
        }

        await permissionManager.BulkGrantAsync(permissionsToGrant, cancellationToken);
        await permissionManager.BulkDenyAsync(permissionsToDeny, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<CheckPermissionResultDto> CheckAsync(
        string applicationId,
        string clientId,
        string providerName,
        string providerKey,
        string name,
        CancellationToken cancellationToken = default)
    {
        return new CheckPermissionResultDto(await permissionManager.CheckAsync(
            applicationId,
            clientId,
            providerName,
            providerKey,
            name,
            cancellationToken)
        );
    }
}
