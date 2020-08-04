using FreeResponse.Core.Projects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeResponse.EntityFrameworkCore.EntityFrameworkCore.Seed.SdlcSystem
{
    public class DefaultSdlcSystemCreator
    {
        private readonly FreeResponseDbContext _context;

        public DefaultSdlcSystemCreator(FreeResponseDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateSdlcSystems();
        }

        private void CreateSdlcSystems()
        {
            if (!_context.SdlcSystems.Any())
            {
                _context.SdlcSystems.Add(new Core.SdlcSystems.SdlcSystem
                {
                    BaseUrl = "http://jira.company.com",
                    Description = "Company JIRA",
                    CreationTime = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Projects = new List<Project>
                    { 
                        new Project { ExternalId = "SAMPLEPROJECT", Name = "Sample Project" ,
                             },
                        new Project { ExternalId = "PROJECTX", Name = "Project X",
                             },
                    }
                });

                _context.SdlcSystems.Add(new Core.SdlcSystems.SdlcSystem
                {
                    BaseUrl = "http://bugzilla.company.com",
                    Description = "Company BugZilla",
                    CreationTime = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Projects = new List<Project>
                    {
                        new Project { ExternalId = "PROJECTX", Name = "Project X",
                             },
                        new Project { ExternalId = "SAMPLEPROJECT", Name = "SAMPLEPROJECT",
                           }
                    }
                });

                _context.SdlcSystems.Add(new Core.SdlcSystems.SdlcSystem
                {
                    BaseUrl = "http://mantis.company.com",
                    Description = "Company Mantis",
                    CreationTime = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Projects = new List<Project>
                    {
                        new Project { ExternalId = "PROJECTZERO", Name = "PROJECT ZERO",
                             },
                        new Project { ExternalId = "PROJECTONE", Name = "PROJECT One",
                             },
                        new Project { ExternalId = "PROJECTTWO", Name = "PROJECT TWO",
                            },
                        new Project { ExternalId = "PROJECTTHREE", Name = "PROJECTTHREE",
                        }
                    }
                });
            }
        }
    }
}

