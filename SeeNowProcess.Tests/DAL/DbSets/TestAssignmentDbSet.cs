using SeeNowProcess.Models;
using SeeNowProcess.Tests.DAL.DbSets.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeNowProcess.Tests.DAL.DbSets
{
    class TestAssignmentDbSet:TestDbSet<Assignment>
    {
        public override Assignment Find(params object[] keyValues)
        {
            return this.SingleOrDefault(assignment => assignment.AssignmentID == (int)keyValues.Single());
        }
    }
}
