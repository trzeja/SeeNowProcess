using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Workers.Models;

namespace Workers.DataAccessLayer
{
    public class WorkersInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<WorkersContext>
    {
        protected override void Seed(WorkersContext context)
        {
            var workers = new List<Worker>
            {
                new Worker { FirstName="Kaj",     LastName="Kajak",              SupervisorId=null},
                new Worker { FirstName="Marcin",  LastName="Cokolwiek",          SupervisorId=1 },
                new Worker { FirstName="Barbara", LastName="Kogut",              SupervisorId=1 },
                new Worker { FirstName="Hieronim",LastName="Brzęczyszczykiewicz",SupervisorId=2 }
            };
            workers.ForEach(w =>
            {
                context.Workers.Add(w);
                context.SaveChanges();
            });
        }
    }
}