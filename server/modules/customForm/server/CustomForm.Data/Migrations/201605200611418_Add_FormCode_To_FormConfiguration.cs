namespace CustomForm.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_FormCode_To_FormConfiguration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormConfiguration", "FormCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormConfiguration", "FormCode");
        }
    }
}
