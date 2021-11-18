using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.DAL;
using SampleWebApi.Models;

namespace SampleWebApi.Controllers
{
	[Route("api/servicerequest")]
	[ApiController]
	public class ServiceRequestController : ControllerBase
	{
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
		}

		/// <summary>
		/// Retrieve a service request by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<ServiceRequest>> Get(Guid id)
		{
			return NoContent();
		}

		/// <summary>
		/// Create a service request
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Create(ServiceRequest request)
		{
			return NoContent();
		}

		/// <summary>
		/// Update a service request
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, ServiceRequest request)
		{
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
			return NoContent();
		}
		#endregion Public Methods
	}
}
