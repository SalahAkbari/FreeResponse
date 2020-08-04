using FreeResponse.Core.Projects;
using FreeResponse.Core.SdlcSystems;
using System;
using System.Collections.Generic;

namespace FreeResponse.Tests
{
    public class MockData
    {
        public static MockData Current { get; } = new MockData();
        public List<Project> Projects { get; set; }
        public List<SdlcSystem> SdlcSystems { get; set; }


        public MockData()
        {
            SdlcSystems = new List<SdlcSystem>
            {
                new SdlcSystem {Id = 1, BaseUrl = "http://jira.company.com", Description = "Company JIRA" , CreationTime = DateTime.Now, LastModifiedDate = DateTime.Now},
                new SdlcSystem {Id = 2, BaseUrl = "http://bugzilla.company.com", Description = "Company BugZilla" , CreationTime = DateTime.Now, LastModifiedDate = DateTime.Now},
                new SdlcSystem {Id = 3, BaseUrl = "http://mantis.company.com", Description = "Company Mantis" , CreationTime = DateTime.Now, LastModifiedDate = DateTime.Now},
            };

            Projects = new List<Project>
            {
                new Project {Id = 1, SdlcSystemId = 1, Name = "project1" , ExternalId = "101"},
                new Project {Id = 2, SdlcSystemId = 1, Name = "project2" , ExternalId = "102"},
                new Project {Id = 3, SdlcSystemId = 1, Name = "project3" , ExternalId = "103"},
                new Project {Id = 4, SdlcSystemId = 1, Name = "project4" , ExternalId = "104"}
            };
        }
    }
}

