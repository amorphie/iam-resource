using System;

namespace BBT.Resource;

public class TestData
{
    public Guid RuleId_1 { get; } = Guid.Parse("d71ebb5f-a80a-4b2f-974d-396ed06fb686");
    public Guid RuleId_2 { get; } = Guid.Parse("7a5c732a-db4c-4e92-8847-2169617abc5a");

    public Guid RoleDefinitionId_1 { get; } = Guid.Parse("5140217c-6891-4355-891f-d022f1c8fe98");
    public Guid RoleDefinitionId_2 { get; } = Guid.Parse("f3b4e91b-2621-4eab-ad04-8cb2283e7485");
    
    public Guid RoleId_1 { get; } = Guid.Parse("96465ada-8410-46b0-baf8-d2999ddd2391");
    public Guid RoleId_2 { get; } = Guid.Parse("cd4163f5-ca5a-4170-8efc-17c91da7243e");
    
    public Guid RoleGroupId_1 { get; } = Guid.Parse("f0a1743e-5415-4576-8cb9-264182027c91");
    public Guid RoleGroupId_2 { get; } = Guid.Parse("1faa70de-4245-4649-bd9f-1f23c91d4a89");
    
    public Guid ResourceGroupId_1 { get; } = Guid.Parse("d9fb3896-a6f2-41f6-9693-4d4573e73916");
    public Guid ResourceGroupId_2 { get; } = Guid.Parse("11f4df22-5e3a-48b0-8ff1-efe6aafd7384");
    
    public Guid ResourceId_1 { get; } = Guid.Parse("2ae96097-6198-48c2-a408-3ee32ef7e1c9");
    public Guid ResourceId_2 { get; } = Guid.Parse("9d87bb21-c1d9-4716-8f56-547c1d9339b6");
    public Guid ResourceId_3 { get; } = Guid.Parse("d62eaade-3861-476e-bcee-6213f17310c9");
    
    public Guid PolicyId_1 { get; } = Guid.Parse("f0a8e2b7-30bc-4737-983b-09e2dfe528ae");
    public Guid PolicyId_2 { get; } = Guid.Parse("6939290f-3807-4ca2-955a-21382511e18e");
    public Guid PolicyId_3 { get; } = Guid.Parse("830ec4dd-bf0d-4cfc-84ce-153267210a80");
    public Guid PolicyId_4 { get; } = Guid.Parse("fd63cdac-44eb-4254-9781-68abb4d051bb");
    public Guid PolicyId_5 { get; } = Guid.Parse("2d83457d-b979-48c2-b47d-8f1b9ae29f59");
    
    public Guid ClientId_1 { get; } = Guid.Parse("b51754b7-353f-4847-a5bf-6287c980b944");
    public Guid ClientId_2 { get; } = Guid.Parse("a3011b23-3f20-487c-b7a8-4eb7935a7ff6");
    public Guid ClientId_3 { get; } = Guid.Parse("ce5a92eb-2ea7-483e-892b-6af387255dde");
    public Guid ClientId_4 { get; } = Guid.Parse("115aabaf-129d-466f-a76f-7cf4980c9bc8");
    public Guid ClientId_5 { get; } = Guid.Parse("fde7f372-c2c7-4ce6-a581-9be3efa1ff59");

    public string ApplicationId { get; } = "Burgan";
    public string ClientId { get; } = "burgan_mobile";
    public Guid PermissionId_1 { get; } = Guid.Parse("182e8a60-10e1-4b37-b67a-d8e8c3913471");
    public Guid PermissionId_2 { get; } = Guid.Parse("4a009d33-178e-4061-801e-cbb91d715737");
    public Guid PermissionId_3 { get; } = Guid.Parse("f7ad3e63-9f60-4d62-bde4-12bcbf33e993");
    public Guid PermissionId_4 { get; } = Guid.Parse("d56d7413-b83a-49bc-941f-fa27fdfe3518");
    public Guid PermissionId_5 { get; } = Guid.Parse("768b0e37-55b2-47c3-975f-f0b9deee213c");
    public Guid PermissionId_6 { get; } = Guid.Parse("c74c1bc3-a814-42cd-8b69-e695addb7e62");

    public string PermissionName_1 { get; } = "Identities";
    public string PermissionName_2 { get; } = "Identities.Users";
    public string PermissionName_3 { get; } = "Identities.Users.Create";
    public string PermissionName_4 { get; } = "Identities.Users.Update";
    public string PermissionName_5 { get; } = "Identities.Users.Roles";
    public string PermissionName_6 { get; } = "Identities.Users.Roles.Assign";

    public string ProviderName { get; } = "U";
    public string ProviderKey { get; } = "c74c1bc3-a814-42cd-8b69-e695addb7e62";
}
