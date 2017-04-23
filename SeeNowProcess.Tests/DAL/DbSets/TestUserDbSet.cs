using SeeNowProcess.Models;
using SeeNowProcess.Tests.DAL.DbSets.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeNowProcess.Tests.DAL.DbSets
{
    class TestUserDbSet : TestDbSet<User>
    {
        public override User Find(params object[] keyValues)
        {
            return this.SingleOrDefault(user => user.UserID == (int)keyValues.Single());
        }
    }
}
