// <auto-generated />
namespace Membership.Library.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class Add_Audit_Fields_To_User_Table : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(Add_Audit_Fields_To_User_Table));
        
        string IMigrationMetadata.Id
        {
            get { return "201608231243045_Add_Audit_Fields_To_User_Table"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
