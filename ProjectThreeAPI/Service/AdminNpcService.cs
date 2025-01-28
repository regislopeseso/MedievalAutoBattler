using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Models.Entities;
using System.Numerics;

namespace ProjectThreeAPI.Service
{
    public class AdminNpcService
    {
        private readonly ApplicationDbContext _daoDBcontext;

        public AdminNpcService(ApplicationDbContext daoDBcontext)
        {
            this._daoDBcontext = daoDBcontext;
        }

    }
}
