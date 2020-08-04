using System;
using System.Collections.Generic;
using System.Text;

namespace FreeResponse.EntityFrameworkCore.EntityFrameworkCore.Seed.SdlcSystem
{
    public class InitialSdlcSystemDbBuilder
    {
        private readonly FreeResponseDbContext _context;

        public InitialSdlcSystemDbBuilder(FreeResponseDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultSdlcSystemCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}

