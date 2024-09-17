using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BBT.Prism.Data.Seeding;
using BBT.Prism.Uow;
using BBT.Resource.Permissions;
using BBT.Resource.Policies;
using BBT.Resource.Resources;
using BBT.Resource.Roles;
using BBT.Resource.Rules;

namespace BBT.Resource.EntityFrameworkCore;

public class ResourceTestDataSeedContributor(
    TestData testData,
    IUnitOfWork unitOfWork,
    IRuleRepository ruleRepository,
    IRoleDefinitionRepository roleDefinitionRepository,
    IRoleGroupRepository roleGroupRepository,
    IRoleRepository roleRepository,
    IResourceGroupRepository resourceGroupRepository,
    IPolicyRepository policyRepository,
    IResourceRepository resourceRepository,
    IPermissionRepository permissionRepository,
    IPermissionGrantRepository permissionGrantRepository) : IDataSeedContributor
{
    public async Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */
        await permissionRepository.InsertManyAsync(GetPermissions());

        await permissionGrantRepository.InsertManyAsync(GetPermissionGrants());

        await ruleRepository.InsertManyAsync(GetRules());

        await roleDefinitionRepository.InsertManyAsync(GetRoleDefinitions());

        await roleRepository.InsertManyAsync(GetRoles());

        await roleGroupRepository.InsertManyAsync(GetRoleGroups());

        await policyRepository.InsertManyAsync(GetPolicies());

        await resourceGroupRepository.InsertManyAsync(GetResourceGroups());

        await resourceRepository.InsertManyAsync(GetResources());
        await unitOfWork.SaveChangesAsync();
    }

    private List<Permission> GetPermissions()
    {
        return
        [
            //Identities
            new Permission(
                testData.PermissionId_1,
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_1),
            //Identities.Users
            new Permission(
                testData.PermissionId_2,
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_2,
                testData.PermissionId_1),
            //Identities.Users.Create
            new Permission(
                testData.PermissionId_3,
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_3,
                testData.PermissionId_2),
            //Identities.Users.Update
            new Permission(
                testData.PermissionId_4,
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_4,
                testData.PermissionId_2),
            //Identities.Users.Roles
            new Permission(
                testData.PermissionId_5,
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_5,
                testData.PermissionId_2),
            //Identities.Users.Roles.Assign
            new Permission(
                testData.PermissionId_6,
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_6,
                testData.PermissionId_5),
        ];
    }

    private List<PermissionGrant> GetPermissionGrants()
    {
        return
        [
            new PermissionGrant(
                Guid.NewGuid(),
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_1,
                testData.ProviderName,
                testData.ProviderKey
            ),
            new PermissionGrant(
                Guid.NewGuid(),
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_2,
                testData.ProviderName,
                testData.ProviderKey
            ),
            new PermissionGrant(
                Guid.NewGuid(),
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_3,
                testData.ProviderName,
                testData.ProviderKey
            ),
            new PermissionGrant(
                Guid.NewGuid(),
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_4,
                testData.ProviderName,
                testData.ProviderKey
            ),
            new PermissionGrant(
                Guid.NewGuid(),
                testData.ApplicationId,
                testData.ClientId,
                testData.PermissionName_5,
                testData.ProviderName,
                testData.ProviderKey
            )
        ];
    }

    private List<Rule> GetRules()
    {
        return
        [
            new Rule(testData.RuleId_1, "Rule_1", "header.client_id == \"test_1\" || header.client_id == \"test_2\""),
            new Rule(testData.RuleId_2, "Rule_2",
                "Utils.CallApiGet(\"https://google.com/card/security/match/\"+header.user_reference+\"/\"+header.card_number,header).IsSuccessStatusCode == true")
        ];
    }

    private List<RoleDefinition> GetRoleDefinitions()
    {
        var mock1 = new RoleDefinition(testData.RoleDefinitionId_1, "FullAuthorized",
            Guid.Parse("e623c6ec-6023-4a2e-85c2-1b89deebad13"));
        mock1.AddTranslation("en-US", "Full Authorized", "Full authorized");
        mock1.AddTranslation("tr-TR", "Tam Yetkili", "Tüm yetkilere sahip");

        var mock2 = new RoleDefinition(testData.RoleDefinitionId_2, "Viewer",
            Guid.Parse("e623c6ec-6023-4a2e-85c2-1b89deebad13"));
        mock2.AddTranslation("en-US", "Viewer", "Viewer");
        mock2.AddTranslation("tr-TR", "Gözlemci", "Sadece okuma yetkisine sahip");

        return
        [
            mock1, mock2
        ];
    }

    private List<Role> GetRoles()
    {
        var mock1 = new Role(testData.RoleId_1, testData.RoleDefinitionId_1);
        mock1.AddTranslation("en-US", "Full Authorized");
        mock1.AddTranslation("tr-TR", "Tam Yetkili");

        var mock2 = new Role(testData.RoleId_2, testData.RoleDefinitionId_2);
        mock2.AddTranslation("en-US", "Viewer");
        mock2.AddTranslation("tr-TR", "Gözlemci");

        return
        [
            mock1, mock2
        ];
    }

    private List<RoleGroup> GetRoleGroups()
    {
        var mock1 = new RoleGroup(testData.RoleGroupId_1);
        mock1.AddTranslation("en-US", "Internet Banking");
        mock1.AddTranslation("tr-TR", "İnternet Bankacılığı");

        mock1.AddRole(testData.RoleId_1);
        mock1.AddRole(testData.RoleId_2);

        var mock2 = new RoleGroup(testData.RoleGroupId_2);
        mock2.AddTranslation("en-US", "Human Resources");
        mock2.AddTranslation("tr-TR", "İnsan Kaynakları");

        return
        [
            mock1, mock2
        ];
    }

    private List<Policy> GetPolicies()
    {
        var policy_1 = new Policy(testData.PolicyId_1, "Policy_1", Effect.Allow, 1);
        policy_1.SetPermissions("U", ["AD.Management", "AD.Organization.Management"]);
        policy_1.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary()
            {
                { "department", "IT" },
                { "clearanceLevel", 5 }
            },
            roles: new[] { "admin" },
            rules: new[] { testData.RuleId_1.ToString(), testData.RuleId_2.ToString() },
            time: null);

        var policy_2 = new Policy(testData.PolicyId_2, "Policy_2", Effect.Allow, 2);
        policy_2.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary()
            {
                { "department", "Integration" },
                { "position", "Expert" }
            },
            roles: new[] { "viewer" },
            rules: new[] { "" },
            time: null);

        var policy_3 = new Policy(testData.PolicyId_3, "Policy_3", Effect.Allow, 3);
        policy_3.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary(),
            roles: new[] { "" },
            rules: new[] { "" },
            time: new PolicyTime(
                new TimeOnly(9, 0, 0),
                new TimeOnly(18, 0, 0)
            ));

        var policy_4 = new Policy(testData.PolicyId_4, "Policy_4", Effect.Allow, 4);
        policy_4.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary(),
            roles: new[] { "assign_team" },
            rules: new[] { "" },
            time: null);

        var policy_5 = new Policy(testData.PolicyId_5, "Policy_5", Effect.Allow, 4);
        policy_5.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary(),
            roles: new[] { "" },
            rules: new[] { testData.RuleId_1.ToString(), testData.RuleId_2.ToString() },
            time: null);

        return
        [
            policy_1, policy_2, policy_3, policy_4, policy_5
        ];
    }

    private List<ResourceGroup> GetResourceGroups()
    {
        var mock1 = new ResourceGroup(testData.ResourceGroupId_1);
        mock1.AddTranslation("en-US", "Users");
        mock1.AddTranslation("tr-TR", "Kullanıcılar");

        var mock2 = new ResourceGroup(testData.ResourceGroupId_2);
        mock2.AddTranslation("en-US", "Token");
        mock2.AddTranslation("tr-TR", "Token");

        return [mock1, mock2];
    }

    private List<Resources.Resource> GetResources()
    {
        var mock1 = new Resources.Resource(testData.ResourceId_1, testData.ResourceGroupId_1, ResourceType.GET,
            "/api/users/([^/]+)");
        mock1.AddTranslation("en-US", "Get User", "Get user by id");
        mock1.AddTranslation("tr-TR", "Kullanıcı Getir", "Id'ye göre kullanıcı getir");

        mock1.AddRule(GetRules()[0].Id);
        mock1.AddRule(GetRules()[1].Id, null, 2);
        mock1.AddPolicy(GetPolicies()[0].Id, new[] { testData.ClientId_1.ToString(), testData.ClientId_2.ToString() });
        mock1.AddPolicy(GetPolicies()[1].Id, new[] { testData.ClientId_1.ToString(), testData.ClientId_2.ToString() });

        var mock2 = new Resources.Resource(testData.ResourceId_2, testData.ResourceGroupId_1, ResourceType.POST,
            "/api/users([^/]+)");
        mock2.AddTranslation("en-US", "Create User", "Create a new user");
        mock2.AddTranslation("tr-TR", "Kullanıcı Oluştur", "Yeni bir kullanıcı oluştur");

        mock2.AddRule(GetRules()[1].Id);
        mock1.AddPolicy(GetPolicies()[0].Id, new[] { testData.ClientId_3.ToString(), testData.ClientId_4.ToString() });
        mock1.AddPolicy(GetPolicies()[1].Id, new[] { testData.ClientId_3.ToString(), testData.ClientId_4.ToString() });
        mock1.AddPolicy(GetPolicies()[2].Id, new[] { testData.ClientId_4.ToString(), testData.ClientId_5.ToString() });

        var mock3 = new Resources.Resource(testData.ResourceId_3, testData.ResourceGroupId_1, ResourceType.GET,
            "/api/users/([^/]+)/roles");
        mock3.AddTranslation("en-US", "Get User Roles", "Get user roles");
        mock3.AddTranslation("tr-TR", "Kullanıcının Rollerini Listele", "Kullanıcının rollerini listeler");
        //No rules

        return [mock1, mock2, mock3];
    }
}
