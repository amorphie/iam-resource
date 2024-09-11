using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BBT.Prism.Data.Seeding;
using BBT.Prism.Uow;
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
    IResourceRepository resourceRepository) : IDataSeedContributor
{
    public async Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */
        await ruleRepository.InsertManyAsync(GetRules());

        await roleDefinitionRepository.InsertManyAsync(GetRoleDefinitions());

        await roleRepository.InsertManyAsync(GetRoles());

        await roleGroupRepository.InsertManyAsync(GetRoleGroups());

        await policyRepository.InsertManyAsync(GetPolicies());

        await resourceGroupRepository.InsertManyAsync(GetResourceGroups());

        await resourceRepository.InsertManyAsync(GetResources());
        await unitOfWork.SaveChangesAsync();
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
        policy_1.SetPermissions(["", ""]);
        policy_1.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary(),
            roles: new[] { "" },
            rules: new[] { "" },
            time: null);

        var policy_2 = new Policy(testData.PolicyId_2, "Policy_2", Effect.Allow, 2);
        policy_2.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary(),
            roles: new[] { "" },
            rules: new[] { "" },
            time: null);
        
        var policy_3 = new Policy(testData.PolicyId_3, "Policy_3", Effect.Allow, 3);
        policy_3.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary(),
            roles: new[] { "" },
            rules: new[] { "" },
            time: null);
        
        var policy_4 = new Policy(testData.PolicyId_4, "Policy_4", Effect.Allow, 4);
        policy_4.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary(),
            roles: new[] { "" },
            rules: new[] { "" },
            time: null);
        
        var policy_5 = new Policy(testData.PolicyId_5, "Policy_5", Effect.Allow, 4);
        policy_5.UpdateCondition(
            context: new ObjectDictionary(),
            attributes: new ObjectDictionary(),
            roles: new[] { "" },
            rules: new[] { "" },
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

        var mock2 = new Resources.Resource(testData.ResourceId_2, testData.ResourceGroupId_1, ResourceType.POST,
            "/api/users([^/]+)");
        mock2.AddTranslation("en-US", "Create User", "Create a new user");
        mock2.AddTranslation("tr-TR", "Kullanıcı Oluştur", "Yeni bir kullanıcı oluştur");

        mock2.AddRule(GetRules()[1].Id);

        var mock3 = new Resources.Resource(testData.ResourceId_3, testData.ResourceGroupId_1, ResourceType.GET,
            "/api/users/([^/]+)/roles");
        mock3.AddTranslation("en-US", "Get User Roles", "Get user roles");
        mock3.AddTranslation("tr-TR", "Kullanıcının Rollerini Listele", "Kullanıcının rollerini listeler");
        //No rules

        return [mock1, mock2, mock3];
    }
}
