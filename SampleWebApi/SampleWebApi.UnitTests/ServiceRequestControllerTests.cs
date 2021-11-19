using System;
using Xunit;
using SampleWebApi.Controllers;
using SampleWebApi.Models;
using SampleWebApi.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SampleWebApi.UnitTests
{
	public class ServiceRequestControllerTests
	{
		#region Private Attributes
		/// <summary>
		/// Database Context used by the controller
		/// </summary>
		private DatabaseContext DatabaseContext { get; set; }
		/// <summary>
		/// Controller that is the subject of this class' tests
		/// </summary>
		private ServiceRequestController ServiceRequestController { get; set; }
		#endregion Private Attributes

		#region Constructors
		public ServiceRequestControllerTests()
		{
			var builder = new DbContextOptionsBuilder<DatabaseContext>()
				.UseSqlite("DataSource=:memory:", x => { });

			DatabaseContext = new DatabaseContext(builder.Options);
			ServiceRequestController = new ServiceRequestController(DatabaseContext);
		}
		#endregion Constructors

		#region Public Methods
		[Fact]
		public async void Get_Should_Return_All_Service_Requests()
		{
			// Arrange
			ServiceRequest request1 = new ServiceRequest();
			ServiceRequest request2 = new ServiceRequest();

			DatabaseContext.ServiceRequests.Add(request1);
			DatabaseContext.ServiceRequests.Add(request2);
			DatabaseContext.SaveChanges();

			// Act
			ActionResult<List<ServiceRequest>> getResult = await ServiceRequestController.Get();
			List<ServiceRequest> requests = getResult?.Value;

			// Assert
			Assert.NotNull(getResult);
			Assert.Null(getResult.Result);
			Assert.NotNull(requests);
			Assert.True(requests.Count() == 2);
			Assert.Contains(requests, x => x.ID == request1.ID);
			Assert.Contains(requests, x => x.ID == request2.ID);
		}

		[Fact]
		public async void Get_Should_Return_204_If_No_Content()
		{
			// Act
			ActionResult<List<ServiceRequest>> getResult = await ServiceRequestController.Get();
			List<ServiceRequest> requests = getResult?.Value;
			ActionResult result = getResult?.Result;
			NoContentResult noContentResult = result as NoContentResult;

			// Assert
			Assert.NotNull(getResult);
			Assert.Null(requests);
			Assert.NotNull(result);
			Assert.NotNull(noContentResult);
			Assert.Equal(204, noContentResult.StatusCode);
		}

		[Fact]
		public async void Get_Should_Return_Requested_Id()
		{
			// Arrange
			ServiceRequest request1 = new ServiceRequest();
			ServiceRequest request2 = new ServiceRequest();

			DatabaseContext.ServiceRequests.Add(request1);
			DatabaseContext.ServiceRequests.Add(request2);
			DatabaseContext.SaveChanges();

			// Act
			ActionResult<ServiceRequest> getResult = await ServiceRequestController.Get(request1.ID);
			ServiceRequest retrievedRequest = getResult?.Value;

			// Assert
			Assert.NotNull(getResult);
			Assert.Null(getResult.Result);
			Assert.NotNull(retrievedRequest);
			Assert.Equal(request1.ID, retrievedRequest.ID);
			Assert.NotEqual(request2.ID, retrievedRequest.ID);
		}

		[Fact]
		public async void Get_Should_Return_404_If_Not_Found()
		{
			// Arrange
			ServiceRequest request1 = new ServiceRequest();
			ServiceRequest request2 = new ServiceRequest();
			ServiceRequest request3 = new ServiceRequest();

			DatabaseContext.ServiceRequests.Add(request1);
			DatabaseContext.ServiceRequests.Add(request2);
			DatabaseContext.SaveChanges();

			// Act
			ActionResult<ServiceRequest> getResult = await ServiceRequestController.Get(request3.ID);
			ServiceRequest request = getResult?.Value;
			ActionResult result = getResult?.Result;
			NotFoundObjectResult notFoundResult = result as NotFoundObjectResult;

			// Assert
			Assert.NotNull(getResult);
			Assert.Null(request);
			Assert.NotNull(result);
			Assert.NotNull(notFoundResult);
			Assert.Equal(404, notFoundResult.StatusCode);
		}

		[Fact]
		public async void Post_Should_Create_New()
		{
			// Arrange
			ServiceRequest request = new ServiceRequest();

			// Act
			IActionResult postResult = await ServiceRequestController.Add(request);
			ServiceRequest insertedRequest = DatabaseContext.ServiceRequests.FirstOrDefault();

			// Assert
			Assert.NotNull(postResult);
			Assert.NotNull(insertedRequest);
			Assert.Equal(request.ID, insertedRequest.ID);
		}

		[Fact]
		public async void Post_Should_Return_400_On_Bad_Request()
		{
			// Arrange
			ServiceRequest request1 = new ServiceRequest();
			DatabaseContext.Add(request1);
			DatabaseContext.SaveChanges();

			ServiceRequest request2 = new ServiceRequest();
			request2.ID = request1.ID; // Match keys to existing record

			// Act
			IActionResult postResult = await ServiceRequestController.Add(request2);
			BadRequestObjectResult badRequestResult = postResult as BadRequestObjectResult;

			// Assert
			Assert.NotNull(postResult);
			Assert.NotNull(badRequestResult);
			Assert.Equal(400, badRequestResult.StatusCode);
		}

		[Fact]
		public async void Put_Should_Update_Record()
		{
			// Arrange
			ServiceRequest request1 = new ServiceRequest() {
				BuildingCode = "CODE_1",
				CurrentStatus = ServiceRequestStatus.InProgress,
				Description = "Request 1 Description"
			};
			ServiceRequest request2 = new ServiceRequest() {
				BuildingCode = "CODE_2",
				CurrentStatus = ServiceRequestStatus.Created,
				Description = "Request 2 Description"
			};

			DatabaseContext.ServiceRequests.Add(request1);
			DatabaseContext.ServiceRequests.Add(request2);
			DatabaseContext.SaveChanges();

			string newCode = "NEW_CODE_1";
			ServiceRequestStatus newStatus = ServiceRequestStatus.Complete;
			string newDescription = "Request 1 Complete";

			request1.BuildingCode = newCode;
			request1.CurrentStatus = newStatus;
			request1.Description = newDescription;

			// Act
			IActionResult putResult = await ServiceRequestController.Update(request1.ID, request1);
			ServiceRequest updRequest = DatabaseContext.ServiceRequests.FirstOrDefault(x => x.ID == request1.ID);

			// Assert
			Assert.NotNull(putResult);
			Assert.NotNull(updRequest);
			Assert.Equal(request1.ID, updRequest.ID);
			Assert.NotEqual(request2.ID, updRequest.ID);
			Assert.Equal(newCode, updRequest.BuildingCode);
			Assert.Equal(newStatus, updRequest.CurrentStatus);
			Assert.Equal(newDescription, updRequest.Description);
		}

		[Fact]
		public async void Put_Should_Return_400_On_Bad_Request()
		{
			// Arrange
			ServiceRequest request1 = new ServiceRequest();
			DatabaseContext.Add(request1);
			DatabaseContext.SaveChanges();

			ServiceRequest request2 = new ServiceRequest();

			// Act
			IActionResult putResult = await ServiceRequestController.Update(request1.ID, request2);
			BadRequestObjectResult badRequestResult = putResult as BadRequestObjectResult;

			// Assert
			Assert.NotNull(putResult);
			Assert.NotNull(badRequestResult);
			Assert.NotEqual(request2.ID, request1.ID);
			Assert.Equal(400, badRequestResult.StatusCode);
		}

		[Fact]
		public async void Put_Should_Return_404_If_Not_Found()
		{
			// Arrange
			ServiceRequest request1 = new ServiceRequest();
			DatabaseContext.Add(request1);
			DatabaseContext.SaveChanges();

			ServiceRequest request2 = new ServiceRequest();

			// Act
			IActionResult putResult = await ServiceRequestController.Update(request2.ID, request2);
			NotFoundObjectResult notFoundResult = putResult as NotFoundObjectResult;

			// Assert
			Assert.NotNull(putResult);
			Assert.NotNull(notFoundResult);
			Assert.NotEqual(request2.ID, request1.ID);
			Assert.Equal(404, notFoundResult.StatusCode);
		}

		[Fact]
		public async void Delete_Should_Remove_Request()
		{
			// Arrange
			ServiceRequest request1 = new ServiceRequest();
			ServiceRequest request2 = new ServiceRequest();

			DatabaseContext.ServiceRequests.Add(request1);
			DatabaseContext.ServiceRequests.Add(request2);
			DatabaseContext.SaveChanges();

			// Act
			IActionResult deleteResult = await ServiceRequestController.Delete(request1.ID);
			CreatedResult createdResult = deleteResult as CreatedResult;
			ServiceRequest remainingRequest = DatabaseContext.ServiceRequests.FirstOrDefault();

			// Assert
			Assert.NotNull(deleteResult);
			Assert.NotNull(createdResult);
			Assert.NotNull(remainingRequest);
			Assert.Equal(201, createdResult.StatusCode);
			Assert.True(DatabaseContext.ServiceRequests.Count() == 1);
			Assert.NotEqual(remainingRequest.ID, request1.ID);
			Assert.Equal(request2.ID, remainingRequest.ID);
		}

		[Fact]
		public async void Delete_Should_Return_404_If_Not_Found()
		{
			// Arrange
			ServiceRequest request1 = new ServiceRequest();
			DatabaseContext.Add(request1);
			DatabaseContext.SaveChanges();

			ServiceRequest request2 = new ServiceRequest();

			// Act
			IActionResult deleteRequest = await ServiceRequestController.Delete(request2.ID);
			NotFoundObjectResult notFoundResult = deleteRequest as NotFoundObjectResult;

			// Assert
			Assert.NotNull(deleteRequest);
			Assert.NotNull(notFoundResult);
			Assert.NotEqual(request2.ID, request1.ID);
			Assert.Equal(404, notFoundResult.StatusCode);
		}
		#endregion Public Methods
	}
}
