namespace Kartverket.Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusConfirmationMunicipalDOK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RegisterItems", "StatusConfirmationMunicipalDOK", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RegisterItems", "StatusConfirmationMunicipalDOK");
        }
    }
}
