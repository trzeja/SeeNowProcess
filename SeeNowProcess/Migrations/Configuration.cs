namespace SeeNowProcess.Migrations
{
    using DAL;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SeeNowProcess.DAL.SeeNowContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SeeNowProcess.DAL.SeeNowContext context)
        {            
            try
            {
                SeeNowInitializer initializer = new SeeNowInitializer();
                initializer.InitializeDatabase(context);
            }
            catch (Exception)
            {
                throw new Exception("Error in SeeNowInitializer. Did you filled all fields required in Seed(Add) methods?");                
            }            
        }
    }
}
