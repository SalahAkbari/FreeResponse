using Abp.Application.Services.Dto;
using AutoMapper;
using FreeResponse.Application.MapperConfiguration;
using FreeResponse.Application.Projects;
using FreeResponse.Application.Projects.Dto;
using FreeResponse.Application.SdlcSystems;
using FreeResponse.Core.Projects;
using FreeResponse.Core.SdlcSystems;
using FreeResponse.EntityFrameworkCore.Repositories;
using FreeResponse.Tests.Utilities;
using FreeResponse.Web.Host.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace FreeResponse.Tests
{
    [TestClass]
    public class ProjectControllerTest
    {
        protected readonly ProjectController _controller;
        protected readonly Mock<IRepository<Project>> _mockRepo;
        protected readonly Mock<IRepository<SdlcSystem>> _mockSystemRepo;
        protected readonly IMapper mapper;

        public ProjectControllerTest()
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            mapper = mappingConfig.CreateMapper();

            _mockRepo = new Mock<IRepository<Project>>();
            _mockSystemRepo = new Mock<IRepository<SdlcSystem>>();

            IProjectAppService _projectAppService = new ProjectAppService(_mockRepo.Object, mapper);
            ISdlcSystemAppService _sdlcSystemAppService = new SdlcSystemAppService(_mockSystemRepo.Object, mapper);

            _controller = new ProjectController(_projectAppService, _sdlcSystemAppService);

            _mockRepo.Setup(m => m.GetAll())
                    .Returns(Task.FromResult(MockData.Current.Projects.AsEnumerable()));

            _mockRepo.Setup(x => x.Save()).Returns(true);
        }

        protected void MoqSetup(int projectId)
        {
            _mockRepo.Setup(x => x.Get(It.Is<int>(y => y == projectId)))
                .Returns(Task.FromResult(MockData.Current.Projects
                    .FirstOrDefault(p => p.Id.Equals(projectId))));
        }

        protected void MoqSetupSystem(int id)
        {
            _mockSystemRepo.Setup(x => x.Get(It.Is<int>(y => y == id)))
                .Returns(Task.FromResult(MockData.Current.SdlcSystems
                    .FirstOrDefault(p => p.Id.Equals(id))));
        }

        protected void MoqSetupAdd(Project testItem)
        {
            _mockRepo.Setup(x => x.Add(It.Is<Project>(y => y == testItem)))
                .Callback<Project>(s => MockData.Current.Projects.Add(s));
        }

        #region GetTests
        [TestCategory("Get"), TestMethod]
        public async Task Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.Get();

            // Assert
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
        }

        [TestCategory("Get"), TestMethod]
        public async Task Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = await _controller.Get() as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(okResult?.Value, typeof(ListResultDto<ProjectDto>));

            Assert.AreEqual(4, ((ListResultDto<ProjectDto>)okResult?.Value).Items.Count);

        }

        [TestCategory("Get"), TestMethod]
        public async Task GetById_UnknownProjectIdPassed_ReturnsNotFoundResult()
        {
            //Arrange
            MoqSetup(50);

            // Act
            var notFoundResult = await _controller.Get(50);

            // Assert
            Assert.IsInstanceOfType(notFoundResult, typeof(NotFoundResult));

        }

        [TestCategory("Get"), TestMethod]
        public async Task GetById_ExistingProjectIdPassed_ReturnsOkResult()
        {
            // Arrange
            const int testProjectId = 2;
            MoqSetup(testProjectId);

            // Act
            var okResult = await _controller.Get(testProjectId);

            // Assert
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));

        }

        [TestCategory("Get"), TestMethod]
        public async Task GetById_ExistingProjectIdPassed_ReturnsRightItem()
        {
            // Arrange
            var testProjectId = 2;
            MoqSetup(2);

            // Act
            var okResult = await _controller.Get(testProjectId) as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(okResult?.Value, typeof(ProjectDto));

            Assert.AreEqual(testProjectId, ((ProjectDto)okResult.Value).Id);
        }

        #endregion

        #region AddTests

        [TestCategory("Add"), TestMethod]

        public async Task Add_NullObjectPassed_ReturnsBadRequest()
        {
            // Act
            var badResponse = await _controller.CreateProject(null);

            // Assert
            Assert.IsInstanceOfType(badResponse, typeof(BadRequestResult));

        }

        [TestCategory("Add"), TestMethod]
        public async Task Add_Project_ReturnsCreatedResponse()
        {
            // Arrange
            const int SdlcSystemId = 2;
            var theItem = new Project
            {
                Name = "project75",
                ExternalId = "project75",

                Id = 12,
                SdlcSystemId = SdlcSystemId
            };

            MoqSetupAdd(theItem);
            MoqSetupSystem(SdlcSystemId);

            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            //See how the ValidateViewModel extension method in the Helper class is useful here
            _controller.ValidateViewModel(createdDto);
            //I have used the above useful extension method to simulate validation instead of adding customly like below
            //_controller.ModelState.AddModelError("ExternalId", "Required");

            var theResponse = await _controller.CreateProject(createdDto);

            // Assert
            Assert.IsInstanceOfType(theResponse, typeof(CreatedAtRouteResult));

        }

        [TestCategory("Add"), TestMethod]
        public async Task Add_MissingExternalIdPassed_ReturnsBadRequest()
        {
            // Arrange
            var theItem = new Project
            {
                Name = "project5",

                Id = 10,
                SdlcSystemId = 1
            };

            MoqSetupAdd(theItem);
            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            _controller.ValidateViewModel(createdDto);

            var badResponse = await _controller.CreateProject(createdDto);

            // Assert
            Assert.IsInstanceOfType(badResponse, typeof(BadRequestObjectResult));

        }

        [TestCategory("Add"), TestMethod]
        public async Task Add_MissingSdlcSystemIdPassed_ReturnsBadRequest()
        {
            // Arrange
            var theItem = new Project
            {
                Name = "project5",
                ExternalId = "project5",
                Id = 10
            };

            MoqSetupAdd(theItem);
            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            _controller.ValidateViewModel(createdDto);

            var badResponse = await _controller.CreateProject(createdDto);

            // Assert
            Assert.IsInstanceOfType(badResponse, typeof(BadRequestObjectResult));

        }

        [TestCategory("Add"), TestMethod]
        public async Task Add_NonExistingSystemPassed_ReturnsNotFoundResult()
        {
            // Arrange
            var theItem = new Project
            {
                Name = "project5",
                ExternalId = "project5",

                Id = 1,
                SdlcSystemId = 12345
            };

            MoqSetupAdd(theItem);
            MoqSetupSystem(1);

            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            _controller.ValidateViewModel(createdDto);

            var notFoundResult = await _controller.CreateProject(createdDto);

            // Assert
            Assert.IsInstanceOfType(notFoundResult, typeof(NotFoundResult));

        }

        [TestCategory("Add"), TestMethod]
        public async Task Add_ConflictingSystemExternalID_ReturnsConflictResult()
        {
            // Arrange
            const int SdlcSystemId = 1;
            var theItem = new Project
            {
                Name = "project5",
                ExternalId = "101",

                Id = 10,
                SdlcSystemId = SdlcSystemId
            };

            MoqSetupAdd(theItem);
            MoqSetupSystem(SdlcSystemId);

            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            _controller.ValidateViewModel(createdDto);

            var conflictObjectResult = await _controller.CreateProject(createdDto);

            // Assert
            Assert.IsInstanceOfType(conflictObjectResult, typeof(ConflictResult));

        }

        #endregion

        #region UpdateTests

        [TestCategory("Update"), TestMethod]
        public async Task Update_ProjectWithFullPayload_ReturnsOkResult()
        {
            // Arrange
            const int SdlcSystemId = 1;

            var theItem = new Project
            {
                Name = "Name-Changed",
                ExternalId = "ExternalId-Changed",
                SdlcSystemId = SdlcSystemId
            };

            MoqSetup(1);
            MoqSetupSystem(SdlcSystemId);

            var createdDto = mapper.Map<ProjectBaseDto>(theItem);

            // Act

            var theResponse = await _controller.UpdateProject(1, createdDto);

            // Assert
            Assert.IsInstanceOfType(theResponse, typeof(OkResult));

        }


        [TestCategory("Update"), TestMethod]
        public async Task Update_ProjectWithOnlySdlcSystemId_ReturnsOkResult()
        {
            // Arrange
            const int SdlcSystemId = 2;

            var theItem = new Project
            {
                SdlcSystemId = SdlcSystemId
            };

            MoqSetup(2);
            MoqSetupSystem(SdlcSystemId);

            var createdDto = mapper.Map<ProjectBaseDto>(theItem);

            // Act

            var theResponse = await _controller.UpdateProject(2, createdDto);

            // Assert
            Assert.IsInstanceOfType(theResponse, typeof(OkResult));

            _mockRepo.Reset();

        }


        [TestCategory("Update"), TestMethod]
        public async Task Update_ProjectWithEmptyPayload_ReturnsOkResult()
        {
            // Arrange
            var theItem = new Project();

            MoqSetup(1);

            var createdDto = mapper.Map<ProjectBaseDto>(theItem);

            // Act

            var theResponse = await _controller.UpdateProject(1, createdDto);

            // Assert
            Assert.IsInstanceOfType(theResponse, typeof(OkResult));

        }

        [TestCategory("Update"), TestMethod]
        public async Task Update_ProjectWithEmptyPayload_NoChangedFields()
        {
            // Arrange
            MoqSetup(1);

            var oldResult = await _controller.Get(1) as OkObjectResult;
            var orginalProject = (ProjectDto)oldResult.Value;

            var theItem = new Project();

            var createdDto = mapper.Map<ProjectBaseDto>(theItem);

            // Act

            await _controller.UpdateProject(1, createdDto);

            var okResult = await _controller.Get(1) as OkObjectResult;
            var changedProject = ((ProjectDto)okResult.Value);

            // Assert
            Assert.AreEqual(orginalProject.SdlcSystemId, changedProject.SdlcSystemId);
            Assert.AreEqual(orginalProject.Name, changedProject.Name);
            Assert.AreEqual(orginalProject.ExternalId, changedProject.ExternalId);
        }

        [TestCategory("Update"), TestMethod]
        public async Task Update_NonExistingSystemPassed_ReturnsNotFoundResult()
        {
            // Arrange
            var theItem = new Project
            {
                SdlcSystemId = 12345
            };

            MoqSetup(1);
            MoqSetupSystem(1);

            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            var notFoundResult = await _controller.UpdateProject(1, createdDto);

            // Assert
            Assert.IsInstanceOfType(notFoundResult, typeof(NotFoundResult));

        }

        [TestCategory("Update"), TestMethod]
        public async Task Update_ConflictingSystem_ReturnsConflictResult()
        {
            // Arrange
            const int SdlcSystemId = 1;

            var theItem = new Project
            {
                SdlcSystemId = SdlcSystemId
            };

            MoqSetup(1);
            MoqSetupSystem(SdlcSystemId);

            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            var conflictObjectResult = await _controller.UpdateProject(1, createdDto);

            // Assert
            Assert.IsInstanceOfType(conflictObjectResult, typeof(ConflictResult));

        }

        [TestCategory("Update"), TestMethod]
        public async Task Update_ConflictingExternalId_ReturnsConflictResult()
        {
            // Arrange
            var theItem = new Project
            {
                ExternalId = "101"
            };

            MoqSetup(1);
            MoqSetupSystem(1);

            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            var conflictObjectResult = await _controller.UpdateProject(1, createdDto);

            // Assert
            Assert.IsInstanceOfType(conflictObjectResult, typeof(ConflictResult));

        }

        [TestCategory("Update"), TestMethod]
        public async Task Update_IllegalPathVariables_ReturnsBadRequest()
        {
            // Arrange
            var theItem = new Project
            {
            };

            MoqSetup(1);
            MoqSetupSystem(1);

            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            var badResponse = await _controller.UpdateProject(0, createdDto);

            // Assert
            Assert.IsInstanceOfType(badResponse, typeof(BadRequestResult));

        }

        [TestCategory("Update"), TestMethod]
        public async Task Update_InvalidPathVariables_ReturnsNotFoundResult()
        {
            // Arrange
            var theItem = new Project
            {
            };

            MoqSetup(1);
            MoqSetupSystem(1);

            var createdDto = mapper.Map<ProjectDto>(theItem);

            // Act

            var notFoundResult = await _controller.UpdateProject(12345, createdDto);

            // Assert
            Assert.IsInstanceOfType(notFoundResult, typeof(NotFoundResult));

        }

        [TestCategory("Update"), TestMethod]
        public async Task Update_ProjectWithOnlyExternalId_SameNameAndSystemButExternalId()
        {
            // Arrange
            const int SdlcSystemId = 1;

            var orginalProject = new Project
            {
                Name = "project4",
                SdlcSystemId = SdlcSystemId,
                ExternalId = "104"
            };

            var theItem = new Project
            {
                ExternalId = "ExternalId-Changed"
            };

            MoqSetup(4);
            MoqSetupSystem(SdlcSystemId);

            var createdDto = mapper.Map<ProjectBaseDto>(theItem);

            // Act

            await _controller.UpdateProject(4, createdDto);

            var okResult = await _controller.Get(4) as OkObjectResult;
            var changedProject = ((ProjectDto)okResult.Value);

            // Assert
            Assert.AreEqual(orginalProject.SdlcSystemId, changedProject.SdlcSystemId);
            Assert.AreEqual(orginalProject.Name, changedProject.Name);
            Assert.AreNotEqual(orginalProject.ExternalId, changedProject.ExternalId);

            _mockRepo.Reset();
        }

        [TestCategory("Update"), TestMethod]
        public async Task Update_ProjectWithOnlyExternalId_ReturnsOkResult()
        {
            // Arrange
            var theItem = new Project
            {
                ExternalId = "ExternalId-Changed"
            };

            MoqSetup(4);
            MoqSetupSystem(1);

            var createdDto = mapper.Map<ProjectBaseDto>(theItem);

            // Act

            var theResponse = await _controller.UpdateProject(4, createdDto);

            // Assert
            Assert.IsInstanceOfType(theResponse, typeof(OkResult));

        }

        [TestCategory("Update"), TestMethod]
        public async Task Update_ProjectWithOnlySdlcSystemId_SameNameAndExternalIdButSystemId()
        {
            // Arrange
            const int SdlcSystemId = 2;

            var orginalProject = new Project
            {
                Name = "project3",
                ExternalId = "103"
            };

            var theItem = new Project
            {
                SdlcSystemId = SdlcSystemId
            };

            MoqSetup(3);
            MoqSetupSystem(SdlcSystemId);

            var createdDto = mapper.Map<ProjectBaseDto>(theItem);

            // Act

            await _controller.UpdateProject(3, createdDto);

            var okResult = await _controller.Get(3) as OkObjectResult;
            var changedProject = ((ProjectDto)okResult.Value);

            // Assert
            Assert.AreNotEqual(orginalProject.SdlcSystemId, changedProject.SdlcSystemId);
            Assert.AreEqual(orginalProject.Name, changedProject.Name);
            Assert.AreEqual(orginalProject.ExternalId, changedProject.ExternalId);

        }

        #endregion
    }
}
