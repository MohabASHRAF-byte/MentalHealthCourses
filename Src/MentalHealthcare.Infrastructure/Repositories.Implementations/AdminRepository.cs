using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.implementations
{
    public interface AdminRepository<T> : IAdminRepository<T> where T : Admin
    {







    }
}
