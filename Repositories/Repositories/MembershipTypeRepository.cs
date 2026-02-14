using TP.Data;
using TP.Models;
using TP.Repositories.Interfaces;

namespace TP.Repositories;

public class MembershipTypeRepository : GenericRepository<MembershipType>, IMembershipTypeRepository
{
    public MembershipTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}