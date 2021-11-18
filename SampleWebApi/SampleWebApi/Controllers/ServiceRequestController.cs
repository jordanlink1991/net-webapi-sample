using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.DAL;
using SampleWebApi.Models;

namespace SampleWebApi.Controllers
{
	[Route("api/servicerequest")]
	[ApiController]
	public class ServiceRequestController : ControllerBase
	{
		#region Constants
		private static readonly string ERROR_MESSAGE_NOT_FOUND = "Service request with provided ID not found";
		private static readonly string ERROR_MESSAGE_DIFFERENT_IDS = "The service request ID and data do not match";
		#endregion Constants

		#region Private Attributes
		/// <summary>
		/// Context object for database access
		/// </summary>
		private DatabaseContext Context { get; set; }
		#endregion Private Attributes

		#region Constructors
		public ServiceRequestController(DatabaseContext databaseContext)
		{
			Context = databaseContext;
			Context.Database.OpenConnection();
			Context.Database.EnsureCreated();
			// TODO close database connection
		}
		#endregion Constructors

		#region Public Methods
		/// <summary>
		/// Retrieve all service requests
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult<List<ServiceRequest>>> Get()
		{
			int c = await Context.ServiceRequests.CountAsync();
			if (c == 0)
				return NoContent();

			return await Context.ServiceRequests.ToListAsync();
		}

		/// <summary>
		/// Retrieve a service request by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<ServiceRequest>> Get(Guid id)
		{
			// if (Context.ServiceRequests.Count() == 0)
			//	 return NoContent();

			ServiceRequest req = await Context.ServiceRequests.FindAsync(id);
			if (req == null)
				return NotFound(ERROR_MESSAGE_NOT_FOUND);

			return req;
		}

		/// <summary>
		/// Add a service request
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Add(ServiceRequest request)
		{
			Context.ServiceRequests.Add(request);

			try
			{
				await Context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

			return Ok();
		}

		/// <summary>
		/// Update a service request
		/// </summary>
		/// <param name="updRequest"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, ServiceRequest updRequest)
		{
			if (!id.Equals(updRequest.ID))
				return BadRequest(ERROR_MESSAGE_DIFFERENT_IDS);

			ServiceRequest req = await Context.ServiceRequests.FindAsync(id);
			if (req == null)
				return NotFound(ERROR_MESSAGE_NOT_FOUND);

			// TODO update via reflection
			req.BuildingCode = updRequest.BuildingCode;
			req.Description = updRequest.Description;
			req.CurrentStatus = updRequest.CurrentStatus;
			req.LastModifiedBy = updRequest.LastModifiedBy;
			req.LastModifiedDate = updRequest.LastModifiedDate; // DateTime.Now?

			try
			{
				await Context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

			return NoContent();
		}

		/// <summary>
		/// Remove a service request
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			ServiceRequest req = await Context.ServiceRequests.FindAsync(id);
			if (req == null)
				return NotFound(ERROR_MESSAGE_NOT_FOUND);

			Context.ServiceRequests.Remove(req);

			try
			{
				await Context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

			return NoContent();
		}
		#endregion Public Methods
	}
}
