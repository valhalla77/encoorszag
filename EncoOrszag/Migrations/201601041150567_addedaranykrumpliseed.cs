namespace EncoOrszag.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedaranykrumpliseed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orszags", "Krumpli", c => c.Int(nullable: false));
            AddColumn("dbo.Orszags", "Arany", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orszags", "Arany");
            DropColumn("dbo.Orszags", "Krumpli");
        }
    }
}
